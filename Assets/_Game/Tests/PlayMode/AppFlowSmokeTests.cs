using System.Collections;
using Game.App;
using Game.Core;
using Game.Core.Battle;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Game.Tests.PlayMode
{
    /// <summary>
    /// Phase 13.2 smoke tests: confirm the app composition root wires services and that
    /// the boot pipeline reaches the menu without touching real infrastructure.
    /// These are intentionally shallow — they catch broken bootstrap wiring, not gameplay.
    /// </summary>
    public sealed class AppFlowSmokeTests
    {
        [Test]
        public void Initialize_RebuildsDestroyedRuntimeContent()
        {
            AppServices.Initialize();
            var previousCampaign = AppServices.Campaign;
            Object.DestroyImmediate(AppServices.Missions[0]);

            AppServices.Initialize();

            Assert.That(AppServices.Campaign, Is.Not.SameAs(previousCampaign));
            foreach (var mission in AppServices.Missions)
            {
                Assert.That(mission != null, Is.True, "Runtime missions must survive as live Unity objects.");
            }
            foreach (var weapon in AppServices.Content.Weapons)
            {
                Assert.That(weapon != null, Is.True);
            }
            foreach (var arena in AppServices.Content.Arenas)
            {
                Assert.That(arena != null, Is.True);
            }
            AppServices.Session.SelectMission(0);
            Assert.That(AppServices.Session.SelectedMission != null, Is.True);
            Assert.That(GameSessionContext.SelectedMissionId, Is.EqualTo("mission.training.01"));

            var save = AppServices.SaveService.Current;
            var adminBefore = save.settings.adminModeEnabled;
            var progressBefore = save.campaign.highestUnlockedMissionIndex;
            try
            {
                save.settings.adminModeEnabled = true;
                var last = AppServices.Missions.Length - 1;
                Assert.That(AppServices.Campaign.IsUnlocked(last), Is.True);
                AppServices.Session.SelectMission(last);
                Assert.That(AppServices.Session.SelectedMission != null, Is.True);
                Assert.That(GameSessionContext.SelectedMissionId, Is.EqualTo(AppServices.Missions[last].MissionId));
                Assert.That(save.campaign.highestUnlockedMissionIndex, Is.EqualTo(progressBefore));
            }
            finally
            {
                save.settings.adminModeEnabled = adminBefore;
                AppServices.Session.SelectNextMission();
            }

            var rebuiltCampaign = AppServices.Campaign;
            AppServices.Initialize();
            Assert.That(AppServices.Campaign, Is.SameAs(rebuiltCampaign));
        }

        [Test]
        public void Initialize_WiresCoreServicesAndBridges()
        {
            AppServices.Initialize();

            Assert.That(AppServices.IsInitialized, Is.True);
            Assert.That(AppServices.SceneLoader, Is.Not.Null);
            Assert.That(AppServices.SaveService?.Current, Is.Not.Null);
            Assert.That(AppServices.Campaign, Is.Not.Null);
            Assert.That(AppServices.Session, Is.Not.Null);
            Assert.That(AppServices.Missions, Is.Not.Empty);
            Assert.That(AppServices.Inventory?.UnlockedWeaponIds, Is.Not.Empty);
            Assert.That(AppServices.Audio, Is.Not.Null);

            Assert.That(BattleTutorialBridge.Feed, Is.Not.Null, "Tutorial feed must be registered.");
            Assert.That(BattleFeedbackBridge.Sink, Is.Not.Null, "Feedback sink must be registered.");
            Assert.That(BattleHudFactoryBridge.Factory, Is.Not.Null, "HUD factory must be registered.");
        }

        [Test]
        public void Session_HasSelectedMissionAfterInitialize()
        {
            AppServices.Initialize();

            var mission = AppServices.Session.SelectedMission;

            Assert.That(mission, Is.Not.Null);
            Assert.That(mission.MissionId, Is.Not.Empty);
            Assert.That(GameSessionContext.SelectedMissionId, Is.EqualTo(mission.MissionId));
        }

        [UnityTest]
        public IEnumerator BootScene_LoadsAndSpawnsBootstrapper()
        {
            var load = SceneManager.LoadSceneAsync(GameSceneNames.Boot, LoadSceneMode.Single);
            Assert.That(load, Is.Not.Null, "BootScene must be in Build Settings.");
            yield return load;

            // Bootstrapper is created via RuntimeInitializeOnLoadMethod; give one frame.
            yield return null;

            var bootstrapper = Object.FindAnyObjectByType<GameBootstrapper>();
            Assert.That(bootstrapper, Is.Not.Null, "GameBootstrapper should be alive after boot.");
            Assert.That(AppServices.IsInitialized, Is.True);
        }
    }
}
