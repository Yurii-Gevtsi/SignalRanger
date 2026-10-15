using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.App;
using Game.Core;
using Game.Data.Save;
using Game.Gameplay.Battle.Assembly;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Game.Tests.PlayMode
{
    /// <summary>
    /// Playtest findings checked in the production battle scene, where the presentation and the simulation run
    /// together: what the player sees in the hands, in flight and on the ground, and whether a turn comes back.
    /// </summary>
    public sealed class BattlePlaytestRegressionTests
    {
        private const BindingFlags PrivateInstance = BindingFlags.Instance | BindingFlags.NonPublic;
        private Func<string, bool> _previousUnlocks;
        private Func<string, SaveWeaponProgressData> _previousProgress;
        private string[] _previousOpponentWeapons;
        private BattleSceneController _controller;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            AppServices.Initialize();
            _previousUnlocks = WeaponProgressionBridge.UnlockedResolver;
            _previousProgress = WeaponProgressionBridge.Resolver;
            _previousOpponentWeapons = GameSessionContext.SelectedOpponentWeaponIds;
            WeaponProgressionBridge.UnlockedResolver = _ => true;
            WeaponProgressionBridge.Resolver = _ => null;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Time.timeScale = 1f;
            SetOpponentWeapons(_previousOpponentWeapons);
            WeaponProgressionBridge.UnlockedResolver = _previousUnlocks;
            WeaponProgressionBridge.Resolver = _previousProgress;
            var battle = SceneManager.GetSceneByName(GameSceneNames.Battle);
            var cleanup = SceneManager.CreateScene("PlaytestRegressionCleanup" + Guid.NewGuid().ToString("N"));
            SceneManager.SetActiveScene(cleanup);
            if (battle.IsValid() && battle.isLoaded)
                yield return SceneManager.UnloadSceneAsync(battle);
        }

        [UnityTest]
        public IEnumerator GuardianDrone_FliesWithItsOwnSkinAndLeavesTheHandsAtOnce()
        {
            yield return Load();
            Select("weapon.guardian-drone");
            Assert.That(HeldWeaponVisible(), Is.True, "held before the throw");
            GameObject body = null;
            var visuals = Get<Dictionary<int, GameObject>>("_projectileVisuals");
            Simulation.ProjectileSpawned += projectile => body = visuals[projectile.Id];
            Set("_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke("Fire");

            Assert.That(body, Is.Not.Null);
            var renderer = body.GetComponentInChildren<SpriteRenderer>();
            Assert.That(renderer.sprite, Is.Not.Null, "not the plain black ball");
            Assert.That(renderer.sprite.texture, Is.EqualTo(Resources.Load<Texture2D>("UI/Weapons/guardian_drone")));
            Assert.That(renderer.color, Is.EqualTo(Color.white));
            Assert.That(HeldWeaponVisible(), Is.False, "the hands are empty while it flies");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator AfterTheTurnsShot_TheHeldSkinAndAimStayHiddenWhenTheShellLands(
            [Values("weapon.granata", "weapon.titanium-disc", "weapon.cryo-grenade")] string weaponId)
        {
            yield return Load();
            Select(weaponId);
            Inventory.SetRemainingAmmo(Inventory.Selected, 5);
            Invoke("RefreshPlayerWeaponVisual");
            Assert.That(HeldWeaponVisible(), Is.True);
            FireAim(new Vector2(0.5f, 0.8f), 7f);
            Assert.That(HeldWeaponVisible(), Is.False, "hidden right after the shot");
            for (var tick = 0; tick < 2400 && Simulation.HasProjectilesInFlight; tick++) Simulation.Tick(1f / 120f);
            yield return null;

            Assume.That(State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player), "still the retreat of the player's turn");
            Assert.That(Get<bool>("_playerWeaponSelected"), Is.False, "nothing to aim with until the turn passes");
            Assert.That(HeldWeaponVisible(), Is.False, "the next item does not come back into the hands");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator PlacedDevices_StandOnTheGroundInsteadOfHalfSunkIntoIt(
            [Values("weapon.force-field", "weapon.grounder")] string weaponId)
        {
            yield return Load();
            Select(weaponId);
            Set("_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke("Fire");

            var device = GameObject.Find(weaponId == "weapon.force-field" ? "Force Field Device" : "Grounder Placed");
            Assert.That(device, Is.Not.Null);
            var renderer = device.GetComponent<SpriteRenderer>();
            Assert.That(renderer.sprite, Is.Not.Null);
            var grid = Get<BattleTerrain>("_battleTerrain").Grid;
            Assert.That(grid.TryFindSurfaceBelow(renderer.bounds.center.x, renderer.bounds.max.y + 1f, 5f, out var surfaceY), Is.True);
            Assert.That(renderer.bounds.min.y, Is.EqualTo(surfaceY).Within(0.12f), "the bottom edge rests on the surface");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator TerrainCrater_ReachesTheGpuTextureThroughTheRegionUpload()
        {
            yield return Load();
            var terrain = Get<BattleTerrain>("_battleTerrain");
            var grid = terrain.Grid;
            var unit = State.GetTeam(BattleTeamId.Player).Units[0];
            var point = Vector2.zero;
            for (var dx = 0.5f; dx < 8f && point == Vector2.zero; dx += 0.25f)
            {
                if (!grid.TryFindSurfaceBelow(unit.Position.x + dx, unit.Position.y + 3f, 10f, out var groundY)) continue;
                var candidate = new Vector2(unit.Position.x + dx, groundY - 0.2f);
                if (grid.IsSolidWorld(candidate) && grid.IsSolidWorld(candidate + Vector2.down * 0.2f)) point = candidate;
            }

            Assert.That(point, Is.Not.EqualTo(Vector2.zero), "a solid spot to cut");
            Assert.That(terrain.ApplyCraterAndGetDestroyedCellCount(point, 0.3f), Is.GreaterThan(0));
            terrain.FlushTextureUploads();

            var checkedVisuals = 0;
            foreach (var visual in (IList)typeof(BattleTerrain).GetField("_visuals", PrivateInstance).GetValue(terrain))
            {
                var type = visual.GetType();
                var texture = (Texture2D)type.GetProperty("Texture").GetValue(visual);
                var worldMin = (Vector2)type.GetProperty("WorldMin").GetValue(visual);
                var worldSize = (Vector2)type.GetProperty("WorldSize").GetValue(visual);
                var x = (int)type.GetProperty("TextureRectX").GetValue(visual)
                    + Mathf.FloorToInt((point.x - worldMin.x) / worldSize.x * (int)type.GetProperty("PixelWidth").GetValue(visual));
                var y = (int)type.GetProperty("TextureRectY").GetValue(visual)
                    + Mathf.FloorToInt((point.y - worldMin.y) / worldSize.y * (int)type.GetProperty("PixelHeight").GetValue(visual));
                if (texture == null || x < 0 || y < 0 || x >= texture.width || y >= texture.height) continue;

                Assert.That(ReadGpuAlpha(texture, x, y), Is.LessThan(0.05f), "the cut is on the GPU copy, not only on the CPU");
                checkedVisuals++;
            }

            Assert.That(checkedVisuals, Is.GreaterThan(0));
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator IncendiaryDrops_ReachTheGroundAtOnceWithoutRisingFirst()
        {
            yield return Load();
            Select("weapon.incendiary-grenade");
            var drops = new List<FireBlobEvent>();
            Simulation.FireBlobSpawned += drops.Add;
            FireAim(new Vector2(0.4f, 0.6f), 6f);
            for (var tick = 0; tick < 2400 && drops.Count == 0; tick++) Simulation.Tick(1f / 120f);

            Assert.That(drops.Count, Is.EqualTo(8));
            Assert.That(drops, Has.All.Matches<FireBlobEvent>(drop => drop.TravelSeconds < 0.2f), "they land at once");
            Assert.That(drops, Has.All.Matches<FireBlobEvent>(drop => drop.InitialVelocity.y <= 0f), "none rises first");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator LaserFenceHull_AGrenadeComingToRestAgainstItLeavesItStanding()
        {
            yield return Load();
            var owner = State.GetTeam(BattleTeamId.Player).Units[0];
            Select("weapon.laser-fence");
            var nodes = new List<LaserFenceNodeDeployedEvent>();
            Simulation.LaserFenceNodeDeployed += nodes.Add;
            Set("_aimState", new AimState(owner.Position, Vector2.down, 4f, true));
            Invoke("Fire");
            for (var tick = 0; tick < 600 && nodes.Count < 1; tick++) Simulation.Tick(1f / 120f);
            Assert.That(nodes.Count, Is.EqualTo(1));

            var removed = 0;
            Simulation.LaserFenceNodeRemoved += _ => removed++;
            var enemy = State.GetTeam(BattleTeamId.Opponent).Units[0];
            Simulation.Spawn(new ProjectileSpawnRequest(
                Game.Gameplay.Battle.Projectiles.ProjectileDefinition.CreateRuntime("projectile.weapon.granata", 0.1f, 1f, 30f, 60, 1.2f),
                enemy.UnitId, nodes[0].Point + new Vector2(0.05f, 0.35f), new Vector2(-0.4f, 0f), 0.2f, Color.white));
            var closest = float.PositiveInfinity;
            for (var tick = 0; tick < 360; tick++)
            {
                Simulation.Tick(1f / 120f);
                var grenade = Simulation.Projectiles.FirstOrDefault(p => p.Definition.ProjectileId == "projectile.weapon.granata");
                if (grenade != null) closest = Mathf.Min(closest, Vector2.Distance(grenade.Position, nodes[0].Point));
            }

            Assert.That(closest, Is.LessThan(0.25f), "the grenade really touched the hull");
            Assert.That(removed, Is.Zero, "a touch is not a hit");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator Bot_PlacingAMineHandsTheTurnBack()
        {
            yield return Load(enabled: true, "weapon.mine");
            Time.timeScale = 4f;
            var mines = 0;
            Simulation.MineDeployed += _ => mines++;
            Simulation.AdvanceTurnDueToNoUsableWeapon();
            Assert.That(State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Opponent));
            var start = Time.realtimeSinceStartup;
            while (State.Turn.ActiveTeam == BattleTeamId.Opponent && Time.realtimeSinceStartup - start < 25f) yield return null;

            Assert.That(mines, Is.GreaterThan(0), "the bot placed a mine");
            Assert.That(State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player), "and the turn came back");
            Assert.That(Simulation.HasProjectilesInFlight || Simulation.HasActiveWeaponEffect || Simulation.HasActiveMotion,
                Is.False, "nothing keeps the player's turn busy");
        }

        [UnityTest]
        public IEnumerator DroneFlight_LeavesTheArenaFreeForTheCamera()
        {
            yield return Load(enabled: true);
            Select("weapon.bomber");
            Set("_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke("Fire");
            yield return null;
            yield return null;
            Assert.That(Simulation.HasActiveControlledProjectile, Is.True);
            foreach (var point in new[] { new Vector2(0.5f, 0.5f), new Vector2(0.3f, 0.6f), new Vector2(0.7f, 0.4f) })
            {
                var data = new PointerEventData(EventSystem.current)
                {
                    position = new Vector2(Screen.width * point.x, Screen.height * point.y)
                };
                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(data, hits);
                Assert.That(hits, Is.Empty, "no interface covers the arena at " + point);
            }
        }

        [UnityTest]
        public IEnumerator PlacedMagnet_ShowsItsHealthAboveItAndKeepsTheTurn()
        {
            yield return Load();
            Select("weapon.grounder");
            Set("_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke("Fire");
            Assert.That(State.Turn.ActiveTeam, Is.EqualTo(BattleTeamId.Player), "setting the magnet down keeps the turn");
            var magnet = GameObject.Find("Grounder Placed").GetComponent<SpriteRenderer>();
            var label = GameObject.Find("Device Health")?.GetComponent<TextMesh>();
            Assert.That(label, Is.Not.Null, "its health is shown");
            Assert.That(label.text, Is.EqualTo(State.GetTeam(BattleTeamId.Player).Units[0].MaxHealth.ToString()));
            Assert.That(label.transform.position.y, Is.GreaterThan(magnet.bounds.max.y), "above the magnet");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator HoppingGrenade_TheAimIsNoLongerDrawnOnceItIsThrown()
        {
            yield return Load();
            Select("weapon.hopping-grenade");
            var unit = State.GetUnit(State.Turn.ActiveUnitId);
            Set("_aimState", new AimState(unit.Position, new Vector2(0.6f, 0.8f).normalized, 9f, true));
            Invoke("DrawTrajectoryPreview");
            var dots = Get<List<SpriteRenderer>>("_trajectoryDots");
            Assert.That(dots.Any(dot => dot.enabled), Is.True, "aimed before the throw");
            Invoke("Fire");
            Assert.That(Simulation.HasProjectilesInFlight, Is.True);
            Assert.That(dots.Any(dot => dot.enabled), Is.False, "the arc is gone once it flies");
            Assert.That(Simulation.CanContinueHoppingGrenade(), Is.True, "the extra jump is still there");
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator WeaponsWithoutAim_LeaveTheDragToTheCamera()
        {
            yield return Load();
            foreach (var (weaponId, ownsTheDrag) in new[]
                     { ("weapon.mine", false), ("weapon.plasma-sword", false), ("weapon.grounder", false),
                       ("weapon.granata", true), (TitaniumBarrierRules.WeaponId, true) })
            {
                Select(weaponId);
                Assert.That((bool)typeof(BattleSceneController).GetMethod("SelectedWeaponOwnsTheDrag", PrivateInstance)
                    .Invoke(_controller, null), Is.EqualTo(ownsTheDrag), weaponId);
            }
        }

        [UnityTest]
        public IEnumerator TitaniumBarrier_TheNextBeamStartsOverTheDroidAgain()
        {
            yield return Load();
            Select(TitaniumBarrierRules.WeaponId);
            Inventory.SetRemainingAmmo(Inventory.Selected, 2);
            Invoke("UpdateTitaniumBarrierPlacement");
            var first = Get<AimState>("_aimState").Origin;
            var grid = Get<BattleTerrain>("_battleTerrain").Grid;
            Set("_aimState", new AimState(new Vector2(first.x, grid.WorldMaxY - 2f), Vector2.right, 1f, true));
            Invoke("Fire");
            Assert.That(Simulation.IsAttackCommitted, Is.False);
            Invoke("UpdateTitaniumBarrierPlacement");
            var second = Get<AimState>("_aimState").Origin;
            var droid = State.GetTeam(BattleTeamId.Player).Units[0].Position;
            Assert.That(Vector2.Distance(second, droid), Is.LessThan(2.5f), "over the droid, not in the middle of the map");
            Assert.That(second, Is.Not.EqualTo(Vector2.zero));
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator SignalRocket_StandsAtTheFeetWhileTheDroidMayWalkAway()
        {
            yield return Load();
            Select("weapon.sky-hook");
            var droid = State.GetTeam(BattleTeamId.Player).Units[0];
            Set("_aimState", new AimState(Vector2.zero, Vector2.zero, 0f, false));
            Invoke("Fire");
            Assert.That(Simulation.HasProjectilesInFlight, Is.True);
            var rocket = Simulation.Projectiles[0];
            Assert.That(Mathf.Abs(rocket.Position.x - droid.Position.x), Is.LessThan(0.1f), "under the droid's feet");
            Assert.That(Simulation.IsActiveSkyHookArming, Is.True);
            Assert.That(Simulation.CanAcceptMovementInput(), Is.True, "free to step away");
            var start = rocket.Position;
            for (var tick = 0; tick < 300; tick++) Simulation.Tick(1f / 120f);
            Assert.That(rocket.Position, Is.EqualTo(start), "still standing after 2.5 s");
            for (var tick = 0; tick < 120; tick++) Simulation.Tick(1f / 120f);
            Assert.That(rocket.Position.y, Is.GreaterThan(start.y + 0.5f), "up it goes after three seconds");
            LogAssert.NoUnexpectedReceived();
        }

        private IEnumerator Load(bool enabled = false, params string[] opponentWeapons)
        {
            if (opponentWeapons != null && opponentWeapons.Length > 0) SetOpponentWeapons(opponentWeapons);
            yield return SceneManager.LoadSceneAsync(GameSceneNames.Battle, LoadSceneMode.Single);
            BattleSceneBootstrap.EnsureCreated();
            _controller = UnityEngine.Object.FindAnyObjectByType<BattleSceneController>();
            Assert.That(_controller, Is.Not.Null);
            _controller.enabled = enabled;
            Set("_firstBattleGuide", null);
            yield return null;
        }

        private static void SetOpponentWeapons(string[] ids)
        {
            typeof(GameSessionContext).GetProperty(nameof(GameSessionContext.SelectedOpponentWeaponIds))
                .SetValue(null, ids ?? Array.Empty<string>());
        }

        private static float ReadGpuAlpha(Texture2D texture, int x, int y)
        {
            var target = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(texture, target);
            var previous = RenderTexture.active;
            RenderTexture.active = target;
            var read = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            read.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
            read.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(target);
            var alpha = read.GetPixel(0, 0).a;
            UnityEngine.Object.Destroy(read);
            return alpha;
        }

        private BattleSimulation Simulation => Get<BattleSimulation>("_simulation");
        private WeaponInventoryRuntime Inventory => Get<WeaponInventoryRuntime>("_inventory");
        private BattleState State => Get<BattleState>("_battleState");

        private void Select(string weaponId)
        {
            var index = -1;
            for (var i = 0; i < Inventory.Count; i++) if (Inventory.Weapons[i].WeaponId == weaponId) index = i;
            Assert.That(index, Is.GreaterThanOrEqualTo(0), weaponId + " is in the arsenal");
            Invoke("OnHudWeaponSelected", index);
            Assert.That(Inventory.Selected.WeaponId, Is.EqualTo(weaponId));
        }

        private void FireAim(Vector2 direction, float power)
        {
            var unit = State.GetUnit(State.Turn.ActiveUnitId);
            Set("_aimState", new AimState(unit.Position, direction.normalized, power, true));
            Invoke("Fire");
        }

        private bool HeldWeaponVisible()
        {
            var presenter = typeof(BattleSceneController).GetMethod("GetPresenter", PrivateInstance)
                .Invoke(_controller, new object[] { BattleTeamId.Player });
            var view = presenter.GetType().GetField("_view", PrivateInstance).GetValue(presenter);
            var renderer = (SpriteRenderer)view.GetType().GetField("_weaponRenderer", PrivateInstance).GetValue(view);
            return renderer != null && renderer.enabled && renderer.sprite != null;
        }

        private T Get<T>(string field) =>
            (T)typeof(BattleSceneController).GetField(field, PrivateInstance).GetValue(_controller);

        private void Set(string field, object value) =>
            typeof(BattleSceneController).GetField(field, PrivateInstance).SetValue(_controller, value);

        private void Invoke(string method, params object[] args) =>
            typeof(BattleSceneController).GetMethod(method, PrivateInstance).Invoke(_controller, args);
    }
}
