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
        private void BuildHud()
        {
            _hud = BattleHudFactoryBridge.Create(transform);
            if (_hud == null)
            {
                Debug.LogWarning("BattleHudFactoryBridge.Factory not registered; HUD will be disabled.");
                return;
            }

            _hud.FireRequested += Fire;
            _hud.WeaponSelected += OnHudWeaponSelected;
            _hud.ArsenalVisibilityChanged += OnHudArsenalVisibilityChanged;
            _hud.WeaponResetRequested += ResetSelectedWeapon;
            _hud.SettingsRequested += OpenBattleSettings;
            _hud.MoveLeftStarted += () => SetUiHorizontalInput(-1);
            _hud.MoveLeftStopped += () => StopUiHorizontalInput(-1);
            _hud.MoveRightStarted += () => SetUiHorizontalInput(1);
            _hud.MoveRightStopped += () => StopUiHorizontalInput(1);
            _hud.JumpLeftRequested += () => TryStartJump(-1);
            _hud.JumpRightRequested += () => TryStartJump(1);
            _hud.RopeUpStarted += () => _ropeVerticalInput = -1;
            _hud.RopeUpStopped += () => _ropeVerticalInput = 0;
            _hud.RopeDownStarted += () => _ropeVerticalInput = 1;
            _hud.RopeDownStopped += () => _ropeVerticalInput = 0;
            _hud.RestartRequested += RestartScene;
            _hud.ContinueRequested += ContinueFromResult;
            _hud.MenuRequested += ExitToMenu;
            _hud.TutorialSkipRequested += SkipFirstBattleTutorial;
        }

        private void SkipFirstBattleTutorial()
        {
            if (_firstBattleGuide == null || !_firstBattleGuide.IsActive)
            {
                return;
            }

            ClearWalkInput();
            _firstBattleGuide.Skip();
            BattleTutorialBridge.SkipActiveMissionTutorial();
            GameSessionContext.ReportBattleFacts(playerWon: true, healthPercent: 100, opponentRingOut: false);
            GameSessionContext.ReportMissionCompleted();
            GameSessionContext.RequestCampaignMissionView(GameSessionContext.SelectedMissionId);
            LoadMenuScene();
        }

        private void OpenBattleSettings()
        {
            if (_battleSettingsOpen || !BattleSettingsBridge.IsConfigured)
            {
                return;
            }

            _battleSettingsOpen = true;
            _timeScaleBeforeSettings = Time.timeScale;
            Time.timeScale = 0f;
            BattleSettingsBridge.Open(
                RestoreBattleTimeScale,
                () =>
                {
                    RestoreBattleTimeScale();
                    RestartScene();
                },
                () =>
                {
                    RestoreBattleTimeScale();
                    ExitToMenu();
                });
        }

        private void RestoreBattleTimeScale()
        {
            if (!_battleSettingsOpen)
            {
                return;
            }

            Time.timeScale = _timeScaleBeforeSettings;
            _battleSettingsOpen = false;
        }

        private void ReportTutorial(string triggerKey)
        {
            if (GameSessionContext.IsTrainingSkirmish)
            {
                return;
            }

            // The first training battle owns its full lesson through
            // FirstBattleGuideState. Generic short-lived cues would otherwise
            // reopen the instruction card after its final required shot.
            if (FirstBattleGuideState.IsFirstMission(GameSessionContext.SelectedMissionId))
            {
                return;
            }

            var cue = BattleTutorialBridge.Report(triggerKey);
            if (!cue.IsEmpty)
            {
                _coachMessage = cue.Message;
                _coachTargetId = cue.UiTargetId;
                _coachTimer = 4.5f;
            }
        }

        private static void EmitFeedback(BattleFeedbackEventType type)
        {
            var key = type switch
            {
                BattleFeedbackEventType.AimStarted => BattleFeedbackBridge.Aim,
                BattleFeedbackEventType.ShotFired => BattleFeedbackBridge.Shot,
                BattleFeedbackEventType.ProjectileImpact => BattleFeedbackBridge.Impact,
                BattleFeedbackEventType.DamageResolved => BattleFeedbackBridge.Damage,
                BattleFeedbackEventType.UnitDefeated => BattleFeedbackBridge.Defeat,
                BattleFeedbackEventType.BattleFinished => BattleFeedbackBridge.Finished,
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(key))
            {
                BattleFeedbackBridge.Report(key);
            }
        }

        private void RestartScene()
        {
            // Go through the loading bridge like every other transition, so a retry shows
            // the loading screen instead of freezing on the result card.
            if (!SceneLoadingBridge.TryLoad(GameSceneNames.Battle))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void ContinueFromResult()
        {
            ExitToMenu();
        }

        private void ExitToMenu()
        {
            if (IsBattleReady() && _simulation.Result.IsFinished)
            {
                PrepareResultReturn();
            }

            LoadMenuScene();
        }

        private void PrepareResultReturn()
        {
            var mode = GameSessionContext.SelectedBattleMode;
            if (mode == BattleMode.Story
                && _simulation.Result.Outcome == BattleOutcome.PlayerWon)
            {
                GameSessionContext.ReportMissionCompleted();
                GameSessionContext.RequestCampaignMissionView(GameSessionContext.SelectedMissionId);
                return;
            }

            // Repeatable modes return to their own tab; training has no dedicated
            // tab and therefore returns to the map section.
            GameSessionContext.RequestBattleModeView(mode);
        }

        private static void LoadMenuScene()
        {
            if (!SceneLoadingBridge.TryLoad(GameSceneNames.Menu))
            {
                SceneManager.LoadScene(GameSceneNames.Menu);
            }
        }

        private void RenderHud()
        {
            UpdateTurnTransferMarker();
            if (_hud == null || !IsBattleReady())
            {
                return;
            }

            var weapon = _inventory.Selected;
            var playerHasTime = _simulation.MovementTimeRemaining > 0f;
            var ropeAttached = _simulation.IsPlayerRopeAttached || _simulation.HasPlayerBoost;
            var directDroneControlActive = _simulation.HasActiveControlledProjectile;
            var fireInputReady = ropeAttached || !BattleSimulation.RequiresAim(weapon) || _aimState.HasInput;
            var bomberActive = _simulation.HasPlayerCopter;
            var canContinueMinePlacement = CanContinueMinePlacementInput();
            var canFire = !_simulation.Result.IsFinished
                && (ropeAttached || bomberActive || (!HasActiveMotion && !HasActiveWeaponEffect
                    && (!HasProjectilesInFlight || canContinueMinePlacement)))
                && !HasPendingGroundAction
                && playerHasTime
                && _playerWeaponSelected
                && (_inventory.CanFireSelected || _simulation.CanContinueHoppingGrenade()
                    || _simulation.CanTriggerTeleportEarly() || _simulation.HasPlayerBoost
                    || _simulation.HasPlayerCopter)
                && fireInputReady;
            var guideActive = _firstBattleGuide != null && _firstBattleGuide.IsActive;
            var isFirstTrainingBattle = FirstBattleGuideState.IsFirstMission(GameSessionContext.SelectedMissionId);
            // Instruction cards belong to the first training battle only. The old
            // condition inverted that and showed them in every later mission instead.
            var showTutorialCard = !GameSessionContext.IsTrainingSkirmish
                && isFirstTrainingBattle
                && (guideActive || _coachTimer > 0f);
            var status = guideActive
                ? _firstBattleGuide.Message
                : _coachTimer > 0f && !string.IsNullOrEmpty(_coachMessage) ? _coachMessage : _status;
            var tutorialTargetId = guideActive
                ? _firstBattleGuide.TargetId
                : _coachTimer > 0f ? _coachTargetId : string.Empty;
            var canUseMovement = IsPlayerControlTurn(_battleState.Turn.ActiveTeam)
                && _simulation.CanAcceptMovementInput()
                && (!guideActive || _firstBattleGuide.AllowsWalkDirection(-1) || _firstBattleGuide.AllowsWalkDirection(1)
                    || _firstBattleGuide.AllowsJumpDirection(-1) || _firstBattleGuide.AllowsJumpDirection(1));
            var mobilityLabel = GameText.MobilityLabel(_simulation.MovementTimeRemaining);
            var opponentWeapon = _opponentInventory?.Selected;
            var canOpenArsenal = !_simulation.Result.IsFinished
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && !HasActiveWeaponEffect
                && !HasPendingGroundAction
                && !_simulation.IsAttackCommitted
                && playerHasTime
                && (!guideActive || _firstBattleGuide.AllowsArsenal);

            var state = new BattleHudState(
                status,
                _battleState.Turn.TurnNumber,
                DescribeTeam(_battleState.Turn.ActiveTeam),
                _battleState.Turn.ActiveTeam == BattleTeamId.Player ? _simulation.MovementTimeRemaining : 0f,
                !guideActive || _firstBattleGuide.ShowTurnTimer,
                GetTeamHealth(BattleTeamId.Player),
                GetTeamMaxHealth(BattleTeamId.Player),
                GetTeamHealth(BattleTeamId.Opponent),
                GetTeamMaxHealth(BattleTeamId.Opponent),
                GameText.WeaponSlot(DescribeWeapon(weapon), _inventory.SelectedIndex + 1, _inventory.Count),
                weapon != null ? ResolveHudIconId(weapon) : string.Empty,
                WeaponPresentationText.DescribeRole(weapon),
                DescribeAmmo(_inventory),
                WeaponPresentationText.DescribeBattleHint(weapon),
                mobilityLabel,
                _arsenalHudItems,
                _playerWeaponSelected ? _inventory.SelectedIndex : -1,
                _playerWeaponSelected,
                _playerWeaponSelected && fireInputReady,
                canOpenArsenal,
                tutorialTargetId,
                showTutorialCard,
                guideActive && _firstBattleGuide.BlocksWeaponSwitch,
                guideActive && _firstBattleGuide.BlocksCancelAim,
                guideActive && !(_firstBattleGuide.AllowsWalkDirection(-1) || _firstBattleGuide.AllowsWalkDirection(1)),
                guideActive && isFirstTrainingBattle,
                _aimState.Power,
                _aimSettings.MinPower,
                _aimSettings.MaxPower,
                canFire && (!guideActive || _firstBattleGuide.AllowsFire),
                canUseMovement,
                _simulation.Result.IsFinished,
                _resultScreenState.Title,
                _resultScreenState.Subtitle,
                _simulation.Result.IsFinished ? DescribeOutcome(_resultScreenState.Result.Outcome) : string.Empty,
                _simulation.Result.IsFinished ? DescribeResultReward(_resultScreenState.RewardPreview) : string.Empty,
                _simulation.Result.IsFinished && _resultScreenState.RewardPreview.HasUnlock ? _resultScreenState.RewardPreview.UnlockText : GameText.NoUnlocks,
                _simulation.Result.IsFinished && _resultScreenState.CanContinue,
                _simulation.Result.IsFinished ? _resultScreenState.RetryLabel : GameText.Restart,
                _resultScreenState.CanContinue ? _resultScreenState.ContinueLabel : GameText.Menu,
                _simulation.Result.Outcome == BattleOutcome.PlayerWon,
                GameSessionContext.SelectedMissionText,
                // Only a won battle banks the mission - a loss leaves it outstanding even
                // when its objective was met, matching what the campaign map will show.
                _simulation.Result.Outcome == BattleOutcome.PlayerWon,
                ResolveOpponentName(),
                DescribeWeapon(opponentWeapon),
                opponentWeapon != null ? _opponentProgression.ResolveRank(opponentWeapon.WeaponId) : 0,
                ropeAttached || directDroneControlActive || IsTitaniumBarrierPlacementActive(),
                ResolveControlledBombsForHud(),
                ResolveControlledSecondsForHud());

            _hud.Render(state);
        }

        /// <summary>Bomb counter of the player's own copter; hidden (-1) otherwise.</summary>
        private int ResolveControlledBombsForHud()
        {
            return _simulation != null && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && _simulation.HasPlayerBomberCopter
                    ? _simulation.BomberBombsRemaining
                    : -1;
        }

        /// <summary>Flight seconds of the player's copter or rocket fuel; hidden (-1) otherwise.</summary>
        private float ResolveControlledSecondsForHud()
        {
            if (_simulation == null || _battleState.Turn.ActiveTeam != BattleTeamId.Player)
            {
                return -1f;
            }

            if (_simulation.HasPlayerCopter)
            {
                return _simulation.CopterSecondsRemaining;
            }

            return _simulation.HasPlayerBoost ? _simulation.BoostSecondsRemaining : -1f;
        }

        private void RefreshArsenalHudItems()
        {
            var items = new BattleWeaponHudItem[_inventory.Count];
            for (var index = 0; index < _inventory.Count; index++)
            {
                var weapon = _inventory.Weapons[index];
                var remainingAmmo = _inventory.GetRemainingAmmo(weapon);
                var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
                var hasProgression = WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression);
                var description = hasProgression
                    ? WeaponUpgradePresentation.GetCurrentDescription(weapon, progression, progress)
                    : WeaponPresentationText.Describe(weapon);
                var damageValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Core)
                    : weapon.WeaponId == "weapon.grounder" ? 2f : weapon.Damage;
                var accuracyValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Aim)
                    : 70f;
                var ammoValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Loadout)
                    : (weapon.HasLimitedAmmo ? weapon.AmmoCount : -1f);
                var radiusValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Radius)
                    : weapon.ImpactRadius;
                var displayName = hasProgression && progress != null
                    ? WeaponUpgradePresentation.GetWeaponName(
                        progression,
                        progress.rank,
                        (WeaponUpgradeBranch)progress.branch,
                        DescribeWeapon(weapon))
                    : DescribeWeapon(weapon);
                var ammoLabel = weapon.HasLimitedAmmo
                    ? GameText.AmmoLimited(remainingAmmo)
                    : GameText.AmmoUnlimited();
                items[index] = new BattleWeaponHudItem(
                    index,
                    displayName,
                    ResolveHudIconId(weapon),
                    ammoLabel,
                    weapon.HasLimitedAmmo,
                    remainingAmmo,
                    _simulation != null && _simulation.CanUsePlayerWeapon(weapon),
                    progress?.rank ?? 1,
                    description,
                    damageValue,
                    accuracyValue,
                    ammoValue,
                    radiusValue,
                    weapon.WeaponId == "weapon.grounder"
                        ? "architect.detail.statDuration"
                        : "architect.detail.statDamage");
            }

            _arsenalHudItems = items;
        }

        /// <summary>
        /// Same rank/branch-aware icon as the workshop tree, arsenal shelf and shop
        /// listing - the HUD selector shows the evolved art once the weapon reaches
        /// rank 5/10, not just the base icon.
        /// </summary>
        private static string ResolveHudIconId(WeaponDefinition weapon)
        {
            var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
            if (progress == null)
            {
                return weapon.IconId;
            }

            var resource = WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                weapon.IconId, progress.rank, (WeaponUpgradeBranch)progress.branch);
            const string prefix = "UI/Weapons/";
            return resource.StartsWith(prefix, System.StringComparison.Ordinal)
                ? resource.Substring(prefix.Length)
                : weapon.IconId;
        }

        private bool IsWeaponSwitchBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive && _firstBattleGuide.BlocksWeaponSwitch;
        }

        private bool IsWeaponSwitchBlockedByGuideBeforeWeaponSelected()
        {
            return IsWeaponSwitchBlockedByGuide();
        }

        private bool IsCancelBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive && _firstBattleGuide.BlocksCancelAim;
        }

        private static string DescribeAmmo(WeaponInventoryRuntime inventory)
        {
            var weapon = inventory.Selected;
            return weapon != null && weapon.HasLimitedAmmo
                ? GameText.AmmoLimited(inventory.GetSelectedRemainingAmmo())
                : GameText.AmmoUnlimited();
        }

        private static string DescribeOutcome(BattleOutcome outcome)
        {
            return outcome switch
            {
                BattleOutcome.PlayerWon => GameText.PlayerWon(),
                BattleOutcome.OpponentWon => GameText.OpponentWon(),
                BattleOutcome.Draw => GameText.Draw(),
                _ => string.Empty
            };
        }

        private static string DescribeTeam(BattleTeamId teamId)
        {
            return teamId switch
            {
                BattleTeamId.Player => GameText.PlayerTeam,
                BattleTeamId.Opponent => GameText.OpponentTeam,
                _ => GameText.NoTeam
            };
        }

        private static string DescribeArena(ArenaDefinition arena)
        {
            return arena == null
                ? GameText.ArenaFallback()
                : GameText.ArenaName(arena.ArenaId, arena.DisplayName);
        }

        private static string DescribeWeapon(WeaponDefinition weapon)
        {
            return weapon == null ? GameText.UnknownWeapon : WeaponDisplayNames.Get(weapon.WeaponId);
        }

        private static string DescribeWeaponId(string weaponId)
        {
            return string.IsNullOrWhiteSpace(weaponId) ? GameText.UnknownWeapon : WeaponDisplayNames.Get(weaponId);
        }

        private int GetTeamHealth(BattleTeamId teamId)
        {
            return _battleState.GetTeam(teamId).Units.Sum(unit => unit.Health);
        }

        private int GetTeamMaxHealth(BattleTeamId teamId)
        {
            return _battleState.GetTeam(teamId).Units.Sum(unit => unit.MaxHealth);
        }
    }
}
