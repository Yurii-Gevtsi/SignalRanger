using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Modes;
using Game.Core.Battle;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Core.Localization;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Feedback;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Net;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Results;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Visuals;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Gameplay.Battle.Assembly
{
    public sealed partial class BattleSceneController
    {
        private static WeaponDefinition[] CreateMissionWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            return allWeapons
                .Where(IsBattleArsenalWeapon)
                .ToArray();
        }

        /// <summary>
        /// Apply battle ammo bonuses from the progression owned by each side.
        /// </summary>
        private static void ApplyProgressionAmmo(WeaponInventoryRuntime inventory, BattleWeaponProgression progression)
        {
            if (inventory == null || progression == null || progression.IsEmpty)
            {
                return;
            }

            foreach (var weapon in inventory.Weapons)
            {
                if (weapon == null)
                {
                    continue;
                }

                if (weapon.WeaponId == "weapon.blaster-caset")
                {
                    inventory.SetRemainingAmmo(weapon, progression.ResolveEvolution(weapon.WeaponId).BlasterBurstShotCount);
                }
                else if (weapon.WeaponId == "weapon.mine")
                {
                    inventory.SetRemainingAmmo(weapon, 4);
                }
                else
                {
                    var loadoutAmmo = ResolveLoadoutBattleAmmo(weapon, progression);
                    if (loadoutAmmo > 0)
                    {
                        inventory.SetRemainingAmmo(weapon, loadoutAmmo);
                    }
                }
            }
        }

        /// <summary>
        /// Battle stock of a limited weapon: its base stock plus one per paid Loadout purchase, within the
        /// catalog limit of that weapon. Returns 0 when the stock is fixed or unlimited.
        /// </summary>
        public static int ResolveLoadoutBattleAmmo(WeaponDefinition weapon, BattleWeaponProgression progression)
        {
            if (weapon == null || progression == null || !weapon.HasLimitedAmmo
                || !WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition)
                || definition.LoadoutUpgradeLimit <= 0)
            {
                return 0;
            }

            var loadoutPurchases = Mathf.Clamp(progression.ResolveUpgradeCount(
                weapon.WeaponId, WeaponUpgradePath.Loadout), 0, definition.LoadoutUpgradeLimit);
            return weapon.AmmoCount + loadoutPurchases;
        }

        private static bool IsBattleArsenalWeapon(WeaponDefinition weapon)
        {
            return weapon != null
                && !WeaponPresentationText.IsDropOnly(weapon)
                && WeaponProgressionBridge.IsWeaponUnlocked(weapon.WeaponId);
        }

        private static WeaponDefinition[] CreateOpponentWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            var byId = allWeapons
                .Where(weapon => weapon != null)
                .ToDictionary(weapon => weapon.WeaponId);

            var selectedIds = GameSessionContext.SelectedOpponentWeaponIds != null && GameSessionContext.SelectedOpponentWeaponIds.Length > 0
                ? GameSessionContext.SelectedOpponentWeaponIds
                : new[] { "weapon.impulse", "weapon.blaster-caset" };

            return selectedIds
                .Where(byId.ContainsKey)
                .Select(id => byId[id])
                .ToArray();
        }

        private void SelectWeapon(int weaponIndex)
        {
            EmitDuelCommand(BattleCommandKind.SelectWeapon, weaponIndex);
            if (_inventory == null || !_inventory.HasWeapons)
            {
                return;
            }

            ClearOrbitalTargetMarker();
            _simulation?.CancelTurnTransfer();
            _inventory.Select(weaponIndex);
            var weapon = _inventory.Selected;
            _aimSettings = weapon.CreateAimSettings(_baseAimSettings);
            ResetAimState();
            _barrierPlacementInitialized = false;
            _barrierPlacementScale = 1f;
            _hasPreviousPinchDistance = false;
            _playerWeaponSelected = true;
            RefreshPlayerWeaponVisual();
            _status = _currentArena != null
                ? GameText.WeaponSelected(DescribeWeapon(weapon))
                : GameText.WeaponSelectedGeneric();
        }

        private void OnHudWeaponSelected(int weaponIndex)
        {
            if (!IsBattleReady()
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || HasProjectilesInFlight
                || HasActiveMotion
                || HasActiveWeaponEffect
                || HasPendingGroundAction
                || _simulation.IsAttackCommitted)
            {
                return;
            }

            var weapon = weaponIndex >= 0 && weaponIndex < _inventory.Count ? _inventory.Weapons[weaponIndex] : null;
            if (weapon == null || (_firstBattleGuide != null && !_firstBattleGuide.AllowsWeapon(weapon.WeaponId)))
            {
                return;
            }

            SelectWeapon(weaponIndex);
            _firstBattleGuide?.ObserveWeaponSelected(weapon.WeaponId);
            _cameraPanActive = false;
        }

        private void OnHudArsenalVisibilityChanged(bool visible)
        {
            _arsenalMenuOpen = visible;
            if (visible)
            {
                ClearWalkInput();
                _cameraPanActive = false;
            }
        }

        private void ResetSelectedWeapon()
        {
            if (!IsBattleReady()
                || !_playerWeaponSelected
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || HasProjectilesInFlight
                || HasActiveMotion
                || HasActiveWeaponEffect
                || (_firstBattleGuide != null && _firstBattleGuide.IsActive))
            {
                return;
            }

            _playerWeaponSelected = false;
            ResetAimState();
            _status = GameText.WeaponReset();
        }

        private void Fire()
        {
            if (!IsBattleReady())
            {
                return;
            }

            if (!_playerWeaponSelected)
            {
                _status = GameText.NoWeaponSelected();
                return;
            }

            if (_firstBattleGuide != null && _firstBattleGuide.IsActive && !_firstBattleGuide.AllowsFire)
            {
                return;
            }

            FireWithAim(_aimState);
        }

        /// <summary>
        /// Fires with an explicit aim, whatever produced it - the local player's
        /// drag, the bot, or a command that arrived over the wire. The checks in
        /// <see cref="Fire"/> guard the local input path only; a remote command has
        /// already been through its own authority checks and must not be refused
        /// because of this client's UI state.
        /// </summary>
        private void FireWithAim(AimState aim)
        {
            if (!IsBattleReady())
            {
                return;
            }

            EmitDuelCommand(BattleCommandKind.Fire, 0, aim.Origin, aim.Direction, aim.Power);

            var result = _simulation.TryFire(aim);
            if (BattleSimulation.IsMarkedStrikeWeapon(result.Weapon)
                && result.Outcome == BattleFireOutcome.UtilityUsed)
            {
                ClearOrbitalTargetMarker();
            }

            // Opening the turn transfer choice shows the whole arena so every ally can be seen.
            if (_simulation.HasTurnTransferSelection && _camera != null && _maximumCameraSize > 0f)
            {
                _camera.orthographicSize = _maximumCameraSize;
                ClampCameraToArena();
            }

            switch (result.Outcome)
            {
                case BattleFireOutcome.NoActiveUnit:
                    HideTrajectoryPreview();
                    break;
                case BattleFireOutcome.AimRequired:
                    _status = GameText.AimRequired();
                    break;
                case BattleFireOutcome.OutOfAmmo:
                    _status = GameText.AmmoDepleted(DescribeWeapon(result.Weapon));
                    break;
                case BattleFireOutcome.NoWeaponSelected:
                    _status = GameText.NoWeaponSelected();
                    break;
                case BattleFireOutcome.Fired:
                    if (result.Team == BattleTeamId.Player)
                    {
                        _firstBattleGuide?.ObservePlayerShotFired();
                        ReportTutorial(BattleTutorialBridge.ShotFired);
                    }

                    break;
                case BattleFireOutcome.BurstFired:
                    SpawnImpactFlash(result.EffectPoint, new Color(1f, 0.62f, 0.24f, 0.85f), 0.95f);
                    if (result.HitCount > 0)
                    {
                        EmitFeedback(BattleFeedbackEventType.DamageResolved);
                    }

                    break;
            }
        }

        private BattleWeaponProgression ProgressionForTeam(BattleTeamId team)
        {
            return team == BattleTeamId.Player ? _playerProgression : _opponentProgression;
        }

        /// <summary>
        /// Evolved art for <paramref name="baseIconId"/> at the rank/branch
        /// <paramref name="progression"/> holds, or the base icon when the weapon has
        /// not evolved or has no evolved art authored.
        /// </summary>
        private static string ResolveEvolvedIconId(
            string baseIconId,
            WeaponDefinition weapon,
            BattleWeaponProgression progression)
        {
            if (weapon == null || progression == null || string.IsNullOrEmpty(baseIconId))
            {
                return baseIconId;
            }

            return progression.ResolveCurrentIconId(weapon, baseIconId);
        }

        private void ClearSelectedWeaponIfDepleted(WeaponDefinition weapon)
        {
            if (weapon == null
                || !weapon.HasLimitedAmmo
                || _inventory?.Selected?.WeaponId != weapon.WeaponId
                || _inventory.GetSelectedRemainingAmmo() > 0
                // The copter's one charge is spent on launch, but its fire button drops the bombs.
                || (weapon.WeaponId is "weapon.bomber" or "weapon.ufo-carrier" && _simulation.HasPlayerCopter))
            {
                return;
            }

            _playerWeaponSelected = false;
            ResetAimState();
            RefreshPlayerWeaponVisual();
        }

        /// <summary>
        /// The player's shot of this turn is spent (only the retreat is left): the held skin and the aim stay hidden
        /// until the turn passes. The blaster burst, the teleport's early trigger and a flying copter still need
        /// their weapon for the follow-up press.
        /// </summary>
        private bool IsPlayerTurnShotSpent()
        {
            return _simulation != null
                && _battleState != null
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && _battleState.Turn.ActionSpent
                && !_simulation.CanContinueBlasterBurst
                && !_simulation.CanTriggerTeleportEarly()
                && !_simulation.HasPlayerCopter;
        }

        private static bool HidesHeldWeaponAfterUse(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.guardian-drone"
                || weapon?.WeaponId == "weapon.granata"
                || weapon?.WeaponId == "weapon.teleport"
                || weapon?.WeaponId == "weapon.mine"
                || weapon?.WeaponId == "weapon.bur-rocket"
                || weapon?.WeaponId == "weapon.titanium-disc"
                || weapon?.WeaponId == "weapon.missle"
                || weapon?.WeaponId == "weapon.fpv-drone"
                || weapon?.WeaponId == "weapon.sticky-grenade"
                || weapon?.WeaponId == "weapon.hopping-grenade"
                || weapon?.WeaponId == "weapon.cryo-grenade"
                || weapon?.WeaponId == "weapon.incendiary-grenade"
                || weapon?.WeaponId == "weapon.laser-fence"
                || weapon?.WeaponId == "weapon.repair-jammer"
                || weapon?.WeaponId == "weapon.virus"
                || weapon?.WeaponId == "weapon.sky-hook"
                || weapon?.WeaponId == "weapon.spider-mine"
                || weapon?.WeaponId == "weapon.bomber"
                || weapon?.WeaponId == "weapon.ufo-carrier";
        }

        private static bool HidesHeldWeaponDuringFlight(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.guardian-drone"
                || weapon?.WeaponId == "weapon.bur-rocket"
                || weapon?.WeaponId == "weapon.titanium-disc"
                || weapon?.WeaponId == "weapon.granata"
                || weapon?.WeaponId == "weapon.teleport"
                || weapon?.WeaponId == "weapon.missle"
                || weapon?.WeaponId == "weapon.sticky-grenade"
                || weapon?.WeaponId == "weapon.hopping-grenade"
                || weapon?.WeaponId == "weapon.cryo-grenade"
                || weapon?.WeaponId == "weapon.incendiary-grenade"
                || weapon?.WeaponId == "weapon.laser-fence"
                || weapon?.WeaponId == "weapon.repair-jammer"
                || weapon?.WeaponId == "weapon.virus"
                || weapon?.WeaponId == "weapon.sky-hook"
                || weapon?.WeaponId == "weapon.spider-mine"
                || weapon?.WeaponId == "weapon.fpv-drone"
                || weapon?.WeaponId == "weapon.bomber"
                || weapon?.WeaponId == "weapon.ufo-carrier";
        }

        private static bool ReturnsHeldWeaponAfterFlight(WeaponDefinition weapon)
        {
            // A spent instance remains hidden, but an arsenal slot with ammo
            // left immediately shows its next physical item after landing.
            return HidesHeldWeaponDuringFlight(weapon);
        }

        void IBattleCommandTarget.SelectWeapon(int weaponIndex)
        {
            SelectWeapon(weaponIndex);
        }

        void IBattleCommandTarget.Fire(Vector2 origin, Vector2 direction, float power)
        {
            FireWithAim(new AimState(origin, direction, power, true));
        }

        private void RefreshPlayerWeaponVisual()
        {
            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null)
            {
                return;
            }

            if (!_playerWeaponSelected || _inventory?.Selected == null || _hideHeldWeaponForReusableThrow
                || BattleSimulation.IsMarkedStrikeWeapon(_inventory.Selected)
                || IsPlayerTurnShotSpent())
            {
                presenter.ClearEquippedWeapon();
                return;
            }

            var weapon = _inventory.Selected;
            if (weapon.WeaponId == "weapon.titanium-barrier")
            {
                presenter.ClearEquippedWeapon();
                return;
            }
            var presentation = ResolveHeldWeaponPresentation(weapon.WeaponId);
            presenter.SetWeaponPresentation(
                presentation.Scale,
                presentation.Position,
                presentation.AimAngleOffset);
            var heldIconId = ResolveHudIconId(weapon);
            if (weapon.WeaponId == "weapon.electric-harpoon" && _simulation != null
                && _simulation.HasPlayerElectricHarpoon)
            {
                heldIconId = heldIconId.EndsWith("_5A", System.StringComparison.Ordinal)
                    ? $"{heldIconId}_shot"
                    : "electric_harpoon_shot";
            }
            presenter.SetEquippedWeapon(
                LoadWeaponIconSprite(heldIconId),
                WeaponIconFacesRight(weapon.IconId));
            var direction = _aimState.HasInput
                ? _aimState.Direction
                : RestingWeaponDirection(weapon.WeaponId, presenter.State.FacingDirectionX);
            presenter.SetWeaponAim(direction);
        }

        /// <summary>
        /// Barrel direction while no aim is committed: heavy tube weapons rest
        /// slightly tilted toward the ground; everything else sits horizontal.
        /// </summary>
        private static Vector2 RestingWeaponDirection(string weaponId, int facingDirectionX)
        {
            var facing = facingDirectionX >= 0 ? 1f : -1f;
            if (!HasRestingTilt(weaponId))
            {
                return Vector2.right * facing;
            }

            var radians = RestingWeaponTiltDegrees * Mathf.Deg2Rad;
            return new Vector2(facing * Mathf.Cos(radians), -Mathf.Sin(radians));
        }

        private static bool HasRestingTilt(string weaponId)
        {
            return weaponId == "weapon.bur"
                || weaponId == "weapon.mortira"
                || weaponId == "weapon.toxic"
                || weaponId == "weapon.freeze";
        }

        private static (Vector2 Scale, Vector2 Position, float AimAngleOffset) ResolveHeldWeaponPresentation(string weaponId)
        {
            return weaponId switch
            {
                "weapon.impulse" => (Vector2.one, new Vector2(-0.48f, 0.27f), 0f),
                "weapon.blaster-caset" => (Vector2.one, new Vector2(-0.56f, 0.34f), 0f),
                // Positive offsets counter the art's built-in downward barrel
                // tilt so the barrel axis matches the aim/trajectory exactly.
                "weapon.bur" => (Vector2.one * 1.5f, new Vector2(-0.54f, 0.42f), 15f),
                "weapon.mortira" => (Vector2.one * 1.12f, new Vector2(-0.55f, 0.40f), 7f),
                "weapon.granata" => (Vector2.one * 0.76f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.sticky-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.hopping-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.cryo-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.incendiary-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.teleport" => (Vector2.one * 0.82f, new Vector2(-0.60f, 0.40f), 0f),
                "weapon.mine" => (Vector2.one * 0.82f, new Vector2(-0.58f, 0.36f), 0f),
                "weapon.toxic" => (Vector2.one, new Vector2(-0.68f, 0.42f), 11f),
                "weapon.emp" => (Vector2.one, new Vector2(-0.55f, 0.42f), 0f),
                "weapon.freeze" => (Vector2.one, new Vector2(-0.55f, 0.42f), 10f),
                "weapon.fire-gun" => (Vector2.one * 1.5f, new Vector2(-0.68f, 0.42f), 0f),
                "weapon.missle" => (Vector2.one * 1.5f, new Vector2(-0.54f, 0.30f), 0f),
                // Raised and moved forward so the hand grips the hilt rather than the middle of the blade.
                "weapon.plasma-sword" => (Vector2.one * 2f, new Vector2(-0.36f, 0.68f), 0f),
                "weapon.rail-rifle" => (new Vector2(1f, 1.5f), new Vector2(-0.62f, 0.39f), 0f),
                "weapon.heal" => (Vector2.one, new Vector2(-0.54f, 0.38f), 180f),
                "weapon.fpv-drone" => (Vector2.one, new Vector2(-0.28f, 1.08f), 0f),
                _ => (Vector2.one, new Vector2(-0.68f, 0.42f), 0f)
            };
        }

        private static bool WeaponIconFacesRight(string iconId)
        {
            // The drill ("bur") art has its bit on the right - mirroring it made
            // the barrel point backwards against the aim line.
            return iconId != "bazooka_simple"
                && iconId != "bazooka"
                && iconId != "drobovik";
        }
    }
}
