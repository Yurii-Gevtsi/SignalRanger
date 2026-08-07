using System.Collections.Generic;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class ImpulseSalvoWeaponEffectTests
    {
        private sealed class RecordingDelayedSpawner : IProjectileSpawner, IDelayedProjectileSpawner
        {
            public readonly List<ProjectileSpawnRequest> ImmediateRequests = new();
            public readonly List<(ProjectileSpawnRequest Request, float Delay)> ScheduledRequests = new();

            public void Spawn(in ProjectileSpawnRequest request)
            {
                ImmediateRequests.Add(request);
            }

            public void Schedule(in ProjectileSpawnRequest request, float delaySeconds)
            {
                ScheduledRequests.Add((request, delaySeconds));
            }
        }

        [TestCase(2, 1)]
        [TestCase(3, 2)]
        public void Fire_SpawnsFirstImpulseImmediatelyAndSchedulesTheRest(int salvoCount, int scheduledCount)
        {
            var weapon = CreateImpulseWeapon();
            var spawner = new RecordingDelayedSpawner();
            var context = new WeaponFireContext(
                weapon,
                "unit.player.01",
                Vector2.zero,
                Vector2.right,
                12f,
                spawner,
                new WeaponEvolutionProfile(salvoCount, 0.18f, 1f));

            new ImpulseSalvoWeaponEffect().Fire(context);

            Assert.That(spawner.ImmediateRequests, Has.Count.EqualTo(1));
            Assert.That(spawner.ScheduledRequests, Has.Count.EqualTo(scheduledCount));
            Assert.That(spawner.ImmediateRequests[0].Definition.Damage, Is.EqualTo(weapon.Damage));
            Assert.That(spawner.ScheduledRequests[0].Delay, Is.EqualTo(0.18f).Within(0.0001f));
        }

        [Test]
        public void Resolver_MapsImpulseBranchBToConfiguredKnockbackStrength()
        {
            var weapon = CreateImpulseWeapon();
            var rankFive = new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            };
            var rankTen = new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            };

            var impact = WeaponEvolutionRuntimeResolver.Resolve(weapon, rankFive);
            var assault = WeaponEvolutionRuntimeResolver.Resolve(weapon, rankTen);

            Assert.That(impact.SalvoCount, Is.EqualTo(1));
            Assert.That(impact.KnockbackMultiplier, Is.EqualTo(1.55f).Within(0.0001f));
            Assert.That(assault.KnockbackMultiplier, Is.EqualTo(2.15f).Within(0.0001f));
        }

        [Test]
        public void Resolver_MapsBlasterBranchesToBurstAndRicochetLimits()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.blaster-caset",
                "weapon.blaster-caset.name",
                WeaponRole.DirectProjectile,
                40,
                1.2f,
                0.12f,
                0.7f,
                6f,
                8f,
                20f,
                -1,
                null,
                null);

            var fourShot = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var fiveShot = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var doubleRicochet = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(fourShot.BlasterBurstShotCount, Is.EqualTo(4));
            Assert.That(fiveShot.BlasterBurstShotCount, Is.EqualTo(5));
            Assert.That(doubleRicochet.BlasterBurstShotCount, Is.EqualTo(3));
            Assert.That(doubleRicochet.TerrainBounceLimit, Is.EqualTo(2));
        }

        [Test]
        public void Resolver_MapsDrillBranchesToRangeAndMaterialPenetration()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.bur",
                "weapon.bur.name",
                WeaponRole.ShortRangeBurst,
                18,
                0.7f,
                0.08f,
                0f,
                1.2f,
                3f,
                3f,
                3,
                null,
                null);

            var longRange = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var deepRange = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var universal = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var pioneer = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(longRange.DrillRange, Is.EqualTo(4.45f).Within(0.0001f));
            Assert.That(deepRange.DrillRange, Is.EqualTo(5.65f).Within(0.0001f));
            Assert.That(universal.DrillPenetration, Is.EqualTo(TerrainMaterial.LightMetal));
            Assert.That(pioneer.DrillPenetration, Is.EqualTo(TerrainMaterial.Titanium));
            Assert.That(pioneer.DrillRange, Is.LessThan(deepRange.DrillRange));
        }

        [Test]
        public void Resolver_MapsDrillRocketBranchesToDurationAndMaterialPenetration()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.bur-rocket",
                "weapon.bur-rocket.name",
                WeaponRole.ArcingExplosive,
                46,
                1.25f,
                0.12f,
                0.8f,
                8f,
                5f,
                13f,
                2,
                null,
                null);

            var longRange = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var deepRange = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var universal = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var allTerrain = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(longRange.RocketDrillDurationSeconds, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(deepRange.RocketDrillDurationSeconds, Is.EqualTo(2.25f).Within(0.0001f));
            Assert.That(universal.RocketDrillDurationSeconds, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(universal.RocketDrillPenetration, Is.EqualTo(TerrainMaterial.LightMetal));
            Assert.That(allTerrain.RocketDrillDurationSeconds, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(allTerrain.RocketDrillPenetration, Is.EqualTo(TerrainMaterial.Titanium));
        }

        [Test]
        public void Resolver_MapsHomingMissileBranchesToClusterAndDetourCounts()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.missle",
                "weapon.missle.name",
                WeaponRole.ArcingExplosive,
                52,
                1.4f,
                0.13f,
                0.8f,
                8f,
                5f,
                13f,
                2,
                null,
                null);

            var ballistic = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var clusterBallistic = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var maneuvering = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var tactical = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(ballistic.MissileSplitChargeCount, Is.EqualTo(2));
            Assert.That(clusterBallistic.MissileSplitChargeCount, Is.EqualTo(3));
            Assert.That(maneuvering.MissileObstacleBypassCount, Is.EqualTo(1));
            Assert.That(tactical.MissileObstacleBypassCount, Is.EqualTo(2));
        }

        [Test]
        public void Resolver_MapsC4BranchesToDamageRadiusAndKnockback()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.c4",
                "weapon.c4.name",
                WeaponRole.ShortRangeBurst,
                74,
                1.85f,
                0.1f,
                0f,
                3f,
                0f,
                0f,
                1,
                null,
                null);

            var enhanced = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var breaching = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var impulse = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var shockwave = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(enhanced.C4DamageMultiplier, Is.EqualTo(1.15f).Within(0.0001f));
            Assert.That(enhanced.C4ImpactRadiusMultiplier, Is.EqualTo(1.3f).Within(0.0001f));
            Assert.That(breaching.C4DamageMultiplier, Is.EqualTo(1.3f).Within(0.0001f));
            Assert.That(breaching.C4ImpactRadiusMultiplier, Is.EqualTo(1.55f).Within(0.0001f));
            Assert.That(impulse.KnockbackMultiplier, Is.EqualTo(2.15f).Within(0.0001f));
            Assert.That(impulse.C4ImpactRadiusMultiplier, Is.EqualTo(0.93f).Within(0.0001f));
            Assert.That(shockwave.KnockbackMultiplier, Is.EqualTo(3f).Within(0.0001f));
            Assert.That(shockwave.C4ImpactRadiusMultiplier, Is.EqualTo(0.85f).Within(0.0001f));
        }

        [Test]
        public void Resolver_MapsGrenadeBranchesToStickyMagneticAndGravityEffects()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.granata",
                "weapon.granata.name",
                WeaponRole.ArcingExplosive,
                42,
                1.55f,
                0.14f,
                1.1f,
                4.5f,
                8f,
                24f,
                3,
                null,
                null);

            var sticky = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var magnetic = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var gravity = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var collapse = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(sticky.GrenadeSticks, Is.True);
            Assert.That(sticky.GrenadeMagneticSeekRadius, Is.Zero);
            Assert.That(magnetic.GrenadeSticks, Is.True);
            Assert.That(magnetic.GrenadeMagneticSeekRadius, Is.EqualTo(2.4f).Within(0.0001f));
            Assert.That(gravity.GrenadeGravityPullRadius, Is.EqualTo(3.5f).Within(0.0001f));
            Assert.That(collapse.GrenadeGravityPullRadius, Is.EqualTo(5f).Within(0.0001f));
            Assert.That(gravity.GrenadePullAcceleration, Is.EqualTo(8f).Within(0.0001f));
        }

        [Test]
        public void Resolver_MapsMineBranchesToPlacementAndFriendlyFireRules()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.mine",
                "weapon.mine.name",
                WeaponRole.ArcingExplosive,
                54,
                1.7f,
                0.14f,
                1.05f,
                8f,
                3f,
                10f,
                5,
                null,
                null);

            var fourMines = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var fiveMines = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var recognition = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var friendly = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(fourMines.MineDeployLimitPerTurn, Is.EqualTo(4));
            Assert.That(fiveMines.MineDeployLimitPerTurn, Is.EqualTo(5));
            Assert.That(recognition.MineTriggersEnemiesOnly, Is.True);
            Assert.That(recognition.MineAlliesImmuneToExplosion, Is.False);
            Assert.That(friendly.MineTriggersEnemiesOnly, Is.True);
            Assert.That(friendly.MineAlliesImmuneToExplosion, Is.True);
        }

        [Test]
        public void Resolver_MapsRepairBranchesToProtectionAndRegenerationDurations()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.heal",
                "weapon.heal.name",
                WeaponRole.DefensiveTool,
                34,
                1.4f,
                0.12f,
                0.65f,
                6f,
                3f,
                9f,
                1,
                null,
                null);

            var protective = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var enhancedProtective = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var regenerative = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var longRegenerative = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(protective.RepairProtectionTurns, Is.EqualTo(1));
            Assert.That(enhancedProtective.RepairProtectionTurns, Is.EqualTo(2));
            Assert.That(regenerative.RepairRegenerationTurns, Is.EqualTo(1));
            Assert.That(longRegenerative.RepairRegenerationTurns, Is.EqualTo(2));
        }

        [Test]
        public void Resolver_MapsTeleportBranchesToExchangeAndImpulseEffects()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.teleport",
                "weapon.teleport.name",
                WeaponRole.MovementTool,
                0,
                2.5f,
                0.11f,
                1.1f,
                8f,
                8f,
                24f,
                2,
                null,
                null);

            var exchange = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var tacticalExchange = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var impulse = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var spatialShift = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(exchange.TeleportExchangesEnemy, Is.True);
            Assert.That(exchange.TeleportExchangesAlliesAndObjects, Is.False);
            Assert.That(exchange.TeleportDisorientationTurns, Is.EqualTo(1));
            Assert.That(tacticalExchange.TeleportExchangesEnemy, Is.True);
            Assert.That(tacticalExchange.TeleportExchangesAlliesAndObjects, Is.True);
            Assert.That(impulse.TeleportImpulseRadius, Is.EqualTo(2.7f).Within(0.0001f));
            Assert.That(spatialShift.TeleportImpulseRadius, Is.EqualTo(4.2f).Within(0.0001f));
            Assert.That(spatialShift.TeleportImpulseStrength, Is.GreaterThan(impulse.TeleportImpulseStrength));
        }

        [Test]
        public void Resolver_MapsMortarBranchesToFragmentsAndCascades()
        {
            var weapon = WeaponDefinition.CreateRuntime(
                "weapon.mortira",
                "weapon.mortira.name",
                WeaponRole.ArcingExplosive,
                48,
                1.8f,
                0.16f,
                1.15f,
                9f,
                4f,
                12f,
                2,
                null,
                null);

            var fragment = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.A
            });
            var assault = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.A
            });
            var cascade = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5,
                branch = (int)WeaponUpgradeBranch.B
            });
            var multiCascade = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 10,
                branch = (int)WeaponUpgradeBranch.B
            });

            Assert.That(fragment.MortarFragmentCount, Is.EqualTo(4));
            Assert.That(assault.MortarFragmentCount, Is.EqualTo(5));
            Assert.That(cascade.MortarCascadeCount, Is.EqualTo(1));
            Assert.That(multiCascade.MortarCascadeCount, Is.EqualTo(2));
        }

        private static WeaponDefinition CreateImpulseWeapon()
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.impulse",
                "weapon.impulse.name",
                WeaponRole.DirectProjectile,
                40,
                1.2f,
                0.12f,
                0.7f,
                6f,
                8f,
                20f,
                -1,
                null,
                null);
        }
    }
}
