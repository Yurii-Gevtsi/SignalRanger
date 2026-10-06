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
        [TestCase(4, 3)]
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
            var allDamage = spawner.ImmediateRequests[0].Definition.Damage;
            foreach (var scheduled in spawner.ScheduledRequests)
            {
                allDamage += scheduled.Request.Definition.Damage;
            }
            Assert.That(allDamage, Is.EqualTo(salvoCount switch
            {
                2 => 54,
                3 => 64,
                _ => 72
            }));
            Assert.That(spawner.ImmediateRequests[0].Definition.Damage,
                Is.LessThan(weapon.Damage));
            Assert.That(spawner.ScheduledRequests[0].Delay, Is.EqualTo(0.18f).Within(0.0001f));
        }

        [Test]
        public void Resolver_MapsLegacyImpulseBranchBToTheCanonicalSalvo()
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

            Assert.That(impact.SalvoCount, Is.EqualTo(2));
            Assert.That(assault.SalvoCount, Is.EqualTo(3));
            Assert.That(impact.KnockbackMultiplier, Is.EqualTo(1f));
            Assert.That(assault.KnockbackMultiplier, Is.EqualTo(1f));
        }

        [TestCase("weapon.impulse", 34)]
        [TestCase("weapon.granata", 42)]
        [TestCase("weapon.plasma-sword", 45)]
        [TestCase("weapon.ion-shotgun", 38)]
        public void EarlyWeapons_CorePurchasesUseTheDisplayedDamageCurve(
            string weaponId, int baseDamage)
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                candidate => candidate.WeaponId == weaponId);
            var multipliers = new[] { 1f, 1.5f, 3f, 5f, 10f };
            for (var purchases = 0; purchases <= 4; purchases++)
            {
                var progress = new SaveWeaponProgressData
                {
                    weaponId = weaponId,
                    rank = purchases + 1,
                    upgradeChoices = new List<SaveWeaponUpgradeChoiceData>()
                };
                for (var rank = 2; rank <= progress.rank; rank++)
                {
                    progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    {
                        rank = rank,
                        path = (int)WeaponUpgradePath.Core
                    });
                }

                var shownDamage = WeaponUpgradeChoiceResolver.ResolveValue(
                    weapon, null, progress, WeaponUpgradePath.Core);
                var modifiers = WeaponUpgradeModifierResolver.Resolve(weapon, progress);
                Assert.That(shownDamage, Is.EqualTo(Mathf.RoundToInt(
                    baseDamage * multipliers[purchases])).Within(0.001f));
                Assert.That(modifiers.DamageMultiplier, Is.EqualTo(multipliers[purchases]).Within(0.001f));
            }
        }

        [TestCase("weapon.bur", WeaponUpgradePath.Radius, 3.25f)]
        [TestCase("weapon.impulse", WeaponUpgradePath.Aim, 70f)]
        [TestCase("weapon.rope", WeaponUpgradePath.Aim, 45f)]
        [TestCase("weapon.rope", WeaponUpgradePath.Radius, 8f)]
        [TestCase("weapon.plasma-sword", WeaponUpgradePath.Core, 45f)]
        [TestCase("weapon.ion-shotgun", WeaponUpgradePath.Radius, 0.1f)]
        public void BaseWeaponStats_AreAvailableBeforeAnyProgressIsSaved(
            string weaponId, WeaponUpgradePath path, float expectedValue)
        {
            var weapon = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                candidate => candidate.WeaponId == weaponId);
            Assert.That(WeaponUpgradeChoiceResolver.ResolveValue(weapon, null, null, path),
                Is.EqualTo(expectedValue).Within(0.0001f));
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
        public void Resolver_IgnoresObsoleteC4BranchesWithoutPurchasedSpecials()
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

            foreach (var resolved in new[] { enhanced, breaching, impulse, shockwave })
            {
                Assert.That(resolved.C4DamageMultiplier, Is.EqualTo(1f));
                Assert.That(resolved.C4ImpactRadiusMultiplier, Is.EqualTo(1f));
                Assert.That(resolved.KnockbackMultiplier, Is.EqualTo(1f));
            }
        }

        [Test]
        public void Resolver_MigratesLegacyGrenadeBranchesToTheTwoCurrentPullEffects()
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

            Assert.That(sticky.GrenadeSticks, Is.False);
            Assert.That(sticky.GrenadeMagneticSeekRadius, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(sticky.GrenadeGravityPullRadius, Is.Zero);
            Assert.That(magnetic.GrenadeSticks, Is.False);
            Assert.That(magnetic.GrenadeGravityPullRadius, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(gravity.GrenadeMagneticSeekRadius, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(gravity.GrenadeGravityPullRadius, Is.Zero);
            Assert.That(collapse.GrenadeGravityPullRadius, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(collapse.GrenadePullAcceleration, Is.EqualTo(8f).Within(0.0001f));
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
        public void Resolver_MapsTheSingleMortarFeatureToFiveCascadeCounts()
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

            var withoutFeature = WeaponEvolutionRuntimeResolver.Resolve(weapon, new SaveWeaponProgressData
            {
                weaponId = weapon.WeaponId,
                rank = 5
            });
            Assert.That(withoutFeature.MortarCascadeCount, Is.Zero);
            Assert.That(withoutFeature.MortarFragmentCount, Is.Zero);

            // Five paid steps of the one feature give 2/3/4/5/6 physical shards.
            for (var steps = 1; steps <= 5; steps++)
            {
                var progress = new SaveWeaponProgressData { weaponId = weapon.WeaponId, rank = 5 + steps };
                progress.EnsureDefaults();
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "cascade-mortar" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = steps });
                var profile = WeaponEvolutionRuntimeResolver.Resolve(weapon, progress);
                Assert.That(profile.MortarCascadeCount, Is.EqualTo(steps + 1), $"purchases={steps}");
                Assert.That(profile.MortarFragmentCount, Is.Zero);
            }
        }

        [Test]
        public void PurchasedImpulseSteps_SetTwoThroughFourShotsWithoutRequiringRankTen()
        {
            var weapon = CreateImpulseWeapon();
            for (var steps = 1; steps <= 3; steps++)
            {
                var progress = new SaveWeaponProgressData
                {
                    weaponId = weapon.WeaponId,
                    rank = 5 + steps,
                    evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                    {
                        new() { rank = 5, optionId = "double-impulse" }
                    },
                    specialUpgrades = new List<SaveWeaponSpecialUpgradeData>
                    {
                        new() { rank = 5, purchaseCount = steps }
                    }
                };

                Assert.That(WeaponEvolutionRuntimeResolver.Resolve(weapon, progress).SalvoCount,
                    Is.EqualTo(1 + steps));
            }
        }

        [Test]
        public void PurchasedMineSafetyAndGrenadePulls_StackIndependentMilestones()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            var mine = System.Array.Find(weapons, value => value.WeaponId == "weapon.mine");
            var grenade = System.Array.Find(weapons, value => value.WeaponId == "weapon.granata");
            var mineProgress = new SaveWeaponProgressData
            {
                weaponId = mine.WeaponId,
                rank = 11,
                evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                {
                    new() { rank = 5, optionId = "ally-safe" },
                    new() { rank = 10, optionId = "ally-immune" }
                }
            };
            var grenadeProgress = new SaveWeaponProgressData
            {
                weaponId = grenade.WeaponId,
                rank = 11,
                evolutionChoices = new List<SaveWeaponEvolutionChoiceData>
                {
                    new() { rank = 5, optionId = "target-pull" },
                    new() { rank = 10, optionId = "gravity-pull" }
                }
            };

            var mineEvolution = WeaponEvolutionRuntimeResolver.Resolve(mine, mineProgress);
            var grenadeEvolution = WeaponEvolutionRuntimeResolver.Resolve(grenade, grenadeProgress);
            Assert.That(mineEvolution.MineTriggersEnemiesOnly, Is.True);
            Assert.That(mineEvolution.MineAlliesImmuneToExplosion, Is.True);
            Assert.That(grenadeEvolution.GrenadeMagneticSeekRadius, Is.GreaterThan(0f));
            Assert.That(grenadeEvolution.GrenadeGravityPullRadius, Is.GreaterThan(0f));
            Assert.That(grenadeEvolution.GrenadeSticks, Is.False);
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
