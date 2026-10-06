using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Game.App;
using Game.Core;
using Game.Data.Save;
using Game.Data.Progression;
using Game.Gameplay.Battle.Assembly;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Game.UI.Battle;
using Game.Gameplay.Battle.Terrain;

namespace Game.Tests.PlayMode
{
    public sealed class StarterWeaponsBattleSmokeTests
    {
        private const BindingFlags PrivateInstance = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly string[] StarterIds =
        {
            "weapon.bur", "weapon.impulse", "weapon.granata", "weapon.rope",
            "weapon.plasma-sword", "weapon.ion-shotgun", "weapon.c4", "weapon.mine",
            "weapon.heal", "weapon.titanium-disc", "weapon.bur-rocket", "weapon.titanium-barrier", "weapon.mortira"
        };

        private Func<string, bool> _previousUnlocks;
        private Func<string, SaveWeaponProgressData> _previousProgress;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            AppServices.Initialize();
            _previousUnlocks = WeaponProgressionBridge.UnlockedResolver;
            _previousProgress = WeaponProgressionBridge.Resolver;
            WeaponProgressionBridge.UnlockedResolver = id => Array.IndexOf(StarterIds, id) >= 0;
            WeaponProgressionBridge.Resolver = _ => null;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            WeaponProgressionBridge.UnlockedResolver = _previousUnlocks;
            WeaponProgressionBridge.Resolver = _previousProgress;
            var battle = SceneManager.GetSceneByName(GameSceneNames.Battle);
            var cleanup = SceneManager.CreateScene("StarterWeaponsSmokeCleanup");
            SceneManager.SetActiveScene(cleanup);
            if (battle.IsValid() && battle.isLoaded)
                yield return SceneManager.UnloadSceneAsync(battle);
        }

        [UnityTest]
        public IEnumerator SelectionAndFire_CreateTheCorrectBattleEffect([ValueSource(nameof(StarterIds))] string weaponId)
        {
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            Assert.That(controller, Is.Not.Null);
            controller.enabled = false; // Deterministic ticks; no bot or device input during the assertion.
            Set(controller, "_firstBattleGuide", null);
            Assert.That(Get<object>(controller, "_hud"), Is.Not.Null, "Production HUD must be wired.");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var index = -1;
            for (var i = 0; i < inventory.Count; i++)
                if (inventory.Weapons[i].WeaponId == weaponId) index = i;
            Assert.That(index, Is.GreaterThanOrEqualTo(0));
            Invoke(controller, "OnHudWeaponSelected", index);
            Assert.That(inventory.Selected.WeaponId, Is.EqualTo(weaponId));
            Assert.That(Get<bool>(controller, "_playerWeaponSelected"), Is.True);

            var unit = simulation.State.GetUnit(simulation.State.Turn.ActiveUnitId);
            Assert.That(unit, Is.Not.Null);
            var beamCount = 0;
            var swingCount = 0;
            var chargeCount = 0;
            simulation.LinearWeaponEffectStarted += _ => beamCount++;
            simulation.SwordSwingStarted += _ => swingCount++;
            simulation.ChargeDeployed += _ => chargeCount++;
            if (weaponId == "weapon.heal") unit.ApplyDamage(unit.MaxHealth / 2, "test");
            var healthBefore = unit.Health;
            var direction = weaponId == "weapon.rope" ? Vector2.up : Vector2.down;
            var fireOrigin = weaponId == TitaniumBarrierRules.WeaponId
                ? new Vector2(0f, Get<BattleTerrain>(controller, "_battleTerrain").Grid.WorldMaxY - 2f) : unit.Position;
            Set(controller, "_aimState", new AimState(fireOrigin, direction, 1f,
                weaponId != "weapon.plasma-sword" && weaponId != "weapon.c4"
                    && weaponId != "weapon.mine" && weaponId != "weapon.heal"));
            Invoke(controller, "Fire");

            if (weaponId == TitaniumBarrierRules.WeaponId)
            {
                Assert.That(simulation.ProjectilesInFlight, Is.Zero);
                Assert.That(simulation.IsAttackCommitted, Is.True);
                var visual = GameObject.Find("Titanium Beam");
                Assert.That(visual, Is.Not.Null);
                Assert.That(visual.GetComponent<SpriteRenderer>().color, Is.EqualTo(Color.white));
                Assert.That((Vector2)visual.transform.position, Is.EqualTo(fireOrigin));
            }
            else if (weaponId == "weapon.bur") Assert.That(beamCount, Is.EqualTo(1));
            else if (weaponId == "weapon.plasma-sword") Assert.That(swingCount, Is.EqualTo(1));
            else if (weaponId == "weapon.rope") Assert.That(simulation.HasPlayerRopeTraversal, Is.True);
            else if (weaponId == "weapon.c4")
            {
                Assert.That(chargeCount, Is.EqualTo(1));
                var visuals = Get<Dictionary<int, GameObject>>(controller, "_mineVisuals");
                Assert.That(visuals.Count, Is.EqualTo(1));
                foreach (var visual in visuals.Values)
                    Assert.That(visual.GetComponent<SpriteRenderer>().sprite.texture,
                        Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/C4")));
            }
            else if (weaponId == "weapon.heal")
            {
                Assert.That(unit.Health - healthBefore, Is.EqualTo(Mathf.RoundToInt(unit.MaxHealth * 0.25f)));
                Assert.That(simulation.ProjectilesInFlight, Is.Zero);
            }
            else
            {
                var expected = weaponId == "weapon.ion-shotgun" ? 5 : 1;
                Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(expected));
                var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
                Assert.That(visuals.Count, Is.EqualTo(expected));
                foreach (var projectile in simulation.Projectiles)
                {
                    if (weaponId != "weapon.mine") Assert.That(projectile.Velocity.magnitude,
                        Is.EqualTo(PrototypeWeaponLibrary.SharedProjectileSpeed).Within(0.001f));
                    var renderer = visuals[projectile.Id].GetComponentInChildren<SpriteRenderer>();
                    Assert.That(renderer.sprite, Is.Not.Null);
                    if (weaponId == "weapon.ion-shotgun") Assert.That(renderer.color, Is.EqualTo(Color.black));
                    if (weaponId == "weapon.mine") Assert.That(renderer.sprite.texture,
                        Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/mine_off")));
                    if (weaponId == "weapon.titanium-disc")
                    {
                        Assert.That(renderer.sprite.texture,
                            Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/titanium_disc")));
                        Assert.That(renderer.GetComponent<WeaponProjectileSpinVisual>(), Is.Not.Null);
                        var initialAngle = renderer.transform.eulerAngles.z;
                        yield return null;
                        Assert.That(Mathf.Abs(Mathf.DeltaAngle(initialAngle,
                            renderer.transform.eulerAngles.z)), Is.GreaterThan(0.01f));
                    }
                    if (weaponId == "weapon.bur-rocket")
                    {
                        Assert.That(renderer.sprite.texture,
                            Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/bur_rocket")));
                        var tail = renderer.transform.Find("Rocket Trail");
                        Assert.That(tail, Is.Not.Null);
                        Assert.That(tail.GetComponent<SpriteRenderer>().sprite.texture,
                            Is.EqualTo(Resources.Load<Texture2D>("UI/Generated/missle_tail_clean")));
                    }
                }
            }

            // Run the real simulation and its subscribed scene effects across rendered frames.
            for (var frame = 0; frame < 8; frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            LogAssert.NoUnexpectedReceived();
        }

        private static readonly int[] MortarPurchases = { 0, 1, 2, 3, 4, 5 };

        [UnityTest]
        public IEnumerator Mortar_ProductionSceneCreatesAllChargesWithTheirShotSkin(
            [ValueSource(nameof(MortarPurchases))] int purchases)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.mortira", rank = 5 + purchases };
            progress.EnsureDefaults();
            for (var i = 2; i <= 5; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                { rank = i, path = (int)(i <= 3 ? WeaponUpgradePath.Loadout : WeaponUpgradePath.Aim) });
            if (purchases > 0)
            {
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "cascade-mortar" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = purchases });
            }
            WeaponProgressionBridge.Resolver = id => id == "weapon.mortira" ? progress : null;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.mortira") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            var shards = 0;
            simulation.ProjectileSpawned += projectile =>
            {
                var renderer = visuals[projectile.Id].GetComponentInChildren<SpriteRenderer>();
                Assert.That(renderer.sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/mortira_shot")));
                if (projectile.Definition.ProjectileId != "projectile.weapon.mortira") shards++;
            };
            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            Set(controller, "_aimState", new AimState(new Vector2(0f, terrain.Grid.WorldMaxY - 2f), Vector2.down, 1f, true));
            Invoke(controller, "Fire");
            Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(1));
            Assert.That(simulation.Projectiles[0].Velocity.magnitude,
                Is.EqualTo(PrototypeWeaponLibrary.SharedProjectileSpeed).Within(0.001f));
            for (var frame = 0; frame < 30 && shards == 0; frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(shards, Is.EqualTo(purchases == 0 ? 2 : purchases + 1));
            // The whole cascade must resolve and hand the turn over, not merely appear.
            var turnAdvanced = false;
            simulation.TurnAdvanced += (_, _) => turnAdvanced = true;
            for (var frame = 0; frame < 120 && (simulation.HasProjectilesInFlight || !turnAdvanced); frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.Zero, "every shard must detonate or expire");
            Assert.That(turnAdvanced, Is.True, "the turn ends after the last shard resolves");
            LogAssert.NoUnexpectedReceived();
        }

        private static readonly int[] BarrierModes = { -1, 0, 4 };

        [UnityTest]
        public IEnumerator TitaniumBarrier_TwoTouchesResizeAndFireCommitsTheVisibleShape(
            [ValueSource(nameof(BarrierModes))] int radiusPurchases)
        {
            if (radiusPurchases >= 0)
            {
                var progress = new SaveWeaponProgressData { weaponId = TitaniumBarrierRules.WeaponId, rank = 6 + radiusPurchases };
                progress.EnsureDefaults();
                for (var i = 0; i < 4; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    { rank = i + 2, path = (int)WeaponUpgradePath.Loadout });
                for (var i = 0; i < radiusPurchases; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    { rank = i + 7, path = (int)WeaponUpgradePath.Radius });
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData
                    { rank = 5, optionId = TitaniumBarrierRules.SizeFeatureId });
                WeaponProgressionBridge.Resolver = id => id == TitaniumBarrierRules.WeaponId ? progress : null;
                yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            }
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != TitaniumBarrierRules.WeaponId) index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Invoke(controller, "RenderHud");
            Invoke(controller, "UpdateTitaniumBarrierPlacement");
            var preview = Get<GameObject>(controller, "_barrierPlacementPreview");
            var renderer = preview.GetComponent<SpriteRenderer>();
            var hud = Get<object>(controller, "_hud");
            var icon = (Image)typeof(BattleHudView).GetField("_fireIcon", PrivateInstance).GetValue(hud);
            Assert.That(renderer.sprite.texture, Is.EqualTo(icon.sprite.texture));
            Assert.That(renderer.color, Is.EqualTo(Color.white));
            var camera = Get<Camera>(controller, "_camera");
            var baseCameraSize = camera.orthographicSize;
            var originalAngle = preview.transform.eulerAngles.z;
            var touch = InputSystem.AddDevice<Touchscreen>();
            try
            {
                SendBarrierTouches(touch, 100f, UnityEngine.InputSystem.TouchPhase.Began);
                Invoke(controller, "UpdateCameraZoom");
                SendBarrierTouches(touch, 200f, UnityEngine.InputSystem.TouchPhase.Moved);
                Invoke(controller, "UpdateCameraZoom");
                Invoke(controller, "UpdateTitaniumBarrierPlacement");
                var maximum = radiusPurchases < 0 ? 1f : 1.1f + radiusPurchases * 0.1f;
                Assert.That(Get<float>(controller, "_barrierPlacementScale"), Is.EqualTo(maximum).Within(0.001f));
                SendBarrierTouches(touch, 100f, UnityEngine.InputSystem.TouchPhase.Moved);
                Invoke(controller, "UpdateCameraZoom");
                Invoke(controller, "UpdateTitaniumBarrierPlacement");
                Assert.That(Get<float>(controller, "_barrierPlacementScale"), Is.EqualTo(maximum * 0.5f).Within(0.001f));
                SendBarrierTouches(touch, 300f, UnityEngine.InputSystem.TouchPhase.Moved);
                Invoke(controller, "UpdateCameraZoom");
                Invoke(controller, "UpdateTitaniumBarrierPlacement");
                Assert.That(Get<float>(controller, "_barrierPlacementScale"), Is.EqualTo(maximum).Within(0.001f));
                Assert.That(camera.orthographicSize, Is.EqualTo(baseCameraSize));
                Assert.That(preview.transform.eulerAngles.z, Is.EqualTo(originalAngle));
                SendBarrierTouches(touch, 300f, UnityEngine.InputSystem.TouchPhase.Ended);
            }
            finally { InputSystem.RemoveDevice(touch); }

            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            var center = new Vector2(0f, terrain.Grid.WorldMaxY - 2f);
            Invoke(controller, "SetTitaniumBarrierPlacement", center, Vector2.right);
            var previewSize = renderer.bounds.size;
            var selectedSize = simulation.TitaniumBarrierBaseSize * Get<float>(controller, "_barrierPlacementScale");
            Assert.That(previewSize.x, Is.EqualTo(selectedSize.x).Within(0.001f));
            Assert.That(previewSize.y, Is.EqualTo(selectedSize.y).Within(0.001f));
            camera.orthographicSize *= 0.75f;
            Invoke(controller, "DrawTitaniumBarrierPlacementPreview", center);
            Assert.That(renderer.bounds.size, Is.EqualTo(previewSize), "zoom must not change physical cover dimensions");
            var stockBefore = inventory.GetSelectedRemainingAmmo();
            Assert.That(stockBefore, Is.EqualTo(radiusPurchases < 0 ? 2 : 6));
            var fire = (Button)typeof(BattleHudView).GetField("_fireButton", PrivateInstance).GetValue(hud);
            fire.onClick.Invoke();
            var placed = GameObject.Find("Titanium Beam");
            Assert.That(placed, Is.Not.Null);
            var placedRenderer = placed.GetComponent<SpriteRenderer>();
            Assert.That(placedRenderer.bounds.size.x, Is.EqualTo(previewSize.x).Within(0.001f));
            Assert.That(placedRenderer.bounds.size.y, Is.EqualTo(previewSize.y).Within(0.001f));
            Assert.That(placedRenderer.color, Is.EqualTo(Color.white));
            Assert.That((Vector2)placed.transform.position, Is.EqualTo(center));
            Assert.That(preview.activeSelf, Is.False);
            Assert.That(simulation.ProjectilesInFlight, Is.Zero);
            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.EqualTo(stockBefore - 1));
            Assert.That(simulation.IsAttackCommitted, Is.True);
            Assert.That(terrain.Grid.IsSolidWorld(center), Is.True);
            terrain.RefreshMovingSpriteSurfaces();
            Assert.That(terrain.Grid.IsSolidWorld(center), Is.True);
            for (var i = 0; i < 300; i++) simulation.Tick(1f / 120f);
            Assert.That((Vector2)placed.transform.position, Is.EqualTo(center));
            LogAssert.NoUnexpectedReceived();
        }

        private static void SendBarrierTouches(Touchscreen device, float distance, UnityEngine.InputSystem.TouchPhase phase)
        {
            InputSystem.QueueStateEvent(device, new TouchState { touchId = 1, phase = phase,
                position = new Vector2(400f - distance * 0.5f, 300f) });
            InputSystem.QueueStateEvent(device, new TouchState { touchId = 2, phase = phase,
                position = new Vector2(400f + distance * 0.5f, 300f) });
            InputSystem.Update();
        }

        private static T Get<T>(object target, string field) =>
            (T)typeof(BattleSceneController).GetField(field, PrivateInstance).GetValue(target);
        private static void Set(object target, string field, object value) =>
            typeof(BattleSceneController).GetField(field, PrivateInstance).SetValue(target, value);
        private static void Invoke(object target, string method, params object[] args) =>
            typeof(BattleSceneController).GetMethod(method, PrivateInstance).Invoke(target, args);
    }
}
