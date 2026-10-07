using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.App;
using Game.Core;
using Game.Data.Save;
using Game.Data.Progression;
using Game.Gameplay.Battle.Assembly;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
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

        [UnityTest]
        public IEnumerator StickyGrenade_ProductionSceneThrowsTheBaseOrPurchasedBodySkin(
            [Values(false, true)] bool purchased)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.sticky-grenade", rank = purchased ? 6 : 1 };
            progress.EnsureDefaults();
            if (purchased)
            {
                for (var i = 2; i <= 5; i++) progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    { rank = i, path = (int)(i <= 3 ? WeaponUpgradePath.Loadout : WeaponUpgradePath.Aim) });
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "toxic-release" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 1 });
            }
            WeaponProgressionBridge.Resolver = id => id == "weapon.sticky-grenade" ? progress : null;
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.sticky-grenade" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.sticky-grenade") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            var expected = Resources.Load<Texture2D>(purchased ? "UI/Weapons/sticky_grenade_5A" : "UI/Weapons/sticky_grenade");
            Assert.That(expected, Is.Not.Null);
            Texture shown = null;
            simulation.ProjectileSpawned += projectile =>
                shown = visuals[projectile.Id].GetComponentInChildren<SpriteRenderer>().sprite.texture;
            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            Set(controller, "_aimState", new AimState(new Vector2(0f, terrain.Grid.WorldMaxY - 2f), Vector2.down, 1f, true));
            Invoke(controller, "Fire");
            Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(1));
            Assert.That(shown, Is.EqualTo(expected));
            for (var frame = 0; frame < 120 && (simulation.HasProjectilesInFlight); frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.Zero, "the thrown grenade must detonate");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator TeleportSphere_ProductionSceneKeepsTheSphereBodyAndTheSecondPressTriggersIt()
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.teleport", rank = 11 };
            progress.EnsureDefaults();
            for (var rank = 2; rank <= 10; rank++)
            {
                if (rank == 6) continue;
                progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData
                    { rank = rank, path = (int)(rank % 2 == 0 ? WeaponUpgradePath.Aim : WeaponUpgradePath.Loadout) });
            }
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "instant-touch" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 1 });
            progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 10, optionId = "early-trigger" });
            progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 10, purchaseCount = 1 });
            WeaponProgressionBridge.Resolver = id => id == "weapon.teleport" ? progress : null;
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.teleport" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.teleport") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            Texture shown = null;
            simulation.ProjectileSpawned += projectile =>
                shown = visuals[projectile.Id].GetComponentInChildren<SpriteRenderer>().sprite.texture;
            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            Set(controller, "_aimState", new AimState(new Vector2(0f, terrain.Grid.WorldMaxY - 2f), Vector2.right, 12f, true));
            Invoke(controller, "Fire");
            Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(1));
            Assert.That(shown, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/teleport")),
                "the purchased feature keeps the sphere body");
            for (var tick = 0; tick < 6; tick++) simulation.Tick(1f / 120f);
            Assert.That(simulation.CanTriggerTeleportEarly(), Is.True);
            Assert.That(typeof(BattleSceneController).GetMethod("CanContinueMinePlacementInput", PrivateInstance)
                    .Invoke(controller, null), Is.EqualTo(true),
                "the Fire button stays usable for the second press");
            var spawnedBeforeSecondPress = simulation.Projectiles.Count;
            Invoke(controller, "Fire");
            Assert.That(simulation.ProjectilesInFlight, Is.Zero,
                "the second press consumed the sphere; before=" + spawnedBeforeSecondPress
                + " now=" + string.Join(",", System.Linq.Enumerable.Select(simulation.Projectiles,
                    p => p.Definition.ProjectileId + "#" + p.Id + "@" + p.Position))
                + " early=" + simulation.CanTriggerTeleportEarly());
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator ThrownGrenadeBody_ProductionSceneShowsTheWeaponBodyAndItDetonates(
            [Values("weapon.cryo-grenade:cryo_grenade", "weapon.hopping-grenade:hopping_grenade",
                "weapon.incendiary-grenade:incendiary_grenade", "weapon.missle:missle_fly", "weapon.emp:emp_shot",
                "weapon.swap-jump:teleport_5_10_A")] string pair)
        {
            var weaponId = pair.Split(':')[0];
            var body = pair.Split(':')[1];
            WeaponProgressionBridge.UnlockedResolver = id => id == weaponId || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != weaponId) index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            Texture shown = null;
            simulation.ProjectileSpawned += projectile =>
                shown = visuals[projectile.Id].GetComponentInChildren<SpriteRenderer>().sprite.texture;
            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            Set(controller, "_aimState", new AimState(new Vector2(0f, terrain.Grid.WorldMaxY - 2f), Vector2.down, 12f, true));
            Invoke(controller, "Fire");
            Assert.That(shown, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/" + body)));
            if (weaponId == "weapon.missle")
            {
                // missle_fly already contains its tail: no second exhaust layer may be added on top.
                foreach (var pair2 in visuals)
                    Assert.That(pair2.Value.transform.Find("Visual/Rocket Trail"), Is.Null);
            }
            for (var frame = 0; frame < 120 && simulation.HasProjectilesInFlight; frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.Zero);
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Grounder_ProductionScenePlacesTheMagnetSkinHandsTheTurnOverAndRemovesItAfterTwoEnemyTurns()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.grounder" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.grounder") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke(controller, "Fire");
            var placed = GameObject.Find("Grounder Placed");
            Assert.That(placed, Is.Not.Null, "the magnet is shown where it was placed");
            Assert.That(placed.GetComponent<SpriteRenderer>().sprite.texture,
                Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/grounder")));
            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.Zero, "one per battle");
            for (var turnEnd = 0; turnEnd < 4 && GameObject.Find("Grounder Placed") != null; turnEnd++)
            {
                simulation.AdvanceTurnDueToNoUsableWeapon();
                yield return null;
            }
            Assert.That(GameObject.Find("Grounder Placed"), Is.Null,
                "the magnet and its skin are gone after its two enemy turns");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator ForceField_ProductionSceneShowsTheSphereAbsorbsAnEnemyChargeAndRemovesItAfterTwoEnemyTurns()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.force-field" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.force-field") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke(controller, "Fire");
            var placed = GameObject.Find("Force Field Placed");
            Assert.That(placed, Is.Not.Null, "the sphere is shown where it was placed");
            var renderer = placed.GetComponent<SpriteRenderer>();
            Assert.That(renderer.sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/force_field_sphere")));
            Assert.That(renderer.sprite.bounds.size.x * placed.transform.localScale.x,
                Is.EqualTo(PrototypeWeaponLibrary.SharedNearbyRadius * 2f).Within(0.01f), "diameter = 2 x N0");
            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.Zero, "one per battle");

            var enemy = Get<BattleState>(controller, "_battleState").GetTeam(BattleTeamId.Opponent).Units[0];
            var spawned = false;
            simulation.ProjectileSpawned += _ => spawned = true;
            var field = placed.transform.position;
            simulation.Spawn(new ProjectileSpawnRequest(
                Game.Gameplay.Battle.Projectiles.ProjectileDefinition.CreateRuntime("projectile.weapon.impulse", 0.1f, 0f, 3f, 24, 0.45f),
                enemy.UnitId, (Vector2)field + new Vector2(3.5f, 0f), Vector2.left * 8f, 0.2f, Color.white));
            for (var tick = 0; tick < 240; tick++) simulation.Tick(1f / 120f);
            Assert.That(spawned, Is.True);
            Assert.That(simulation.ProjectilesInFlight, Is.Zero, "the charge was absorbed");
            Assert.That(renderer.color, Is.Not.EqualTo(Color.white), "the sphere flashes on absorption");

            for (var turnEnd = 0; turnEnd < 4 && GameObject.Find("Force Field Placed") != null; turnEnd++)
            {
                simulation.AdvanceTurnDueToNoUsableWeapon();
                yield return null;
            }
            Assert.That(GameObject.Find("Force Field Placed"), Is.Null, "the sphere is gone after its two enemy turns");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Boost_ProductionSceneLiftsTheDroidSteersItAndASecondPressDropsIt()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.boost" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var motors = Get<Dictionary<string, UnitMotor>>(controller, "_unitMotors");
            var state = Get<BattleState>(controller, "_battleState");
            var player = state.GetUnit(state.Turn.ActiveUnitId);
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.boost") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var startY = player.Position.y;
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke(controller, "Fire");
            Assert.That(simulation.HasPlayerBoost, Is.True);
            for (var frame = 0; frame < 12; frame++)
            {
                for (var tick = 0; tick < 20; tick++) simulation.Tick(1f / 120f);
                Invoke(controller, "RefreshUnitViews");
                yield return null;
            }
            var hoverY = player.Position.y;
            Assert.That(hoverY, Is.GreaterThan(startY + 0.3f), "lifted off");
            Assert.That(motors[player.UnitId].IsHovering, Is.True);

            // The shared rope buttons steer it: "up" arrives as -1 from the HUD.
            Set(controller, "_ropeVerticalInput", -1);
            Invoke(controller, "UpdateWalking");
            for (var tick = 0; tick < 120; tick++) simulation.Tick(1f / 120f);
            Assert.That(player.Position.y, Is.GreaterThan(hoverY + 1f), "the up button climbs");
            Set(controller, "_ropeVerticalInput", 0);
            Invoke(controller, "UpdateWalking");

            Invoke(controller, "Fire");
            Assert.That(simulation.HasPlayerBoost, Is.False, "the second press cuts the fuel");
            for (var frame = 0; frame < 60 && !motors[player.UnitId].IsStable; frame++)
            {
                for (var tick = 0; tick < 20; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(motors[player.UnitId].IsStable, Is.True, "the droid landed");
            Assert.That(player.Position.y, Is.EqualTo(startY).Within(0.3f));
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator SkyHook_ProductionSceneShowsTheUprightRocketWithItsTailAndItBlastsAtTheTop()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.sky-hook" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.sky-hook") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Texture shown = null;
            GameObject body = null;
            simulation.ProjectileSpawned += projectile =>
            {
                body = visuals[projectile.Id];
                shown = body.GetComponentInChildren<SpriteRenderer>().sprite.texture;
            };
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke(controller, "Fire");
            Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(1));
            Assert.That(shown, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/sky_hook")));
            var visual = body.transform.Find("Visual");
            Assert.That(visual, Is.Not.Null);
            Assert.That(visual.Find("Rocket Trail"), Is.Not.Null, "the shared exhaust tail is attached");
            Assert.That(Vector2.Dot(visual.up, Vector2.up), Is.GreaterThan(0.99f), "the art points up, the way it flies");
            Assert.That(visual.localScale.x, Is.EqualTo(0.049f).Within(0.001f));
            for (var tick = 0; tick < 6; tick++) simulation.Tick(1f / 120f);
            Invoke(controller, "SyncProjectileVisuals");
            Assert.That(Vector2.Dot(visual.up, Vector2.up), Is.GreaterThan(0.99f),
                "the per-frame sync keeps the nose pointing the way it flies");
            for (var frame = 0; frame < 120 && simulation.HasProjectilesInFlight; frame++)
            {
                for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.Zero, "the rocket blasted");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator SpiderMine_ProductionSceneAnimatesTheBaseOrThePurchasedBodyAndItBlasts(
            [Values(false, true)] bool purchased)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.spider-mine", rank = purchased ? 6 : 1 };
            progress.EnsureDefaults();
            if (purchased)
            {
                for (var rank = 2; rank <= 5; rank++)
                    progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData { rank = rank, path = (int)WeaponUpgradePath.Core });
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "spider-burn" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 1 });
            }
            WeaponProgressionBridge.Resolver = id => id == "weapon.spider-mine" ? progress : null;
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.spider-mine" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.spider-mine") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke(controller, "Fire");
            var spider = GameObject.Find("Mine Armed");
            Assert.That(spider, Is.Not.Null, "the crawler is shown where it was set up");
            var animator = spider.GetComponent<WeaponProjectileSpriteAnimator>();
            Assert.That(animator, Is.Not.Null, "it is animated with its four frames");
            yield return null;
            var first = spider.GetComponent<SpriteRenderer>().sprite;
            var prefix = purchased ? "spider_mine_5A_frame_" : "spider_mine_frame_";
            Assert.That(first, Is.Not.Null);
            Assert.That(first.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/" + prefix + "00")));
            var start = spider.transform.position;
            for (var frame = 0; frame < 20; frame++)
            {
                for (var tick = 0; tick < 12; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(Vector2.Distance(start, spider.transform.position), Is.GreaterThan(0.2f), "it crawls");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Bomber_ProductionSceneShowsTheCopterAndVerticalBombsAndBlastsOnTheGround()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.bomber" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.bomber") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var shown = new List<(string id, GameObject visual)>();
            simulation.ProjectileSpawned += projectile => shown.Add((projectile.Definition.ProjectileId, visuals[projectile.Id]));
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke(controller, "Fire");
            Assert.That(simulation.HasPlayerCopter, Is.True);
            var copter = shown[0].visual.transform.Find("Visual");
            Assert.That(copter.GetComponent<SpriteRenderer>().sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/bomber")));
            Assert.That(copter.localScale.x, Is.EqualTo(0.085f).Within(0.001f));

            simulation.SetControlledProjectileInput(Vector2.left);
            for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
            Invoke(controller, "SyncProjectileVisuals");
            Assert.That(copter.localScale.x, Is.LessThan(0f), "the copter mirrors when it flies left");

            Invoke(controller, "Fire");
            Assert.That(shown.Count, Is.EqualTo(2));
            var bomb = shown[1].visual.transform.Find("Visual");
            Assert.That(bomb.GetComponent<SpriteRenderer>().sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/blaster_shot")));
            Assert.That(Vector2.Dot(bomb.right, Vector2.down), Is.GreaterThan(0.99f), "the bolt stands vertically along the fall");
            for (var tick = 0; tick < 20; tick++) simulation.Tick(1f / 120f);
            Invoke(controller, "SyncProjectileVisuals");
            Assert.That(Vector2.Dot(bomb.right, Vector2.down), Is.GreaterThan(0.99f));
            for (var frame = 0; frame < 25; frame++)
            {
                for (var tick = 0; tick < 20; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(1), "the bomb blasted, the copter still flies");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Carrier_ProductionSceneAnimatesTheCopterAndDrawsAHeldMineOverIt()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.ufo-carrier" || id == "weapon.mine" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.mine") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            var minePoint = Vector2.zero;
            simulation.MineDeployed += evt => minePoint = evt.Point;
            Invoke(controller, "Fire");                                    // an own mine beside the droid
            for (var tick = 0; tick < 600 && GameObject.Find("Mine Armed") == null; tick++) simulation.Tick(1f / 120f);
            var mineVisual = GameObject.Find("Mine Armed");
            Assert.That(mineVisual, Is.Not.Null, "the thrown mine landed and is shown");
            var mineOrder = mineVisual.GetComponent<SpriteRenderer>().sortingOrder;
            index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.ufo-carrier") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            GameObject body = null;
            simulation.ProjectileSpawned += projectile => body = visuals[projectile.Id];
            Invoke(controller, "Fire");
            Assert.That(simulation.HasPlayerCopter, Is.True);
            Assert.That(body, Is.Not.Null, "copter visual spawned");
            var copter = body.transform.Find("Visual");
            Assert.That(copter, Is.Not.Null, "copter Visual child");
            Assert.That(copter.GetComponent<WeaponProjectileSpriteAnimator>(), Is.Not.Null, "the rotor frames are animated");
            Assert.That(copter.GetComponent<SpriteRenderer>().sprite, Is.Not.Null);

            // Fly down beside the mine and grab it.
            var mine = simulation.Projectiles.Count;
            for (var i = 0; i < 600 && !simulation.CopterHasCargo; i++)
            {
                var c = simulation.Projectiles[0].Position;
                var target = minePoint + Vector2.up * 0.3f;
                var delta = target - c;
                if (Vector2.Distance(c, minePoint) < 0.5f) { Invoke(controller, "Fire"); break; }
                simulation.SetControlledProjectileInput(Mathf.Abs(delta.x) > 0.15f ? new Vector2(Mathf.Sign(delta.x), 0f) : new Vector2(0f, Mathf.Sign(delta.y)));
                for (var tick = 0; tick < 4; tick++) simulation.Tick(1f / 120f);
            }
            Assert.That(simulation.CopterHasCargo, Is.True, "the carrier holds the mine; copter=" + (simulation.Projectiles.Count > 0 ? simulation.Projectiles[0].Position.ToString() : "none") + " mine=" + minePoint + " hasCopter=" + simulation.HasPlayerCopter + " projectiles=" + simulation.Projectiles.Count);
            Assert.That(mineVisual.GetComponent<SpriteRenderer>().sortingOrder, Is.GreaterThan(30), "drawn over the copter body");
            simulation.SetControlledProjectileInput(Vector2.right);
            for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
            yield return null;
            Assert.That(Vector2.Distance(mineVisual.transform.position, simulation.Projectiles[0].Position), Is.LessThan(0.5f),
                "the held mine travels with the copter");
            Invoke(controller, "Fire");
            Assert.That(simulation.CopterHasCargo, Is.False);
            Assert.That(mineVisual.GetComponent<SpriteRenderer>().sortingOrder, Is.EqualTo(mineOrder), "back on the ground layer");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator SelfDestruct_ProductionSceneMarksTheDroidWithAnIconAndKeepsTheTurn()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.self-destruct" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var state = Get<BattleState>(controller, "_battleState");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.self-destruct") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Set(controller, "_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            var owner = state.GetUnit(state.Turn.ActiveUnitId);
            Invoke(controller, "Fire");
            Assert.That(simulation.IsUnitSelfDestructing(owner.UnitId), Is.True);
            Assert.That(state.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player), "the turn is not ended by the marking");
            Assert.That(state.Turn.ActionSpent, Is.False);
            Invoke(controller, "RefreshUnitViews");
            var visible = 0;
            foreach (var renderer in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include))
                if (renderer.name == "Self Destruct Status Icon" && renderer.enabled
                    && renderer.sprite != null && renderer.sprite.texture.name == "self_destruct") visible++;
            Assert.That(visible, Is.EqualTo(1), "exactly one marker, on the marked droid");
            yield return null;
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Nuclear_ProductionSceneDropsTheBombNoseDownAfterTheEnemyTurn()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.nuclear-charge" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var state = Get<BattleState>(controller, "_battleState");
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            var enemy = state.GetTeam(BattleTeamId.Opponent).Units[0];
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.nuclear-charge") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            Assert.That(simulation.TrySetOrbitalTarget(enemy.Position), Is.True);
            GameObject body = null;
            simulation.ProjectileSpawned += projectile => body = visuals[projectile.Id];
            Invoke(controller, "Fire");
            Assert.That(state.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent), "the strike is deferred");
            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.Zero, "one use");
            simulation.AdvanceTurnDueToNoUsableWeapon();
            Assert.That(body, Is.Not.Null, "the bomb falls after the enemy turn");
            for (var tick = 0; tick < 10; tick++) simulation.Tick(1f / 120f);
            Invoke(controller, "SyncProjectileVisuals");
            var visual = body.transform.Find("Visual");
            Assert.That(visual.GetComponent<SpriteRenderer>().sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/nuclear_charge")));
            // The art's nose points towards the lower left (225 degrees); rotated, it must point straight down.
            var nose = (Vector2)(visual.rotation * new Vector3(Mathf.Cos(225f * Mathf.Deg2Rad), Mathf.Sin(225f * Mathf.Deg2Rad), 0f));
            Assert.That(Vector2.Dot(nose, Vector2.down), Is.GreaterThan(0.98f), "nose down along the fall");
            Assert.That(visual.localScale.x, Is.EqualTo(0.06f).Within(0.001f));
            for (var frame = 0; frame < 60 && simulation.HasProjectilesInFlight; frame++)
            {
                for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.Zero);
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator LaserFence_ProductionSceneDrawsSmallHullsAndALongBeamThatVanishesWithAWreckedHull()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.laser-fence" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var state = Get<BattleState>(controller, "_battleState");
            var owner = state.GetTeam(BattleTeamId.Player).Units[0];
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.laser-fence") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var nodes = new List<LaserFenceNodeDeployedEvent>();
            simulation.LaserFenceNodeDeployed += nodes.Add;
            var floor = owner.Position + new Vector2(0f, -0.5f);
            Set(controller, "_aimState", new AimState(owner.Position, Vector2.down, 4f, true));
            Invoke(controller, "Fire");
            for (var tick = 0; tick < 600 && nodes.Count < 1; tick++) simulation.Tick(1f / 120f);
            Set(controller, "_aimState", new AimState(owner.Position, new Vector2(1f, -0.35f), 6f, true));
            for (var tick = 0; tick < 600 && simulation.HasProjectilesInFlight; tick++) simulation.Tick(1f / 120f);
            Invoke(controller, "Fire");
            for (var tick = 0; tick < 900 && nodes.Count < 2; tick++) simulation.Tick(1f / 120f);
            yield return null;
            Assert.That(nodes.Count, Is.EqualTo(2), "two hulls on the ground");
            var hulls = new List<GameObject>();
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude))
                if (go.name == "Laser Fence Node") hulls.Add(go);
            Assert.That(hulls.Count, Is.EqualTo(2));
            Assert.That(hulls[0].transform.localScale.x, Is.EqualTo(0.055f).Within(0.001f), "half the former 0.11");
            var beams = new List<GameObject>();
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude))
                if (go.name == "Laser Fence Link") beams.Add(go);
            var distance = Vector2.Distance(nodes[0].Point, nodes[1].Point);
            Assert.That(beams.Count, Is.EqualTo(distance <= BattleSimulation.LaserFenceLinkRange ? 1 : 0), "the beam is drawn within the 5.4 reach");

            simulation.Spawn(new ProjectileSpawnRequest(
                Game.Gameplay.Battle.Projectiles.ProjectileDefinition.CreateRuntime("projectile.weapon.granata", 0.1f, 0f, 0.05f, 60, 1.5f),
                state.GetTeam(BattleTeamId.Opponent).Units[0].UnitId, nodes[0].Point + Vector2.up * 0.3f, Vector2.zero, 0.2f, Color.white));
            for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
            yield return null;
            var remaining = 0;
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude))
                if (go.name == "Laser Fence Node") remaining++;
            Assert.That(remaining, Is.EqualTo(1), "the wrecked hull's visual is gone");
            var beamsAfter = 0;
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude))
                if (go.name == "Laser Fence Link") beamsAfter++;
            Assert.That(beamsAfter, Is.Zero, "and its beam with it");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator ElectricHarpoon_ProductionSceneShowsTheHookSkinForTheBaseAndPurchasedBody(
            [Values(false, true)] bool purchased)
        {
            var progress = new SaveWeaponProgressData { weaponId = "weapon.electric-harpoon", rank = purchased ? 6 : 1 };
            progress.EnsureDefaults();
            if (purchased)
            {
                var paths = new[] { WeaponUpgradePath.Core, WeaponUpgradePath.Core, WeaponUpgradePath.Loadout, WeaponUpgradePath.Loadout };
                for (var rank = 2; rank <= 5; rank++)
                    progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData { rank = rank, path = (int)paths[rank - 2] });
                progress.evolutionChoices.Add(new SaveWeaponEvolutionChoiceData { rank = 5, optionId = "impact-pull" });
                progress.specialUpgrades.Add(new SaveWeaponSpecialUpgradeData { rank = 5, purchaseCount = 1 });
            }
            WeaponProgressionBridge.Resolver = id => id == "weapon.electric-harpoon" ? progress : null;
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.electric-harpoon" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.electric-harpoon") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            Set(controller, "_aimState", new AimState(new Vector2(0f, terrain.Grid.WorldMaxY - 2f), Vector2.right, 12f, true));
            Invoke(controller, "Fire");
            for (var tick = 0; tick < 12; tick++) simulation.Tick(1f / 120f);
            Invoke(controller, "UpdateElectricHarpoonVisual");
            var hook = GameObject.Find("Electric Harpoon Hook");
            Assert.That(hook, Is.Not.Null);
            var expected = Resources.Load<Texture2D>(purchased
                ? "UI/Weapons/electric_harpoon_5A_shot" : "UI/Weapons/electric_harpoon_shot");
            Assert.That(hook.GetComponent<SpriteRenderer>().sprite.texture, Is.EqualTo(expected));
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator GuardianDrone_ProductionSceneHidesAStealthDroneOnlyFromTheEnemyObserver()
        {
            yield return null;
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            var weapon = PrototypeWeaponLibrary.CreateAll().Single(w => w.WeaponId == "weapon.guardian-drone");
            bool Shown(BattleTeamId team, bool concealed, int id)
            {
                typeof(BattleSceneController).GetMethod("OnSimulationMineDeployed", PrivateInstance)
                    .Invoke(controller, new object[] { new MineDeployedEvent(id, Vector2.zero, weapon, team, concealed) });
                var visuals = Get<Dictionary<int, GameObject>>(controller, "_mineVisuals");
                return visuals[id].GetComponent<SpriteRenderer>().enabled;
            }

            Assert.That(Shown(BattleTeamId.Opponent, concealed: false, id: 9001), Is.True, "an ordinary enemy drone is visible");
            Assert.That(Shown(BattleTeamId.Opponent, concealed: true, id: 9002), Is.False, "stealth hides it from the player");
            Assert.That(Shown(BattleTeamId.Player, concealed: true, id: 9003), Is.True, "the owner always sees it");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator OrbitalRocket_ProductionSceneMarkerDraftConfirmAndDelayedStrike()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.orbital-rocket" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var state = Get<BattleState>(controller, "_battleState");
            var orbital = 0;
            while (inventory.Weapons[orbital].WeaponId != "weapon.orbital-rocket") orbital++;
            var other = 0;
            while (inventory.Weapons[other].WeaponId == "weapon.orbital-rocket") other++;
            var enemy = state.GetTeam(BattleTeamId.Opponent).Units[0];

            Invoke(controller, "OnHudWeaponSelected", orbital);
            Assert.That(simulation.TrySetOrbitalTarget(enemy.Position), Is.True);
            Invoke(controller, "ShowOrbitalTargetMarker", enemy.Position);
            var marker = GameObject.Find("Orbital Strike Marker");
            Assert.That(marker, Is.Not.Null);
            var arms = marker.GetComponentsInChildren<SpriteRenderer>();
            Assert.That(arms.Length, Is.EqualTo(2));
            foreach (var arm in arms)
            {
                Assert.That(Mathf.Abs(Mathf.DeltaAngle(arm.transform.eulerAngles.z, 45f)) < 1f
                    || Mathf.Abs(Mathf.DeltaAngle(arm.transform.eulerAngles.z, -45f)) < 1f,
                    "an X like the modal close glyph, not a plus");
                Assert.That(arm.color.r, Is.EqualTo(1f).Within(0.01f));
                Assert.That(arm.color.g, Is.LessThan(0.3f));
            }

            // A draft is dropped when another weapon is chosen.
            Invoke(controller, "OnHudWeaponSelected", other);
            Assert.That(simulation.HasOrbitalTargetMarker, Is.False);
            yield return null; // Destroy takes effect at the end of the frame
            Assert.That(GameObject.Find("Orbital Strike Marker"), Is.Null);

            // Confirming fixes the call and ends the turn; the rocket falls after the opponent's turn.
            Invoke(controller, "OnHudWeaponSelected", orbital);
            Assert.That(simulation.TrySetOrbitalTarget(enemy.Position), Is.True);
            Invoke(controller, "ShowOrbitalTargetMarker", enemy.Position);
            var healthBefore = enemy.Health;
            Invoke(controller, "Fire");
            yield return null;
            Assert.That(GameObject.Find("Orbital Strike Marker"), Is.Null, "the marker is consumed by the call");
            Assert.That(state.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
            Assert.That(simulation.ProjectilesInFlight, Is.Zero, "nothing falls yet");
            var advance = typeof(BattleSimulation).GetMethod("AdvanceTurnNow",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            advance.Invoke(simulation, new object[] { TurnAdvanceReason.InstantActionCompleted });
            Assert.That(simulation.HasProjectilesInFlight, Is.True, "the rocket falls at the end of the opponent's turn");
            for (var frame = 0; frame < 120 && simulation.HasProjectilesInFlight; frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(enemy.Health, Is.LessThan(healthBefore), "the strike hit the marked place");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator RepairJammer_ProductionSceneShowsTheNoRepairIconOnTheAffectedDroid()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.repair-jammer" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var state = Get<BattleState>(controller, "_battleState");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.repair-jammer") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var motors = Get<Dictionary<string, UnitMotor>>(controller, "_unitMotors");
            var player = state.GetTeam(BattleTeamId.Player).Units[0];
            var enemy = state.GetTeam(BattleTeamId.Opponent).Units[0];
            var next = player.Position + new Vector2(1.4f, 0f);
            enemy.MoveTo(next);
            motors[enemy.UnitId].Teleport(next);
            Assert.That(simulation.IsUnitRepairJammed(enemy.UnitId), Is.False);

            Invoke(controller, "Fire");
            for (var frame = 0; frame < 30 && !simulation.IsUnitRepairJammed(enemy.UnitId); frame++)
            {
                for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.IsUnitRepairJammed(enemy.UnitId), Is.True);
            Invoke(controller, "RefreshUnitViews");
            var icons = UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include);
            var visible = 0;
            foreach (var renderer in icons)
                if (renderer.name == "Repair Jammer Status Icon" && renderer.enabled
                    && renderer.sprite != null && renderer.sprite.texture.name == "no-repair-2") visible++;
            Assert.That(visible, Is.EqualTo(1), "exactly one no-repair icon, on the jammed droid");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Virus_ProductionSceneShowsTheVirusIconOnTheInfectedDroidOnly()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.virus" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var state = Get<BattleState>(controller, "_battleState");
            var motors = Get<Dictionary<string, UnitMotor>>(controller, "_unitMotors");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.virus") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var player = state.GetTeam(BattleTeamId.Player).Units[0];
            var enemy = state.GetTeam(BattleTeamId.Opponent).Units[0];
            var next = player.Position + new Vector2(1.4f, 0f);
            enemy.MoveTo(next);
            motors[enemy.UnitId].Teleport(next);
            Assert.That(simulation.IsUnitInfected(enemy.UnitId), Is.False);

            Invoke(controller, "Fire");
            for (var frame = 0; frame < 40 && !simulation.IsUnitInfected(enemy.UnitId); frame++)
            {
                for (var tick = 0; tick < 30; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.IsUnitInfected(enemy.UnitId), Is.True);
            Invoke(controller, "RefreshUnitViews");
            var visible = 0;
            foreach (var renderer in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include))
                if (renderer.name == "Virus Status Icon" && renderer.enabled
                    && renderer.sprite != null && renderer.sprite.texture.name == "virus") visible++;
            Assert.That(visible, Is.EqualTo(1), "exactly one virus icon, on the infected droid");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator RailRifle_ProductionSceneFiresASmallBoltAlongTheShotAndItLeavesTheArena()
        {
            WeaponProgressionBridge.UnlockedResolver = id =>
                id == "weapon.rail-rifle" || Array.IndexOf(StarterIds, id) >= 0;
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            var controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            controller.enabled = false;
            Set(controller, "_firstBattleGuide", null);
            var simulation = Get<BattleSimulation>(controller, "_simulation");
            var inventory = Get<WeaponInventoryRuntime>(controller, "_inventory");
            var index = 0;
            while (inventory.Weapons[index].WeaponId != "weapon.rail-rifle") index++;
            Invoke(controller, "OnHudWeaponSelected", index);
            var visuals = Get<Dictionary<int, GameObject>>(controller, "_projectileVisuals");
            SpriteRenderer shown = null;
            simulation.ProjectileSpawned += projectile =>
                shown = visuals[projectile.Id].GetComponentInChildren<SpriteRenderer>();
            var terrain = Get<BattleTerrain>(controller, "_battleTerrain");
            var direction = new Vector2(1f, 0.25f).normalized;
            Set(controller, "_aimState", new AimState(new Vector2(0f, terrain.Grid.WorldMaxY - 2f), direction, 1f, true));
            Invoke(controller, "Fire");
            Assert.That(simulation.ProjectilesInFlight, Is.EqualTo(1));
            Assert.That(shown.sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/blaster_shot")));
            Assert.That(simulation.Projectiles[0].Velocity.magnitude,
                Is.EqualTo(PrototypeWeaponLibrary.SharedProjectileSpeed).Within(0.001f));
            Assert.That(shown.transform.localScale.x, Is.LessThan(0.03f), "a small bolt, not a grenade-sized sprite");
            Assert.That(Mathf.DeltaAngle(shown.transform.eulerAngles.z,
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg), Is.EqualTo(0f).Within(1f));
            for (var frame = 0; frame < 60 && simulation.HasProjectilesInFlight; frame++)
            {
                for (var tick = 0; tick < 60; tick++) simulation.Tick(1f / 120f);
                yield return null;
            }
            Assert.That(simulation.ProjectilesInFlight, Is.Zero, "the bolt leaves the arena or spends its budget");
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
