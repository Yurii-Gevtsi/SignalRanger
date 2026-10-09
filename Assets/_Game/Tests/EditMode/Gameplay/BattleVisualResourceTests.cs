using System.Collections.Generic;
using NUnit.Framework;
using Game.App;
using Game.Core;
using Game.Gameplay.Battle.Weapons;
using Game.Gameplay.Battle.Assembly;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Visuals;
using Game.UI.Battle;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleVisualResourceTests
    {
        [Test]
        public void TrainingArena_UsesOneImportedDestructibleSurfaceAndReusableAnimatedBackground()
        {
            var trainingVisual = Resources.Load<Sprite>("Visuals/Arenas/Training/training_reconstructed_visual");
            var trainingMask = Resources.Load<Texture2D>("Visuals/Arenas/Training/training_reconstructed_mask");
            var laserKillZone = Resources.Load<Sprite>("Visuals/Arenas/KillZones/LaserLine/laser_line");
            var backgroundPrefab = Resources.Load<GameObject>("Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground");

            Assert.IsNotNull(trainingVisual);
            Assert.IsNotNull(trainingMask);
            Assert.IsNotNull(laserKillZone);
            Assert.IsNotNull(backgroundPrefab);
            Assert.IsNotNull(backgroundPrefab.GetComponent<AnimatedSkyBackground>());
            Assert.AreEqual(new Vector3(0f, 1f, 0f), backgroundPrefab.transform.localPosition);
            Assert.AreEqual(new Vector3(1.5f, 1.2f, 1f), backgroundPrefab.transform.localScale);
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/empty_back"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/cloud1"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/cloud2"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/small_planet"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/station"));

            var backgroundInstance = Object.Instantiate(backgroundPrefab);
            try
            {
                Assert.IsNotNull(backgroundInstance.transform.Find("Empty Background"));
                Assert.IsNotNull(backgroundInstance.transform.Find("Orbiting Planet"));
                Assert.IsNotNull(backgroundInstance.transform.Find("Floating Station"));
                Assert.That(backgroundInstance.transform.Find("Cloud 2 Near").localEulerAngles.z,
                    Is.EqualTo(355f).Within(0.01f));
                Assert.That(backgroundInstance.transform.Find("Cloud 2 Far").localEulerAngles.z,
                    Is.EqualTo(355f).Within(0.01f));
                Assert.AreEqual(7, backgroundInstance.transform.childCount);
            }
            finally
            {
                Object.DestroyImmediate(backgroundInstance);
            }

            Assert.IsTrue(trainingVisual.texture.isReadable);
            Assert.AreEqual(4013, trainingVisual.texture.width);
            Assert.AreEqual(2258, trainingVisual.texture.height);
            Assert.That(trainingVisual.pixelsPerUnit, Is.EqualTo(256f).Within(0.01f));
            Assert.IsTrue(trainingMask.isReadable);
            Assert.AreEqual(4013, trainingMask.width);
            Assert.AreEqual(2258, trainingMask.height);
            Assert.AreEqual(Color.black, trainingMask.GetPixel(0, 0));

            Assert.AreEqual(0, laserKillZone.texture.GetPixel(0, 0).a);
        }

        [Test]
        public void ImportedTrainingAndBonesSurfacesHaveMatchingReadableMasks()
        {
            foreach (var id in new[] { "training_reconstructed" })
            {
                var visual = Resources.Load<Sprite>($"Visuals/Arenas/Training/{id}_visual");
                var mask = Resources.Load<Texture2D>($"Visuals/Arenas/Training/{id}_mask");
                Assert.IsNotNull(visual, id + " visual");
                Assert.IsNotNull(mask, id + " mask");
                Assert.IsTrue(visual.texture.isReadable, id + " visual readable");
                Assert.IsTrue(mask.isReadable, id + " mask readable");
                Assert.AreEqual(visual.texture.width, mask.width, id + " width");
                Assert.AreEqual(visual.texture.height, mask.height, id + " height");
                Assert.AreEqual(256f, visual.pixelsPerUnit, 0.01f, id + " PPU");
            }

            var bonesVisual = Resources.Load<Sprite>("Visuals/Arenas/Bones/bones_reconstructed_visual");
            var bonesMask = Resources.Load<Texture2D>("Visuals/Arenas/Bones/bones_reconstructed_mask");
            Assert.IsNotNull(bonesVisual);
            Assert.IsNotNull(bonesMask);
            Assert.IsTrue(bonesVisual.texture.isReadable);
            Assert.IsTrue(bonesMask.isReadable);
            Assert.AreEqual(bonesVisual.texture.width, bonesMask.width);
            Assert.AreEqual(bonesVisual.texture.height, bonesMask.height);
        }

        [Test]
        public void CharacterResources_ContainDroidAndSigmaBattleRigsAndMenuRaccoon()
        {
            var droidRig = Resources.Load<GameObject>("Visuals/Characters/DroidRig");
            var sigmaRig = Resources.Load<GameObject>("Visuals/Characters/SigmaRig");
            Assert.IsNotNull(droidRig);
            Assert.IsNotNull(sigmaRig);
            Assert.IsNotNull(Resources.Load<Sprite>("Visuals/Characters/racoon-full"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/oct_full"));

            var animator = droidRig.GetComponent<Animator>();
            Assert.IsNotNull(animator);
            Assert.IsTrue(System.Array.Exists(animator.parameters,
                parameter => parameter.name == "IsMoving" && parameter.type == AnimatorControllerParameterType.Bool));
            Assert.IsNotNull(droidRig.transform.Find("Platform")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(droidRig.transform.Find("Body")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(droidRig.transform.Find("Body/Screen Background")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(droidRig.transform.Find("Body/Face")?.GetComponent<SpriteRenderer>());

            var platform = droidRig.transform.Find("Platform");
            var body = droidRig.transform.Find("Body");
            var screen = droidRig.transform.Find("Body/Screen Background");
            var face = droidRig.transform.Find("Body/Face");
            Assert.AreEqual(new Vector3(0.6f, 0.6f, 1f), platform.localScale);
            Assert.AreEqual(new Vector3(1f, 1f, 1f), body.localScale);
            Assert.AreEqual(new Vector3(0.35f, 0.35f, 1f), face.localScale);
            Assert.AreEqual(1.8f, body.localPosition.y);
            Assert.AreEqual(Color.white, screen.GetComponent<SpriteRenderer>().color);
            Assert.AreEqual(new Vector3(3.18f, 2.72f, 1f), screen.localScale);

            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat-builder-soft"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat-builder"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat2-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/ent-guardian-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/raccoon-agent-clean"));

            var sigmaAnimator = sigmaRig.GetComponent<Animator>();
            Assert.IsNotNull(sigmaAnimator);
            Assert.IsTrue(System.Array.Exists(sigmaAnimator.parameters,
                parameter => parameter.name == "IsMoving" && parameter.type == AnimatorControllerParameterType.Bool));
            Assert.IsNotNull(sigmaRig.transform.Find("Platform")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(sigmaRig.transform.Find("Body")?.GetComponent<SpriteRenderer>());
            Assert.IsNull(sigmaRig.transform.Find("Body/Face"));
        }

        [Test]
        public void BattleHudResources_UseArsenalAndWeaponIconsWithoutDedicatedFireArt()
        {
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Battle/arsenal"));
            Assert.IsNull(Resources.Load<Texture2D>("UI/Battle/fire_btn"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Weapons/bazooka_simple"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Battle/card"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Menu/settings_btn"));
        }

        [Test]
        public void RepairJammerStatus_UsesTheApprovedTransparentIconResource()
        {
            var icon = Resources.Load<Texture2D>("UI/Common/no-repair-2");
            Assert.IsNotNull(icon);
            Assert.That(icon.width, Is.EqualTo(512));
            Assert.That(icon.height, Is.EqualTo(512));
        }

        [Test]
        public void WeaponsScreenResources_ContainShelfBackgroundAndOwnedWeaponIcons()
        {
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Menu/weapons_menu"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Common/architect_btn"));

            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                if (weapon == null || WeaponPresentationText.IsDropOnly(weapon)
                    || string.IsNullOrWhiteSpace(weapon.IconId))
                {
                    continue;
                }

                Assert.IsNotNull(Resources.Load<Texture2D>($"UI/Weapons/{weapon.IconId}"), weapon.WeaponId);
            }
        }

        [Test]
        public void CombatProjectileResources_ExistIndependentlyFromHeldWeaponArt()
        {
            var projectileSpriteIds = new[]
            {
                "blaster_shot",
                "impulse_shot",
                "bur_lazer",
                "bur_rocket",
                "mortira_shot",
                "emp_shot",
                "ice_shot",
                "toxic_shot",
                "fire_shot_1",
                "missle_fly",
                "missle_tail"
            };

            foreach (var spriteId in projectileSpriteIds)
            {
                Assert.IsNotNull(Resources.Load<Texture2D>($"UI/Weapons/{spriteId}"), spriteId);
            }
        }

        [Test]
        public void DrillLaser_FiringShowsTheTransparentBeamSprite()
        {
            var root = new GameObject("Drill Laser Presentation Test");
            GameObject beam = null;
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var handler = typeof(BattleSceneController).GetMethod(
                    "OnSimulationLinearWeaponEffectStarted",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var arenaObjects = typeof(BattleSceneController).GetField(
                    "_arenaObjects",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                Assert.That(handler, Is.Not.Null);
                Assert.That(arenaObjects, Is.Not.Null);
                var drill = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.bur");
                UnityEngine.TestTools.LogAssert.Expect(LogType.Error,
                    new System.Text.RegularExpressions.Regex("Destroy may not be called from edit mode"));
                handler.Invoke(controller, new object[]
                {
                    new LinearWeaponEffectEvent(drill, BattleTeamId.Player,
                        Vector2.zero, Vector2.right, 3.25f, 0.4f, 1f)
                });

                var visuals = (List<GameObject>)arenaObjects.GetValue(controller);
                beam = visuals[visuals.Count - 1];
                var sprite = beam.GetComponent<SpriteRenderer>().sprite;
                Assert.That(sprite, Is.Not.Null);
                Assert.That(sprite.texture,
                    Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/bur_lazer")));
                Assert.That(beam.GetComponent<LinearWeaponEffectVisual>(), Is.Not.Null);
            }
            finally
            {
                if (beam != null)
                {
                    Object.DestroyImmediate(beam);
                }
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Grenade_FlightUsesItsThrownBodyAndPurchasedSkin()
        {
            var root = new GameObject("Grenade Flight Art Test");
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var grenade = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.granata");
                var projectile = new SimProjectile(1, grenade.CreateProjectileDefinition(),
                    "player.test.01", Vector2.zero, Vector2.right, 3f);
                var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                var resolve = typeof(BattleSceneController).GetMethod("ResolveProjectileSprite", flags);
                var progressionField = typeof(BattleSceneController).GetField("_playerProgression", flags);
                Assert.That(resolve, Is.Not.Null);
                Assert.That(progressionField, Is.Not.Null);

                var baseSprite = (Sprite)resolve.Invoke(controller, new object[] { projectile, grenade });
                Assert.That(baseSprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/granata")));

                var progress = new Game.Data.Save.SaveWeaponProgressData
                {
                    weaponId = grenade.WeaponId,
                    rank = 5,
                    evolutionChoices = new List<Game.Data.Save.SaveWeaponEvolutionChoiceData>
                    {
                        new() { rank = 5, optionId = "target-pull" }
                    }
                };
                progressionField.SetValue(controller, BattleWeaponProgression.FromResolver(
                    new[] { grenade }, weaponId => weaponId == grenade.WeaponId ? progress : null));
                var upgradedSprite = (Sprite)resolve.Invoke(controller, new object[] { projectile, grenade });
                Assert.That(upgradedSprite.texture,
                    Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/granata_5_10_B")));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [TestCase(0, 67.5f)]
        [TestCase(4, 59.5f)]
        public void Rope_AimPurchasesExpandTheSameSectorShownAndUsedInBattle(
            int purchases, float expectedAngle)
        {
            var root = new GameObject("Rope Aim Sector Test");
            try
            {
                var rope = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.rope");
                var progress = new Game.Data.Save.SaveWeaponProgressData
                {
                    weaponId = rope.WeaponId,
                    rank = purchases + 1,
                    upgradeChoices = new List<Game.Data.Save.SaveWeaponUpgradeChoiceData>()
                };
                for (var rank = 2; rank <= progress.rank; rank++)
                {
                    progress.upgradeChoices.Add(new Game.Data.Save.SaveWeaponUpgradeChoiceData
                    {
                        rank = rank,
                        path = (int)Game.Data.Progression.WeaponUpgradePath.Aim
                    });
                }

                var controller = root.AddComponent<BattleSceneController>();
                var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                typeof(BattleSceneController).GetField("_inventory", flags)
                    .SetValue(controller, new WeaponInventoryRuntime(new[] { rope }));
                typeof(BattleSceneController).GetField("_playerProgression", flags)
                    .SetValue(controller, BattleWeaponProgression.FromResolver(
                        new[] { rope }, id => id == rope.WeaponId ? progress : null));
                var method = typeof(BattleSceneController).GetMethod(
                    "ClampRopeAimToUpwardSector", flags);
                Assert.That(method, Is.Not.Null);
                var result = (AimState)method.Invoke(controller, new object[]
                {
                    new AimState(Vector2.zero, Vector2.right, 1f, true)
                });
                Assert.That(Mathf.Atan2(result.Direction.y, result.Direction.x) * Mathf.Rad2Deg,
                    Is.EqualTo(expectedAngle).Within(0.001f));
                Assert.That(WeaponUpgradeChoiceResolver.ResolveRopeAimSectorDegrees(purchases),
                    Is.EqualTo(180f - expectedAngle * 2f).Within(0.001f));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [TestCase(0, 3)]
        [TestCase(1, 4)]
        [TestCase(4, 7)]
        public void Grenade_LoadoutPurchasesIncreaseBattleAmmo(int purchases, int expectedAmmo)
        {
            var grenade = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.granata");
            var progress = new Game.Data.Save.SaveWeaponProgressData
            {
                weaponId = grenade.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<Game.Data.Save.SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new Game.Data.Save.SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)Game.Data.Progression.WeaponUpgradePath.Loadout
                });
            }

            var progression = BattleWeaponProgression.FromResolver(new[] { grenade },
                weaponId => weaponId == grenade.WeaponId ? progress : null);
            var inventory = new WeaponInventoryRuntime(new[] { grenade });
            var apply = typeof(BattleSceneController).GetMethod("ApplyProgressionAmmo",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            Assert.That(apply, Is.Not.Null);
            apply.Invoke(null, new object[] { inventory, progression });

            Assert.That(inventory.GetRemainingAmmo(grenade), Is.EqualTo(expectedAmmo));
            for (var shot = 0; shot < expectedAmmo; shot++)
            {
                Assert.That(inventory.TryConsumeSelectedAmmo(), Is.True);
            }
            Assert.That(inventory.TryConsumeSelectedAmmo(), Is.False);
        }

        [TestCase("weapon.granata", 0, 3.5f)]
        [TestCase("weapon.granata", 1, 3.7f)]
        [TestCase("weapon.granata", 4, 4.3f)]
        [TestCase("weapon.impulse", 0, 3.5f)]
        [TestCase("weapon.impulse", 1, 3.7f)]
        [TestCase("weapon.impulse", 4, 4.3f)]
        public void AimedStarter_AimPurchasesExtendOnlyItsTrajectoryPreview(
            string weaponId, int purchases, float expectedLength)
        {
            var root = new GameObject("Starter Aim Preview Test");
            try
            {
                var grenade = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.granata");
                var impulse = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.impulse");
                var selected = weaponId == grenade.WeaponId ? grenade : impulse;
                var other = weaponId == grenade.WeaponId ? impulse : grenade;
                var progress = new Game.Data.Save.SaveWeaponProgressData
                {
                    weaponId = selected.WeaponId,
                    rank = 1 + purchases,
                    upgradeChoices = new List<Game.Data.Save.SaveWeaponUpgradeChoiceData>()
                };
                for (var rank = 2; rank <= progress.rank; rank++)
                {
                    progress.upgradeChoices.Add(new Game.Data.Save.SaveWeaponUpgradeChoiceData
                    {
                        rank = rank,
                        path = (int)Game.Data.Progression.WeaponUpgradePath.Aim
                    });
                }

                var controller = root.AddComponent<BattleSceneController>();
                var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                var progressionField = typeof(BattleSceneController).GetField("_playerProgression", flags);
                var resolve = typeof(BattleSceneController).GetMethod("ResolveTrajectoryPreviewLength", flags);
                Assert.That(progressionField, Is.Not.Null);
                Assert.That(resolve, Is.Not.Null);
                progressionField.SetValue(controller, BattleWeaponProgression.FromResolver(
                    new[] { selected }, id => id == selected.WeaponId ? progress : null));

                Assert.That((float)resolve.Invoke(controller, new object[] { selected, 3.5f }),
                    Is.EqualTo(expectedLength).Within(0.001f));
                Assert.That((float)resolve.Invoke(controller, new object[] { other, 3.5f }),
                    Is.EqualTo(3.5f));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [TestCase(0, 3)]
        [TestCase(4, 7)]
        public void IonShotgun_LoadoutPurchasesIncreaseBattleAmmo(int purchases, int expectedAmmo)
        {
            var shotgun = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.ion-shotgun");
            var progress = new Game.Data.Save.SaveWeaponProgressData
            {
                weaponId = shotgun.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<Game.Data.Save.SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new Game.Data.Save.SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)Game.Data.Progression.WeaponUpgradePath.Loadout
                });
            }

            var progression = BattleWeaponProgression.FromResolver(new[] { shotgun },
                weaponId => weaponId == shotgun.WeaponId ? progress : null);
            var inventory = new WeaponInventoryRuntime(new[] { shotgun });
            var apply = typeof(BattleSceneController).GetMethod("ApplyProgressionAmmo",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            Assert.That(apply, Is.Not.Null);
            apply.Invoke(null, new object[] { inventory, progression });

            Assert.That(inventory.GetRemainingAmmo(shotgun), Is.EqualTo(expectedAmmo));
        }

        [Test]
        public void IonShotgun_FlyingPelletsAreVisibleBlackCirclesWithoutShotArt()
        {
            var root = new GameObject("Ion Shotgun Pellet Visual Test");
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var shotgun = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.ion-shotgun");
                var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                var spriteMethod = typeof(BattleSceneController).GetMethod("ResolveProjectileSprite", flags);
                var colorMethod = typeof(BattleSceneController).GetMethod("ResolveProjectileVisualColor", flags);
                var scaleMethod = typeof(BattleSceneController).GetMethod("ResolveProjectileVisualScale",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                Assert.That(spriteMethod, Is.Not.Null);
                Assert.That(colorMethod, Is.Not.Null);
                Assert.That(scaleMethod, Is.Not.Null);

                // Owner decision 2026-10-09: plain black circles, four times the former invisible 0.02.
                Assert.That(spriteMethod.Invoke(controller, new object[] { null, shotgun }), Is.Null);
                Assert.That((Color)colorMethod.Invoke(controller, new object[] { shotgun }), Is.EqualTo(Color.black));
                Assert.That((Vector3)scaleMethod.Invoke(null,
                    new object[] { shotgun, shotgun.ProjectileRadius * 2f }),
                    Is.EqualTo(Vector3.one * 0.08f));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        public void C4_LoadoutPurchasesIncreaseBattleSupply(int purchases, int expectedAmmo)
        {
            var c4 = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                weapon => weapon.WeaponId == "weapon.c4");
            var progress = new Game.Data.Save.SaveWeaponProgressData
            {
                weaponId = c4.WeaponId,
                rank = 1 + purchases,
                upgradeChoices = new List<Game.Data.Save.SaveWeaponUpgradeChoiceData>()
            };
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                progress.upgradeChoices.Add(new Game.Data.Save.SaveWeaponUpgradeChoiceData
                {
                    rank = rank,
                    path = (int)Game.Data.Progression.WeaponUpgradePath.Loadout
                });
            }

            var progression = BattleWeaponProgression.FromResolver(new[] { c4 },
                weaponId => weaponId == c4.WeaponId ? progress : null);
            var inventory = new WeaponInventoryRuntime(new[] { c4 });
            var apply = typeof(BattleSceneController).GetMethod("ApplyProgressionAmmo",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            Assert.That(apply, Is.Not.Null);
            apply.Invoke(null, new object[] { inventory, progression });
            Assert.That(inventory.GetRemainingAmmo(c4), Is.EqualTo(expectedAmmo));
        }

        [Test]
        public void C4_DroppedChargeUsesTheWeaponBodySprite()
        {
            var root = new GameObject("C4 Ground Visual Test");
            GameObject chargeVisual = null;
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var c4 = System.Array.Find(PrototypeWeaponLibrary.CreateAll(),
                    weapon => weapon.WeaponId == "weapon.c4");
                var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                var handler = typeof(BattleSceneController).GetMethod("OnSimulationChargeDeployed", flags);
                var visualsField = typeof(BattleSceneController).GetField("_mineVisuals", flags);
                Assert.That(handler, Is.Not.Null);
                Assert.That(visualsField, Is.Not.Null);
                handler.Invoke(controller, new object[]
                {
                    new ChargeDeployedEvent(7, Vector2.up, Vector2.zero, 0.4f,
                        c4, BattleTeamId.Opponent)
                });

                var visuals = (Dictionary<int, GameObject>)visualsField.GetValue(controller);
                chargeVisual = visuals[7];
                var sprite = chargeVisual.GetComponent<SpriteRenderer>().sprite;
                Assert.That(sprite, Is.Not.Null);
                Assert.That(sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/C4")));
                Assert.That(chargeVisual.GetComponent<GroundDropVisual>(), Is.Not.Null);
            }
            finally
            {
                if (chargeVisual != null)
                {
                    Object.DestroyImmediate(chargeVisual);
                }
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void OrbitalRocket_TimerExpiryInvalidatesTheVisibleDraftMarker()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "ShouldClearOrbitalTargetMarkerOnTurnAdvanced",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Assert.IsNotNull(method, "the scene requires an explicit turn-boundary rule for orbital marker cleanup");
            Assert.That((bool)method.Invoke(null, new object[] { TurnAdvanceReason.PlayerTimerExpired }), Is.True);
            Assert.That((bool)method.Invoke(null, new object[] { TurnAdvanceReason.InstantActionCompleted }), Is.False);
        }

        [Test]
        public void Grounder_DeploymentShowsItsSkinAndRemovalClearsIt()
        {
            var root = new GameObject("Grounder Presentation Test");
            GameObject visual = null;
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                var deployed = typeof(BattleSceneController).GetMethod("OnSimulationGrounderDeployed", flags);
                var removed = typeof(BattleSceneController).GetMethod("OnSimulationGrounderRemoved", flags);
                var visualsField = typeof(BattleSceneController).GetField("_grounderVisuals", flags);
                Assert.IsNotNull(deployed);
                Assert.IsNotNull(removed);
                Assert.IsNotNull(visualsField);

                var weapon = WeaponDefinition.CreateRuntime(
                    "weapon.grounder", "weapon.grounder.name", WeaponRole.DefensiveTool,
                    0, 1.4f, 0.1f, 0f, 5f, 4f, 8f, 1, null, null, iconId: "grounder");
                var point = new Vector2(2f, -1f);
                deployed.Invoke(controller, new object[]
                {
                    new GrounderDeployedEvent(47, point, weapon, BattleTeamId.Player)
                });

                var visuals = (Dictionary<int, GameObject>)visualsField.GetValue(controller);
                Assert.That(visuals.TryGetValue(47, out visual), Is.True);
                var renderer = visual.GetComponent<SpriteRenderer>();
                Assert.IsNotNull(renderer);
                Assert.That(renderer.enabled, Is.True);
                Assert.IsNotNull(renderer.sprite);
                Assert.That(visual.transform.position.x, Is.EqualTo(point.x).Within(0.0001f));
                // The deploy point sits 0.06 above the surface; the magnet's bottom edge rests on the surface.
                Assert.That(renderer.bounds.min.y, Is.EqualTo(point.y - 0.06f).Within(0.001f), "it stands on the ground");
                Assert.That(renderer.sprite.texture,
                    Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/grounder")));

                UnityEngine.TestTools.LogAssert.Expect(LogType.Error,
                    new System.Text.RegularExpressions.Regex("Destroy may not be called from edit mode"));
                removed.Invoke(controller, new object[] { 47 });
                Assert.That(visuals.ContainsKey(47), Is.False);
            }
            finally
            {
                if (visual != null)
                {
                    Object.DestroyImmediate(visual);
                }
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void OrbitalRocket_UsesMissileFlightResource()
        {
            var root = new GameObject("Orbital Rocket Presentation Test");
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var weapon = WeaponDefinition.CreateRuntime(
                    "weapon.orbital-rocket",
                    "weapon.orbital-rocket.name",
                    WeaponRole.ArcingExplosive,
                    65,
                    2.1f,
                    0.16f,
                    1.1f,
                    6f,
                    5f,
                    14f,
                    2,
                    null,
                    new[] { "arc", "orbital" },
                    iconId: "missle_5_10_A");
                var projectile = new SimProjectile(
                    1,
                    weapon.CreateProjectileDefinition(),
                    "player.test.01",
                    Vector2.zero,
                    Vector2.down,
                    2f);
                var resolver = typeof(BattleSceneController).GetMethod(
                    "ResolveProjectileSprite",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                Assert.IsNotNull(resolver, "projectile sprite selection must remain an explicit battle-scene seam");
                var sprite = resolver.Invoke(controller, new object[] { projectile, weapon }) as Sprite;

                Assert.IsNotNull(sprite, "orbital rocket requires an explicit flight sprite");
                Assert.That(sprite.texture.name, Is.EqualTo("missle_fly"));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void GuardianDrone_UsesSmallMortarShotSprite()
        {
            var root = new GameObject("Guardian Drone Presentation Test");
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var weapon = WeaponDefinition.CreateRuntime(
                    "weapon.guardian-drone",
                    "weapon.guardian-drone.name",
                    WeaponRole.DefensiveTool,
                    42,
                    1.6f,
                    0.12f,
                    1f,
                    3f,
                    0f,
                    0f,
                    2,
                    null,
                    new[] { "ground", "drone" },
                    iconId: "guardian_drone");
                var projectile = new SimProjectile(
                    1,
                    weapon.CreateProjectileDefinition(),
                    "player.test.01",
                    Vector2.zero,
                    Vector2.right,
                    2f);
                var resolver = typeof(BattleSceneController).GetMethod(
                    "ResolveProjectileSprite",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                Assert.IsNotNull(resolver);
                var sprite = resolver.Invoke(controller, new object[] { projectile, weapon }) as Sprite;

                Assert.IsNotNull(sprite, "the guardian's guaranteed shot needs a real projectile skin");
                Assert.That(sprite.texture.name, Is.EqualTo("mortira_shot"));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void SwapJump_UsesTeleportProjectileSprite()
        {
            var root = new GameObject("Swap Jump Presentation Test");
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var weapon = WeaponDefinition.CreateRuntime(
                    "weapon.swap-jump",
                    "weapon.swap-jump.name",
                    WeaponRole.MovementTool,
                    0,
                    1.4f,
                    0.12f,
                    1f,
                    3f,
                    0f,
                    0f,
                    1,
                    null,
                    new[] { "movement", "teleport" },
                    iconId: "teleport_5_10_A");
                var projectile = new SimProjectile(
                    1,
                    weapon.CreateProjectileDefinition(),
                    "player.test.01",
                    Vector2.zero,
                    Vector2.right,
                    2f);
                var resolver = typeof(BattleSceneController).GetMethod(
                    "ResolveProjectileSprite",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                Assert.IsNotNull(resolver);
                var sprite = resolver.Invoke(controller, new object[] { projectile, weapon }) as Sprite;

                Assert.IsNotNull(sprite, "swap-jump requires an in-flight teleport skin");
                Assert.That(sprite.texture.name, Is.EqualTo("teleport_5_10_A"));

                var ordinaryTeleport = WeaponDefinition.CreateRuntime(
                    "weapon.teleport", "weapon.teleport.name", WeaponRole.MovementTool,
                    0, 1.4f, 0.12f, 1f, 3f, 0f, 0f, 1, null,
                    new[] { "movement", "teleport" }, iconId: "teleport_5_10_A");
                var scaleResolver = typeof(BattleSceneController).GetMethod(
                    "ResolveProjectileVisualScale",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                Assert.IsNotNull(scaleResolver);
                var swapScale = (Vector3)scaleResolver.Invoke(null, new object[] { weapon, 0.24f });
                var teleportScale = (Vector3)scaleResolver.Invoke(null, new object[] { ordinaryTeleport, 0.24f });
                Assert.That(swapScale, Is.EqualTo(teleportScale),
                    "the two teleport spheres must have the same in-flight size");
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void RopeWeapon_UsesDedicatedHookArt()
        {
            var ropeWeapon = Resources.Load<Texture2D>("UI/Weapons/rope");
            var ropeHook = Resources.Load<Texture2D>("UI/Weapons/rope_shot");

            Assert.IsNotNull(ropeWeapon);
            Assert.IsNotNull(ropeHook);
            Assert.AreNotSame(ropeWeapon, ropeHook);
            Assert.AreEqual("rope_shot", ropeHook.name);
        }

        [Test]
        public void ElectroStatusOverlay_HasFourAnimationFrames()
        {
            for (var frame = 1; frame <= 4; frame++)
            {
                var texture = Resources.Load<Texture2D>($"UI/Generated/electro_overlay_0{frame}");
                Assert.IsNotNull(texture, frame.ToString());
            }
        }

        [Test]
        public void LoadingScreenResources_ContainAllBackgroundsAndSpinner()
        {
            for (var i = 1; i <= 5; i++)
            {
                Assert.IsNotNull(Resources.Load<Texture2D>($"UI/Loaders/{i}"), $"Loader background {i} is missing.");
            }

            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Loaders/loader3"));
            AssertMagentaBackgroundIsTransparent(Resources.Load<Texture2D>("UI/Loaders/loader3"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Common/card2"));
            AssertMagentaBackgroundIsTransparent(Resources.Load<Texture2D>("UI/Common/architect_btn"));

            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Startup/startup_02"));
        }

        [Test]
        public void LoadingScreenPreview_BuildsMissionTextSpinnerAndPercent()
        {
            var root = new GameObject("Loading Screen Test");
            try
            {
                var controller = root.AddComponent<LoadingScreenController>();
                controller.InitializeForPreview(GameSceneNames.Battle);

                var canvas = root.transform.Find("Loading Canvas");
                Assert.IsNotNull(canvas);
                Assert.IsNotNull(RequireRect(canvas, "Loading Background").GetComponent<Image>().sprite);
                Assert.IsNotNull(RequireRect(canvas, "Mission Description Card").GetComponent<Image>().sprite);
                var missionDescription = RequireRect(canvas, "Mission Description");
                Assert.IsNotNull(missionDescription.GetComponent<TMPro.TMP_Text>());
                Assert.AreEqual(new Vector2(-420f, 245f), missionDescription.anchoredPosition);
                Assert.AreEqual(new Vector2(430f, 150f), missionDescription.sizeDelta);
                Assert.IsFalse(missionDescription.GetComponent<TMPro.TMP_Text>().enableAutoSizing);
                Assert.AreEqual("Навчальна місія", missionDescription.GetComponent<TMPro.TMP_Text>().text);
                var ring = RequireRect(canvas, "Loading Ring");
                Assert.AreEqual(new Vector2(224f, 224f), ring.sizeDelta);
                Assert.AreEqual(new Vector2(0f, -400f), ring.anchoredPosition);
                Assert.AreEqual(Vector3.one, ring.localScale);
                Assert.AreEqual("loader3", ring.GetComponent<Image>().sprite.texture.name);

                var percent = RequireRect(canvas, "Loading Percent");
                Assert.AreEqual("0%", percent.GetComponent<TMPro.TMP_Text>().text);
                Assert.AreEqual(ring.anchoredPosition, percent.anchoredPosition);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void WeaponsScreen_ShowsShelfActionsAndDetailsWithoutPurchaseButtons()
        {
            var root = new GameObject("Weapons Screen Test");
            try
            {
                root.AddComponent<WeaponsScreenController>();
                var canvas = root.transform.Find("Weapons Canvas");
                Assert.IsNotNull(canvas);

                var settings = RequireRect(canvas, "Settings Button");
                Assert.AreEqual(new Vector2(140f, 140f), settings.sizeDelta);
                Assert.That(settings.anchoredPosition.x, Is.EqualTo(-26f).Within(0.01f));

                var bureau = RequireRect(canvas, "Constructor Bureau Button");
                Assert.AreEqual(new Vector2(220f, 220f), bureau.sizeDelta);
                Assert.That(bureau.anchoredPosition.x, Is.EqualTo(-80f).Within(0.01f));
                Assert.That(bureau.anchoredPosition.y, Is.EqualTo(30f).Within(0.01f));
                Assert.That(bureau.localScale.x, Is.EqualTo(1.2f).Within(0.01f));
                Assert.That(bureau.localScale.y, Is.EqualTo(1.5f).Within(0.01f));
                Assert.AreEqual("architect_btn", bureau.GetComponent<Image>().sprite.texture.name);
                Assert.IsNotNull(bureau.Find("Constructor Bureau Label"));

                var firstWeapon = RequireRect(canvas, "Shelf Weapon 0");
                Assert.AreEqual(new Vector2(195f, 195f), firstWeapon.sizeDelta);
                Assert.That(firstWeapon.anchoredPosition.x, Is.EqualTo(-730f).Within(0.01f));
                Assert.That(firstWeapon.anchoredPosition.y, Is.EqualTo(338f).Within(0.01f));
                Assert.IsNotNull(firstWeapon.GetComponent<Button>());

                var detailsOverlay = canvas.Find("Weapon Details Overlay");
                Assert.IsNotNull(detailsOverlay);
                Assert.IsFalse(detailsOverlay.gameObject.activeSelf);

                firstWeapon.GetComponent<Button>().onClick.Invoke();
                Assert.IsTrue(detailsOverlay.gameObject.activeSelf);
                var details = RequireRect(detailsOverlay, "Weapon Details");
                Assert.AreEqual(new Vector2(580f, 720f), details.sizeDelta);
                Assert.That(details.anchoredPosition, Is.EqualTo(Vector2.zero));
                var detailsFrame = RequireRect(details, "Weapon Details Frame");
                Assert.AreEqual(new Vector2(560f, 720f), detailsFrame.sizeDelta);
                var detailsHitArea = RequireRect(details, "Weapon Details Hit Area");
                Assert.AreEqual(new Vector2(580f, 720f), detailsHitArea.sizeDelta);
                Assert.IsTrue(detailsHitArea.GetComponent<Image>().raycastTarget);
                var detailChildren = detailsOverlay.GetComponentsInChildren<Transform>(true);
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Buy Coins"));
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Buy Crystals"));
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Weapon Details Icon"));
                Assert.IsTrue(System.Array.Exists(detailChildren, child => child.name == "Weapon Details Level"));
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Close Weapon Details"));
                var name = RequireRect(details, "Weapon Details Name");
                Assert.That(name.anchorMin.x, Is.EqualTo(0.08f).Within(0.01f));
                Assert.That(name.anchorMax.x, Is.EqualTo(0.92f).Within(0.01f));
                var description = RequireRect(details, "Weapon Details Description");
                Assert.That(description.anchoredPosition.y, Is.EqualTo(-20f).Within(0.01f));
                Assert.That(description.anchorMin.x, Is.EqualTo(0.20f).Within(0.01f));
                Assert.That(description.anchorMax.x, Is.EqualTo(0.80f).Within(0.01f));
                for (var i = 0; i < 4; i++)
                {
                    var stat = RequireRect(details, i switch
                    {
                        0 => "Weapon Details Damage",
                        1 => "Weapon Details Accuracy",
                        2 => "Weapon Details Ammo",
                        _ => "Weapon Details Radius"
                    });
                    Assert.AreEqual(new Vector2(70f, 70f), RequireRect(stat, stat.name + " Icon").sizeDelta);
                }

                var rightSideWeapon = RequireRect(canvas, "Shelf Weapon 5");
                rightSideWeapon.GetComponent<Button>().onClick.Invoke();
                Assert.That(details.anchoredPosition, Is.EqualTo(Vector2.zero));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void TrainingArenaTheme_UsesDroidRigWithoutBattleRaccoonFallback()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.AreEqual("Visuals/Characters/DroidRig", theme.PlayerRigResourcePath);
            Assert.IsFalse(theme.HasPlayerSkin);
            Assert.AreEqual(1, theme.PlayerSkinFacingDirectionX);
            Assert.AreEqual(new Vector2(0f, -0.08f), theme.UnitSpriteOffset);
        }

        [Test]
        public void DesertArenaTheme_ReusesAnimatedSkyAndCharacterRigs()
        {
            var arena = System.Array.Find(
                PrototypeArenaLibrary.CreateAll(),
                item => item.ArenaId == "arena.desert-sci-fi");
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsNotNull(theme);
            Assert.AreEqual("Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground", theme.BackgroundPrefabResourcePath);
            Assert.AreEqual("Visuals/Characters/DroidRig", theme.PlayerRigResourcePath);
            Assert.AreEqual("Visuals/Characters/SigmaRig", theme.OpponentRigResourcePath);
            Assert.AreEqual(new Vector2(0.18f, 0.18f), theme.UnitSpriteScale);
            Assert.IsTrue(theme.HideProceduralTerrain);
            Assert.IsFalse(theme.UseProceduralTerrainColliders);
        }

        [Test]
        public void DesertArena_AllModularVisualAndMaskResourcesAreReadableAndPixelMatched()
        {
            var arena = System.Array.Find(
                PrototypeArenaLibrary.CreateAll(),
                item => item.ArenaId == "arena.desert-sci-fi");

            Assert.IsNotNull(arena);
            foreach (var surface in arena.SpriteSurfaces)
            {
                var visual = Resources.Load<Sprite>(surface.VisualResourcePath);
                var mask = Resources.Load<Texture2D>(surface.MaskResourcePath);
                Assert.IsNotNull(visual, surface.VisualResourcePath);
                Assert.IsNotNull(mask, surface.MaskResourcePath);
                Assert.IsTrue(visual.texture.isReadable, surface.VisualResourcePath);
                Assert.IsTrue(mask.isReadable, surface.MaskResourcePath);
                Assert.AreEqual(visual.texture.width, mask.width, surface.InstanceId);
                Assert.AreEqual(visual.texture.height, mask.height, surface.InstanceId);
                Assert.AreEqual(256f, visual.pixelsPerUnit, surface.InstanceId);
                Assert.Greater(surface.Scale, 0f, surface.InstanceId);
            }
        }

        [Test]
        public void DroidRig_PlatformAnimatesOnlyWhenPresentedPositionChanges()
        {
            var viewObject = new GameObject("Droid View Test");
            Sprite weaponSprite = null;
            try
            {
                var prefab = Resources.Load<GameObject>("Visuals/Characters/DroidRig");
                var rig = Object.Instantiate(prefab, viewObject.transform);
                var animator = rig.GetComponent<Animator>();
                var view = viewObject.AddComponent<BattleUnitView>();
                view.SetPreserveSpriteColor(true);
                view.SetVisualRoot(rig.transform);
                Assert.IsNotNull(rig.transform.Find("Body/Screen Background").GetComponent<SpriteRenderer>().sprite);

                var state = new BattleUnitState("player.droid.01", BattleTeamId.Player, Vector2.zero, 100);
                var presenter = new BattleUnitPresenter(state, view);
                presenter.Initialize(Color.white);
                view.SetHealthLabelPositionY(2f);
                Assert.IsFalse(animator.GetBool("IsMoving"));

                var healthLabel = viewObject.transform.Find("Health Label")?.GetComponent<TextMesh>();
                Assert.IsNotNull(healthLabel);
                Assert.AreEqual("100", healthLabel.text);
                Assert.AreEqual(new Color(0.06f, 1f, 0.10f, 1f), healthLabel.color);
                Assert.AreEqual(healthLabel.transform.localScale.x, healthLabel.transform.localScale.y);
                Assert.That(healthLabel.transform.localPosition.y, Is.EqualTo(2f).Within(0.0001f));
                Assert.That(healthLabel.transform.localScale.x, Is.EqualTo(0.15f).Within(0.0001f));

                weaponSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                    new Vector2(0.5f, 0.5f), 1f);
                presenter.SetEquippedWeapon(weaponSprite, false);
                presenter.SetWeaponAim(new Vector2(1f, 1f));
                var weaponMount = viewObject.transform.Find("Weapon Mount");
                var weaponRenderer = weaponMount?.GetComponent<SpriteRenderer>();
                Assert.IsNotNull(weaponRenderer);
                Assert.AreSame(weaponSprite, weaponRenderer.sprite);
                Assert.IsTrue(weaponRenderer.flipX);
                Assert.That(weaponMount.localPosition.x, Is.EqualTo(-0.68f).Within(0.0001f));
                Assert.That(weaponMount.localEulerAngles.z, Is.EqualTo(45f).Within(0.01f));
                var expectedMuzzle = weaponMount.TransformPoint(new Vector3(weaponSprite.bounds.extents.x, 0f, 0f));
                Assert.That(view.WeaponMuzzlePosition.x, Is.EqualTo(expectedMuzzle.x).Within(0.0001f));
                Assert.That(view.WeaponMuzzlePosition.y, Is.EqualTo(expectedMuzzle.y).Within(0.0001f));

                var initialRigScaleX = rig.transform.localScale.x;
                presenter.SetFacing(-1);
                Assert.AreEqual(-1, state.FacingDirectionX);
                Assert.That(rig.transform.localScale.x, Is.EqualTo(-initialRigScaleX).Within(0.0001f));
                Assert.That(weaponMount.localPosition.x, Is.EqualTo(0.68f).Within(0.0001f));

                state.MoveTo(new Vector2(0.25f, 0f));
                presenter.Refresh(true);
                Assert.IsTrue(animator.GetBool("IsMoving"));

                presenter.Refresh(true);
                Assert.IsTrue(animator.GetBool("IsMoving"));

                view.SendMessage("AdvanceMovementAnimation", 0f);
                Assert.IsTrue(animator.GetBool("IsMoving"));

                view.SendMessage("AdvanceMovementAnimation", 0.51f);

                Assert.IsFalse(animator.GetBool("IsMoving"));

                state.MoveTo(new Vector2(state.Position.x, state.Position.y + 0.25f));
                presenter.Refresh(true);
                Assert.IsFalse(animator.GetBool("IsMoving"));
            }
            finally
            {
                if (weaponSprite != null)
                {
                    Object.DestroyImmediate(weaponSprite);
                }

                Object.DestroyImmediate(viewObject);
            }
        }

        [Test]
        public void OpponentHealth_IsShownAsUnscaledRedNumberOnly()
        {
            var viewObject = new GameObject("Opponent Health Test");
            try
            {
                var view = viewObject.AddComponent<BattleUnitView>();
                var state = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, Vector2.zero, 100);
                var presenter = new BattleUnitPresenter(state, view);

                presenter.Initialize(Color.white);

                var healthLabel = viewObject.transform.Find("Health Label")?.GetComponent<TextMesh>();
                Assert.IsNotNull(healthLabel);
                Assert.AreEqual("100", healthLabel.text);
                Assert.AreEqual(new Color(1f, 0.05f, 0.03f, 1f), healthLabel.color);
                Assert.IsNull(viewObject.transform.Find("Health Bar"));
                Assert.AreEqual(healthLabel.transform.localScale.x, healthLabel.transform.localScale.y);
                Assert.That(healthLabel.transform.localPosition.y, Is.EqualTo(1.48f).Within(0.0001f));
                Assert.That(healthLabel.transform.localScale.x, Is.EqualTo(0.20f).Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(viewObject);
            }
        }

        [Test]
        public void DroidRig_MoveClipLoopsAllFourPlatformFrames()
        {
            var moveClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(
                "Assets/_Game/Art/Characters/Droid/DroidMove.anim");
            Assert.IsNotNull(moveClip);

            var bindings = AnimationUtility.GetObjectReferenceCurveBindings(moveClip);
            Assert.AreEqual(1, bindings.Length);
            Assert.AreEqual("Platform", bindings[0].path);

            var keys = AnimationUtility.GetObjectReferenceCurve(moveClip, bindings[0]);
            Assert.AreEqual(5, keys.Length);
            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual($"platform_only{i + 1}", keys[i].value.name);
            }

            Assert.AreSame(keys[0].value, keys[4].value);
            Assert.IsTrue(AnimationUtility.GetAnimationClipSettings(moveClip).loopTime);
        }

        [Test]
        public void BattleHud_UsesCompactAlignedTopBarAndBottomWeaponControls()
        {
            var root = new GameObject("Battle HUD Layout Test");
            GameObject createdEventSystem = null;
            try
            {
                root.AddComponent<BattleHudView>();
                createdEventSystem = GameObject.Find("Battle EventSystem");

                var canvas = root.transform.Find("Battle HUD Canvas");
                Assert.IsNotNull(canvas);

                var tutorialCard = RequireRect(canvas, "Tutorial Card");
                var timer = RequireRect(canvas, "Turn Timer");
                var settings = RequireRect(canvas, "Settings");
                var arsenal = RequireRect(canvas, "Arsenal");
                var moveLeft = RequireRect(canvas, "Move Left");
                var moveRight = RequireRect(canvas, "Move Right");
                var jumpLeft = RequireRect(canvas, "Jump Left");
                var jumpRight = RequireRect(canvas, "Jump Right");

                Assert.AreEqual(new Vector2(500f, 500f), tutorialCard.sizeDelta);
                Assert.AreEqual(new Vector2(246f, 180f), timer.sizeDelta);
                Assert.IsFalse(timer.GetComponent<Image>().preserveAspect);
                Assert.That(tutorialCard.anchoredPosition.y, Is.EqualTo(24f).Within(0.01f));
                Assert.That(timer.anchoredPosition.y, Is.EqualTo(0f).Within(0.01f));
                Assert.That(settings.anchoredPosition.y, Is.EqualTo(-20f).Within(0.01f));
                Assert.That(arsenal.anchoredPosition.y, Is.EqualTo(-30f).Within(0.01f));
                Assert.AreEqual(new Vector2(264f, 264f), arsenal.sizeDelta);
                Assert.AreEqual(new Vector2(202f, 202f), moveLeft.sizeDelta);
                Assert.AreEqual(new Vector2(202f, 202f), moveRight.sizeDelta);
                Assert.AreEqual(new Vector2(134f, 134f), jumpLeft.sizeDelta);
                Assert.AreEqual(new Vector2(134f, 134f), jumpRight.sizeDelta);

                var fire = RequireRect(canvas, "Fire Weapon");
                var reset = RequireRect(canvas, "Reset Weapon");
                Assert.That(fire.anchorMin.x, Is.EqualTo(0.75f).Within(0.001f));
                Assert.That(reset.anchorMin.x, Is.EqualTo(0.25f).Within(0.001f));
                Assert.That(fire.anchoredPosition.y, Is.EqualTo(135f).Within(0.01f));
                Assert.That(reset.anchoredPosition.y, Is.EqualTo(135f).Within(0.01f));
                Assert.AreEqual(new Vector2(180f, 180f), fire.sizeDelta);
                Assert.AreEqual(new Vector2(180f, 180f), reset.sizeDelta);
                var fireFrameSprite = fire.GetComponent<Image>().sprite;
                Assert.AreEqual("Battle Fire Ring", fireFrameSprite.name);
                Assert.That(fireFrameSprite.texture.GetPixel(128, 128).a, Is.EqualTo(0f).Within(0.001f));
                Assert.AreEqual("reset_btn", reset.GetComponent<Image>().sprite.texture.name);
                var fireIcon = RequireRect(fire, "Selected Weapon Icon");
                Assert.AreEqual(new Vector2(120f, 120f), fireIcon.sizeDelta);
                Assert.That(fireIcon.anchoredPosition.y, Is.EqualTo(-10f).Within(0.01f));
                Assert.AreEqual(new Vector3(0.8f, 0.8f, 1f), fireIcon.localScale);
                Assert.IsFalse(fireIcon.GetComponent<Image>().raycastTarget);

                Assert.IsNull(canvas.Find("Power Track"));
                Assert.IsNull(canvas.Find("Power Capsule"));
                Assert.IsNull(canvas.Find("Aim Guide"));
            }
            finally
            {
                Object.DestroyImmediate(root);
                if (createdEventSystem != null)
                {
                    Object.DestroyImmediate(createdEventSystem);
                }
            }
        }

        [Test]
        public void BattleHud_RopeControlsSeparateHorizontalInputAndHideWeaponReset()
        {
            var root = new GameObject("Battle HUD Rope Layout Test");
            GameObject createdEventSystem = null;
            try
            {
                var view = root.AddComponent<BattleHudView>();
                createdEventSystem = GameObject.Find("Battle EventSystem");
                var activeField = typeof(BattleHudView).GetField(
                    "_ropeControlsActive",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var layoutMethod = typeof(BattleHudView).GetMethod(
                    "ApplyMovementControlLayout",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var resetMethod = typeof(BattleHudView).GetMethod(
                    "ShouldShowWeaponReset",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Assert.IsNotNull(activeField);
                Assert.IsNotNull(layoutMethod);
                Assert.IsNotNull(resetMethod);

                activeField.SetValue(view, true);
                layoutMethod.Invoke(view, null);

                var canvas = root.transform.Find("Battle HUD Canvas");
                var moveLeft = RequireRect(canvas, "Move Left");
                var moveRight = RequireRect(canvas, "Move Right");
                var ropeUp = RequireRect(canvas, "Jump Left");
                var ropeDown = RequireRect(canvas, "Jump Right");

                Assert.That(moveLeft.anchoredPosition.y, Is.EqualTo(moveRight.anchoredPosition.y).Within(0.01f));
                Assert.That(moveRight.anchoredPosition.x - moveLeft.anchoredPosition.x, Is.EqualTo(254f).Within(0.01f));
                Assert.That(moveRight.anchoredPosition.x - moveLeft.anchoredPosition.x, Is.GreaterThan(moveLeft.sizeDelta.x));
                Assert.That(ropeUp.anchoredPosition.x, Is.EqualTo(ropeDown.anchoredPosition.x).Within(0.01f));
                Assert.That(ropeUp.anchoredPosition.y, Is.GreaterThan(ropeDown.anchoredPosition.y));
                Assert.That(ropeUp.localEulerAngles.z, Is.EqualTo(270f).Within(0.01f));
                Assert.That(ropeDown.localEulerAngles.z, Is.EqualTo(270f).Within(0.01f));
                Assert.IsFalse((bool)resetMethod.Invoke(null, new object[] { true, true }));
                Assert.IsTrue((bool)resetMethod.Invoke(null, new object[] { true, false }));
            }
            finally
            {
                Object.DestroyImmediate(root);
                if (createdEventSystem != null)
                {
                    Object.DestroyImmediate(createdEventSystem);
                }
            }
        }

        private static RectTransform RequireRect(Transform parent, string name)
        {
            var child = parent.Find(name) as RectTransform;
            Assert.IsNotNull(child, $"{name} must exist in the battle HUD.");
            return child;
        }

        private static void AssertMagentaBackgroundIsTransparent(Texture2D texture)
        {
            Assert.IsNotNull(texture);
            Assert.AreEqual(0f, texture.GetPixel(0, 0).a, $"Top-left magenta background should be transparent for {texture.name}.");
        }

        [Test]
        public void TrainingArenaTheme_UsesAnimatedBackgroundPrefabAndLargerForeground()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsNotNull(theme);
            Assert.IsFalse(theme.Background.HasSprite);
            Assert.IsTrue(theme.HasBackgroundPrefab);
            Assert.AreEqual("Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground", theme.BackgroundPrefabResourcePath);
            Assert.IsFalse(theme.Foreground.HasSprite);
            Assert.AreEqual(0, theme.AdditionalDestructibleLayers.Count);
            Assert.AreEqual(0, theme.OverlayLayers.Count);
            Assert.IsTrue(theme.CameraFrame.HasValue);
        }

        [Test]
        public void TrainingArenaCameraFrame_ShowsTheWholeReconstructedSurfaceAtEightyPercentWidth()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);
            var surface = arena.SpriteSurfaces[0];

            Assert.IsTrue(theme.CameraFrame.HasValue);
            var visual = Resources.Load<Sprite>(surface.VisualResourcePath);
            Assert.IsNotNull(visual);

            var terrainWidth = visual.texture.width / visual.pixelsPerUnit * surface.Scale;
            Assert.That(terrainWidth / theme.CameraFrame.Value.width, Is.EqualTo(0.837f).Within(0.01f));
        }

        [Test]
        public void EveryArenaTheme_ProvidesAFrameForCameraAndKillZonePlacement()
        {
            foreach (var arena in PrototypeArenaLibrary.CreateAll())
            {
                var theme = BattleVisualThemeLibrary.ForArena(arena);

                Assert.IsNotNull(theme, arena.ArenaId);
                Assert.IsTrue(theme.CameraFrame.HasValue, arena.ArenaId);
                Assert.That(theme.CameraFrame.Value.width, Is.GreaterThan(0f), arena.ArenaId);
                Assert.That(theme.CameraFrame.Value.height, Is.GreaterThan(0f), arena.ArenaId);
                Assert.IsTrue(theme.CameraFrame.Value.Contains(arena.PlayerSpawn), arena.ArenaId);
                Assert.IsTrue(theme.CameraFrame.Value.Contains(arena.OpponentSpawn), arena.ArenaId);
            }
        }

        [Test]
        public void DesertTheme_UsesSharedDownwardOffsetForTheCanyonWorld()
        {
            ArenaDefinition desert = null;
            foreach (var arena in PrototypeArenaLibrary.CreateAll())
            {
                if (arena.ArenaId == "arena.desert-sci-fi")
                {
                    desert = arena;
                    break;
                }
            }

            Assert.IsNotNull(desert);
            var theme = BattleVisualThemeLibrary.ForArena(desert);

            Assert.That(theme.WorldOffset.x, Is.Zero.Within(0.001f));
            Assert.That(theme.WorldOffset.y, Is.EqualTo(-1.225f).Within(0.001f));
            Assert.That(theme.ArenaScale.x, Is.EqualTo(1.37f).Within(0.001f));
            Assert.That(theme.ArenaScale.y, Is.EqualTo(1.37f).Within(0.001f));
            Assert.That(theme.ArenaScalePivot.y, Is.EqualTo(theme.CameraFrame.Value.yMin).Within(0.001f));
            Assert.That(theme.CameraTopPanAllowancePercent, Is.EqualTo(0.2f).Within(0.001f));
            Assert.That(theme.KillZoneOffsetY, Is.EqualTo(1.225f).Within(0.001f));
            Assert.That(
                theme.CameraFrame.Value.center.y + 1f + theme.WorldOffset.y,
                Is.EqualTo(2f).Within(0.001f));
        }

        [Test]
        public void ForestTheme_LowersArenaContentWithoutMovingItsBackground()
        {
            var forest = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.forest");
            var theme = BattleVisualThemeLibrary.ForArena(forest);

            Assert.That(theme.WorldOffset, Is.EqualTo(Vector2.zero));
            Assert.That(theme.ArenaContentOffset, Is.EqualTo(new Vector2(0f, -3.16f)));
            Assert.That(theme.CameraBottomPanAllowance, Is.EqualTo(3.2f).Within(0.001f));
        }

        [Test]
        public void ForestArena_UsesAuthoredTreeBranchOpponentSpawn()
        {
            var forest = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.forest");

            Assert.That(forest.OpponentSpawn.x, Is.EqualTo(4.8f).Within(0.001f));
            Assert.That(forest.OpponentSpawn.y, Is.EqualTo(2.5f).Within(0.001f));
        }

        [Test]
        public void RobotsAndMonumentThemes_UseTheirStaticBackgrounds()
        {
            var expectedBackgrounds = new Dictionary<string, string>
            {
                ["arena.robots"] = "Visuals/Arenas/Backgrounds/Robots/robots",
                ["arena.monument"] = "Visuals/Arenas/Backgrounds/Monument/monument"
            };

            foreach (var pair in expectedBackgrounds)
            {
                var arena = System.Array.Find(PrototypeArenaLibrary.CreateAll(), item => item.ArenaId == pair.Key);
                var theme = BattleVisualThemeLibrary.ForArena(arena);

                Assert.That(theme.Background.ResourcePath, Is.EqualTo(pair.Value), pair.Key);
                Assert.IsFalse(theme.HasBackgroundPrefab, pair.Key);
            }
        }

        [Test]
        public void BonesTheme_UsesASlightlyDimmedArenaTint()
        {
            var bones = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.bones");
            var theme = BattleVisualThemeLibrary.ForArena(bones);

            Assert.That(theme.ArenaTint.r, Is.EqualTo(0.84f).Within(0.001f));
            Assert.That(theme.ArenaTint.g, Is.EqualTo(0.84f).Within(0.001f));
            Assert.That(theme.ArenaTint.b, Is.EqualTo(0.84f).Within(0.001f));
            Assert.That(theme.ArenaTint.a, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void ForestTheme_UsesASlightlyDimmedArenaTint()
        {
            var forest = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.forest");
            var theme = BattleVisualThemeLibrary.ForArena(forest);

            Assert.That(theme.ArenaTint.r, Is.EqualTo(0.84f).Within(0.001f));
            Assert.That(theme.ArenaTint.g, Is.EqualTo(0.84f).Within(0.001f));
            Assert.That(theme.ArenaTint.b, Is.EqualTo(0.84f).Within(0.001f));
            Assert.That(theme.ArenaTint.a, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void GribTheme_DisablesKillZoneButKeepsTerrainFallBoundary()
        {
            var grib = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.grib");
            var theme = BattleVisualThemeLibrary.ForArena(grib);

            Assert.IsNotNull(theme);
            Assert.IsFalse(theme.HasKillZone);
        }

        [Test]
        public void LagunaTheme_LowersOnlyBattleContentByFiftyPixels()
        {
            var laguna = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.laguna");
            var theme = BattleVisualThemeLibrary.ForArena(laguna);

            Assert.That(theme.WorldOffset, Is.EqualTo(Vector2.zero));
            Assert.That(theme.ArenaContentOffset, Is.EqualTo(new Vector2(0f, -50f / 256f)));
        }

        [Test]
        public void MonumentTheme_LowersOnlyBattleContentByFiftyPixels()
        {
            var monument = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.monument");
            var theme = BattleVisualThemeLibrary.ForArena(monument);

            Assert.That(theme.WorldOffset, Is.EqualTo(Vector2.zero));
            Assert.That(theme.ArenaContentOffset, Is.EqualTo(new Vector2(0f, -50f / 256f)));
        }

        [Test]
        public void VinesTheme_DisablesKillZoneButKeepsTerrainFallBoundary()
        {
            var vines = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.vines");
            var theme = BattleVisualThemeLibrary.ForArena(vines);

            Assert.IsNotNull(theme);
            Assert.IsFalse(theme.HasKillZone);
        }

        [Test]
        public void RobotsTheme_LowersOnlyBattleContentByFiftyPixels()
        {
            var robots = System.Array.Find(PrototypeArenaLibrary.CreateAll(), arena => arena.ArenaId == "arena.robots");
            var theme = BattleVisualThemeLibrary.ForArena(robots);

            Assert.That(theme.WorldOffset, Is.EqualTo(Vector2.zero));
            Assert.That(theme.ArenaContentOffset, Is.EqualTo(new Vector2(0f, -50f / 256f)));
        }

        [Test]
        public void KillZoneVisual_IsTwiceItsPreviousHeight()
        {
            var field = typeof(BattleSceneController).GetField(
                "KillZoneVisualHeightScale",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.IsNotNull(field);
            Assert.That((float)field.GetRawConstantValue(), Is.EqualTo(0.9f).Within(0.001f));
        }

        [Test]
        public void ForestArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Forest/forest_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Forest/forest_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(3344));
            Assert.That(visual.texture.height, Is.EqualTo(1882));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void GribArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Grib/grib_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Grib/grib_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(3344));
            Assert.That(visual.texture.height, Is.EqualTo(1880));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void LagunaArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Laguna/laguna_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Laguna/laguna_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(4096));
            Assert.That(visual.texture.height, Is.EqualTo(2304));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void KrakenArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Kraken/kraken_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Kraken/kraken_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(4096));
            Assert.That(visual.texture.height, Is.EqualTo(2304));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void MonumentArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Monument/monument_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Monument/monument_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(4096));
            Assert.That(visual.texture.height, Is.EqualTo(2304));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void RobotsArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Robots/robots_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Robots/robots_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(4096));
            Assert.That(visual.texture.height, Is.EqualTo(2304));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void VinesArena_ReconstructedFullCanvasHasTheRuntimeVisualAndCollisionMask()
        {
            var visual = Resources.Load<Sprite>("Visuals/Arenas/Vines/vines_reconstructed_visual");
            var mask = Resources.Load<Texture2D>("Visuals/Arenas/Vines/vines_reconstructed_mask");

            Assert.IsNotNull(visual);
            Assert.IsNotNull(mask);
            Assert.That(visual.texture.width, Is.EqualTo(4096));
            Assert.That(visual.texture.height, Is.EqualTo(2304));
            Assert.That(visual.pixelsPerUnit, Is.EqualTo(256f));
            Assert.That(mask.width, Is.EqualTo(visual.texture.width));
            Assert.That(mask.height, Is.EqualTo(visual.texture.height));
            Assert.IsTrue(mask.isReadable);
        }

        [Test]
        public void ForestCamera_UsesLowerPanSpaceUntilTheFullyZoomedOutFrame()
        {
            var expandMethod = typeof(BattleSceneController).GetMethod(
                "ExpandCameraPanBounds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var clampBoundsMethod = typeof(BattleSceneController).GetMethod(
                "SelectCameraClampBounds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(expandMethod);
            Assert.IsNotNull(clampBoundsMethod);

            var frame = Rect.MinMaxRect(-15.925f, -7.61f, 15.925f, 10.31f);
            var panBounds = (Rect)expandMethod.Invoke(null, new object[] { frame, 0f, 3.2f });
            Assert.That(panBounds.yMin, Is.EqualTo(-10.81f).Within(0.001f));
            Assert.That(panBounds.yMax, Is.EqualTo(frame.yMax).Within(0.001f));

            var intermediateBounds = (Rect)clampBoundsMethod.Invoke(null, new object[]
            {
                frame, panBounds, true, 7.7f, 8.96f
            });
            var zoomOutBounds = (Rect)clampBoundsMethod.Invoke(null, new object[]
            {
                frame, panBounds, true, 8.96f, 8.96f
            });
            Assert.That(intermediateBounds.yMin, Is.EqualTo(panBounds.yMin).Within(0.001f));
            Assert.That(zoomOutBounds.yMin, Is.EqualTo(frame.yMin).Within(0.001f));
        }

        [Test]
        public void CameraFrameBottoms_AlignWithTheirArenaCanvasBottoms()
        {
            var expectedBottoms = new System.Collections.Generic.Dictionary<string, float>
            {
                ["arena.training-meadow"] = -11.265f,
                ["arena.desert-sci-fi"] = -11.25f,
                ["arena.forest"] = -7.61f,
                ["arena.city"] = -8.1f,
                ["arena.grib"] = -8.1f,
                ["arena.laguna"] = -8.1f,
                ["arena.monument"] = -8.1f,
                ["arena.kraken"] = -8.1f,
                ["arena.vines"] = -8.1f,
                ["arena.robots"] = -8.1f
            };

            foreach (var arena in PrototypeArenaLibrary.CreateAll())
            {
                var frame = BattleVisualThemeLibrary.ForArena(arena).CameraFrame;
                Assert.IsTrue(frame.HasValue, arena.ArenaId);
                Assert.That(frame.Value.yMin, Is.EqualTo(expectedBottoms[arena.ArenaId]).Within(0.001f), arena.ArenaId);
            }
        }

        [Test]
        public void ZoomedOutCamera_UsesViewportEdgesWhenClampingToArena()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "ClampCameraPositionToBounds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            var result = (Vector3)method.Invoke(null, new object[]
            {
                new Vector3(-9.5f, -6f, -10f),
                Rect.MinMaxRect(-9.5f, -6f, 9.5f, 6.8f),
                7f,
                2.1f,
                new Vector2(0f, 1.2f)
            });

            Assert.That(result.x, Is.EqualTo(0f).Within(0.001f));
            Assert.That(result.y, Is.EqualTo(1.2f).Within(0.001f));
            Assert.That(result.z, Is.EqualTo(-10f).Within(0.001f));
        }

        [Test]
        public void CameraFrameZoomOutSize_ContainsTheWholeFrame()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "CalculateCameraSizeToContainBounds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            var result = (float)method.Invoke(null, new object[]
            {
                Rect.MinMaxRect(-21f, -8.1f, 21f, 15.525f),
                16f / 9f
            });

            Assert.That(result, Is.EqualTo(11.8125f).Within(0.001f));
        }

        [Test]
        public void NarrowConfiguredFrame_ExpandsToTheActiveViewportAspect()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "ResolveCameraFrame",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            var frame = (Rect)method.Invoke(null, new object[]
            {
                Rect.MinMaxRect(-17.25f, -8.1f, 17.25f, 11.31f),
                2.12f
            });

            Assert.That(frame.yMin, Is.EqualTo(-8.1f).Within(0.001f));
            Assert.That(frame.width, Is.EqualTo(41.1492f).Within(0.001f));
            Assert.That(frame.height, Is.EqualTo(19.41f).Within(0.001f));
        }

        [Test]
        public void ArenaSpriteSeamOverdraw_ExpandsEachEdgeByTwoPixels()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "CalculateArenaSpriteSeamOverdraw",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            var scale = (Vector2)method.Invoke(null, new object[] { new Vector2(1000f, 500f) });

            Assert.That(scale.x, Is.EqualTo(1.004f).Within(0.0001f));
            Assert.That(scale.y, Is.EqualTo(1.008f).Within(0.0001f));
        }

        [Test]
        public void MovementControls_AreOwnedOnlyByThePlayerTurn()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "IsPlayerControlTurn",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            Assert.IsTrue((bool)method.Invoke(null, new object[] { BattleTeamId.Player }));
            Assert.IsFalse((bool)method.Invoke(null, new object[] { BattleTeamId.Opponent }));
        }

        [Test]
        public void FullTrajectory_AddsDotsInsteadOfIncreasingTheirSpacing()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "CalculateFullTrajectoryDotCount",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            const float arcLength = 44f;
            var dotCount = (int)method.Invoke(null, new object[] { arcLength });

            Assert.That(dotCount, Is.GreaterThan(20));
            Assert.That(arcLength / (dotCount - 1), Is.LessThanOrEqualTo(0.55f));
        }

    }
}
