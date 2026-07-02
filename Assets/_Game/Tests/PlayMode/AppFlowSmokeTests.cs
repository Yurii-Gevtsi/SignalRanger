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
