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
        /// <summary>
        /// Drill rockets don't get a fully baked "in flight" sprite per rank/branch
        /// body variant - instead the shared exhaust trail art is overlaid as a
        /// second layer behind the body sprite. The homing missile uses its own
        /// baked missle_fly sprite and must not receive this overlay.
        /// </summary>
        /// <summary>The shared exhaust is attached under the nose-up signal rocket body.</summary>
        private void SpawnSkyHookTail(Transform visualTransform, SpriteRenderer bodyRenderer)
        {
            var trailSprite = LoadCenteredTextureSprite(RocketTrailResourcePath, "Rocket Exhaust Trail");
            if (trailSprite == null || bodyRenderer.sprite == null)
            {
                return;
            }

            var trailObject = new GameObject("Rocket Trail");
            trailObject.transform.SetParent(visualTransform, false);
            // The exhaust art trails to the left; a quarter turn makes it trail downwards.
            trailObject.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var baseY = -bodyRenderer.sprite.bounds.extents.y * 0.92f;
            var trailLength = trailSprite.bounds.size.x;
            trailObject.transform.localPosition = new Vector3(0f, baseY - trailLength * 0.35f, 0f);
            var trailRenderer = trailObject.AddComponent<SpriteRenderer>();
            trailRenderer.sprite = trailSprite;
            trailRenderer.sortingOrder = 29;
            // The body is scaled down about 20x; the exhaust must follow it.
            trailObject.transform.localScale = Vector3.one * 1.0f;
        }

        private void SpawnRocketTrail(Transform visualTransform, SpriteRenderer bodyRenderer, bool isDrillRocket)
        {
            var trailSprite = LoadCenteredTextureSprite(RocketTrailResourcePath, "Rocket Exhaust Trail");
            if (trailSprite == null || bodyRenderer.sprite == null)
            {
                return;
            }

            var trailObject = new GameObject("Rocket Trail");
            trailObject.transform.SetParent(visualTransform, false);

            // Body sprites face +X (nose right); the engine sits inset from the
            // sprite's left edge rather than exactly on it. Best-effort placement -
            // nudge RocketTrailAttachFraction if the flame doesn't line up with the
            // engine once seen in the editor.
            var bodyHalfWidth = bodyRenderer.sprite.bounds.extents.x;
            var trailHalfWidth = trailSprite.bounds.extents.x;
            var attachX = -bodyHalfWidth * RocketTrailAttachFraction;
            trailObject.transform.localPosition = new Vector3(attachX - trailHalfWidth * 0.7f, 0f, 0f);

            var trailRenderer = trailObject.AddComponent<SpriteRenderer>();
            trailRenderer.sprite = trailSprite;
            // The drill rocket has a larger body and a heavier exhaust plume;
            // extend only its shared tail so the ordinary missile keeps its
            // established silhouette.
            trailObject.transform.localScale = isDrillRocket ? Vector3.one * 1.35f : Vector3.one;
            trailRenderer.sortingOrder = 29;
        }

        private void ApplyProjectileVisualTransform(Transform projectileTransform, SimProjectile projectile)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var diameter = projectile.Definition.Radius * 2f;
            if (weapon == null)
            {
                projectileTransform.localScale = Vector3.one * diameter;
                return;
            }

            if (projectile.Definition.ProjectileId == GuardianDeployProjectileId)
            {
                projectileTransform.localScale = Vector3.one * GuardianPlacedVisualScale;
                return;
            }

            if (IsCopterBody(projectile.Definition.ProjectileId))
            {
                // The copter art is about 10 units across at 100 px/unit; it faces right and never rotates.
                projectileTransform.localScale = Vector3.one * 0.085f;
                projectileTransform.rotation = Quaternion.identity;
                return;
            }

            if (projectile.Definition.ProjectileId == "projectile.weapon.bomber.bomb")
            {
                // An enlarged blaster bolt standing vertically along the fall (about 0.35 long and 0.11 thick).
                projectileTransform.localScale = Vector3.one * 0.045f;
                projectileTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
                return;
            }

            projectileTransform.localScale = ResolveProjectileVisualScale(weapon, diameter);
            if (weapon.WeaponId != "weapon.fpv-drone")
            {
                projectileTransform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg
                    + ResolveProjectileArtRotationOffset(weapon));
            }
        }

        /// <summary>The bomber and the carrier copters (and the carrier falling after its window) share one upright body look.</summary>
        private static bool IsCopterBody(string projectileId)
        {
            return projectileId == "projectile.weapon.bomber"
                || projectileId == "projectile.weapon.ufo-carrier"
                || projectileId == "projectile.weapon.ufo-carrier.fall";
        }

        /// <summary>Most flight skins face +X; the signal rocket art points up.</summary>
        private static float ResolveProjectileArtRotationOffset(WeaponDefinition weapon)
        {
            // The signal rocket points up; the nuclear bomb's nose points towards the lower left (225 degrees).
            return weapon?.WeaponId switch
            {
                "weapon.sky-hook" => -90f,
                "weapon.nuclear-charge" => 135f,
                _ => 0f
            };
        }

        private static Vector3 ResolveProjectileVisualScale(WeaponDefinition weapon, float diameter)
        {
            diameter *= 0.5f;
            return weapon.WeaponId switch
            {
                "weapon.blaster-caset" => Vector3.one * (diameter * 0.42f),
                // Plain black pellets; at the former 0.02 they could not be seen (owner decision 2026-10-09).
                "weapon.ion-shotgun" => Vector3.one * 0.08f,
                // blaster_shot is 1254 px wide (opaque bolt 781x241 px): this scale gives a bolt about 0.035 world units thick, close to a shotgun pellet.
                "weapon.rail-rifle" => Vector3.one * 0.0145f,
                "weapon.impulse" => Vector3.one * (diameter * 0.40572f),
                "weapon.freeze" => Vector3.one * (diameter * 0.315f),
                "weapon.mortira" => Vector3.one * (diameter * 0.36f),
                "weapon.guardian-drone" => Vector3.one * (diameter * 0.24f),
                // sky_hook is 1024 x 1536 px (15.4 world units tall at 100 px/unit); this makes the rocket about 0.75 tall.
                "weapon.sky-hook" => Vector3.one * 0.049f,
                "weapon.granata" => Vector3.one * (diameter * 0.35f),
                // The new grenade art has more empty canvas than the original
                // grenade. Its flight scale compensates for that so it matches
                // the intentionally smaller held presentation below.
                "weapon.sticky-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.hopping-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.cryo-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.incendiary-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.titanium-disc" => Vector3.one * (diameter * 0.85f),
                "weapon.laser-fence" => Vector3.one * (diameter * 0.35f),
                "weapon.teleport" => Vector3.one * (diameter * 0.72f),
                "weapon.swap-jump" => Vector3.one * (diameter * 0.72f),
                "weapon.toxic" => Vector3.one * (diameter * 1.05f),
                "weapon.emp" => new Vector3(diameter * 0.46f, diameter * 0.33f, 1f),
                "weapon.mine" => Vector3.one * (diameter * 0.55f),
                "weapon.bur-rocket" => Vector3.one * (diameter * 0.68f),
                "weapon.missle" => Vector3.one * (diameter * 1.25f),
                "weapon.orbital-rocket" => Vector3.one * (diameter * 1.1f),
                // nuclear_charge is 1254 px square (12.5 units at 100 px/unit); the bomb is drawn about 0.8 long.
                "weapon.nuclear-charge" => Vector3.one * 0.06f,
                "weapon.fpv-drone" => Vector3.one * (diameter * 1.4f),
                _ => Vector3.one * (diameter * 1.15f)
            };
        }

        private Color ResolveProjectileVisualColor(WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return Color.black;
            }

            return weapon.WeaponId == "weapon.ion-shotgun"
                ? Color.black
                : ProjectileWeaponEffect.GetProjectileColor(weapon.Role);
        }

        private Sprite ResolveProjectileSprite(SimProjectile projectile, WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return null;
            }

            if (projectile?.Definition != null && projectile.Definition.ProjectileId == "projectile.weapon.bomber.bomb")
            {
                return LoadWeaponIconSprite("blaster_shot");
            }

            // The thrown guardian body flies with the same skin it shows on the ground; only its bullet is small.
            if (projectile?.Definition != null && projectile.Definition.ProjectileId == GuardianDeployProjectileId)
            {
                return LoadWeaponIconSprite(ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId));
            }

            var spriteId = weapon.WeaponId switch
            {
                // These are explicit projectile sprites. Never substitute the held
                // weapon/evolution icon here: it makes the gun itself look as though
                // it was fired from the character.
                "weapon.blaster-caset" => "blaster_shot",
                "weapon.rail-rifle" => "blaster_shot",
                "weapon.impulse" => "impulse_shot",
                "weapon.mortira" => "mortira_shot",
                "weapon.guardian-drone" => "mortira_shot",
                "weapon.emp" => "emp_shot",
                "weapon.freeze" => "ice_shot",
                "weapon.toxic" => "toxic_shot",
                "weapon.missle" => MissileInFlightSpriteId,
                "weapon.orbital-rocket" => MissileInFlightSpriteId,
                "weapon.nuclear-charge" => "nuclear_charge",
                "weapon.bur-rocket" => "bur_rocket",
                "weapon.bur" => "bur_lazer",
                "weapon.fire-gun" => "fire_shot_1",
                // The following launch their actual placed/thrown object. Their
                // rank-aware art is therefore the correct in-flight representation.
                "weapon.granata" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.sticky-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.hopping-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.cryo-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.incendiary-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.titanium-disc" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.laser-fence" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.mine" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.c4" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.heal" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                 "weapon.teleport" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                 "weapon.swap-jump" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.bonus-box" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.fpv-drone" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.repair-jammer" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.virus" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.sky-hook" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.spider-mine" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.bomber" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.ufo-carrier" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                _ => null
            };
            return LoadWeaponIconSprite(spriteId);
        }

        /// <summary>
        /// Rank/branch-aware icon for a weapon's in-flight/held visual, resolved from
        /// the progression of whichever side fired it.
        /// </summary>
        private string ResolveOwnerAwareEvolvedIconId(WeaponDefinition weapon, string ownerUnitId)
        {
            var isPlayerOwned = !string.IsNullOrWhiteSpace(ownerUnitId)
                && ownerUnitId.StartsWith("player.", System.StringComparison.Ordinal);
            var evolved = ResolveEvolvedIconId(
                weapon?.IconId,
                weapon,
                isPlayerOwned ? _playerProgression : _opponentProgression);
            if (!string.Equals(evolved, weapon?.IconId, System.StringComparison.Ordinal))
            {
                return evolved;
            }

            return weapon.IconId;
        }

        private WeaponDefinition ResolveWeaponFromProjectileId(string projectileId)
        {
            if (projectileId == "projectile.weapon.mine.deploy")
            {
                return _weaponsById.TryGetValue("weapon.mine", out var mine) ? mine : null;
            }

            if (projectileId == "projectile.weapon.laser-fence.deploy")
            {
                return _weaponsById.TryGetValue("weapon.laser-fence", out var laserFence) ? laserFence : null;
            }

            if (projectileId == "projectile.weapon.bomber.bomb")
            {
                return _weaponsById.TryGetValue("weapon.bomber", out var bomber) ? bomber : null;
            }

            if (projectileId == "projectile.weapon.ufo-carrier.fall")
            {
                return _weaponsById.TryGetValue("weapon.ufo-carrier", out var carrier) ? carrier : null;
            }

            // The thrown guardian body; without this it had no weapon and flew as a plain black ball.
            if (projectileId == GuardianDeployProjectileId)
            {
                return _weaponsById.TryGetValue("weapon.guardian-drone", out var guardian) ? guardian : null;
            }

            if (projectileId != null
                && (projectileId.StartsWith("projectile.weapon.mortira.fragment", System.StringComparison.Ordinal)
                    || projectileId.StartsWith("projectile.weapon.mortira.cascade.", System.StringComparison.Ordinal)))
            {
                return _weaponsById.TryGetValue("weapon.mortira", out var mortar) ? mortar : null;
            }

            if (projectileId != null
                && projectileId.StartsWith("projectile.weapon.missle.cluster.", System.StringComparison.Ordinal))
            {
                return _weaponsById.TryGetValue("weapon.missle", out var missile) ? missile : null;
            }

            if (!string.IsNullOrWhiteSpace(projectileId)
                && projectileId.StartsWith("projectile.", System.StringComparison.Ordinal))
            {
                var weaponId = projectileId.Substring("projectile.".Length);
                if (_weaponsById.TryGetValue(weaponId, out var weapon))
                {
                    return weapon;
                }
            }

            return null;
        }

        private void SyncProjectileVisuals()
        {
            if (_simulation == null)
            {
                return;
            }

            foreach (var projectile in _simulation.Projectiles)
            {
                if (_projectileVisuals.TryGetValue(projectile.Id, out var projectileObject) && projectileObject != null)
                {
                    projectileObject.transform.position = projectile.Position;
                    if (IsCopterBody(projectile.Definition.ProjectileId)
                        && projectileObject.transform.childCount > 0)
                    {
                        // The copter art faces right: it mirrors with the direction of travel.
                        var body = projectileObject.transform.GetChild(0);
                        var facing = projectile.Velocity.x < -0.05f ? -1f : projectile.Velocity.x > 0.05f ? 1f : Mathf.Sign(body.localScale.x);
                        body.localScale = new Vector3(Mathf.Abs(body.localScale.x) * facing, body.localScale.y, body.localScale.z);
                    }
                    else if (projectile.Definition.ProjectileId == "projectile.weapon.bomber.bomb"
                        || projectile.Definition.ProjectileId == GuardianDeployProjectileId)
                    {
                        // Keeps its pose (the bomb vertical, the thrown guardian upright); nothing to rotate.
                    }
                    else if (projectileObject.transform.childCount > 0
                        && projectile.Velocity.sqrMagnitude > 0.01f
                        && ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId)?.WeaponId is not
                            ("weapon.fpv-drone" or "weapon.titanium-disc"))
                    {
                        var artOffset = ResolveProjectileArtRotationOffset(
                            ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId));
                        projectileObject.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f,
                            Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg + artOffset);
                    }
                }
            }

            var activeIds = new HashSet<int>(_simulation.Projectiles.Select(projectile => projectile.Id));
            foreach (var projectileId in _projectileVisuals.Keys.Where(id => !activeIds.Contains(id)).ToArray())
            {
                var visual = _projectileVisuals[projectileId];
                _projectileVisuals.Remove(projectileId);
                _arenaObjects.Remove(visual);
                Destroy(visual);
            }
        }
    }
}
