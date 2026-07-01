using Game.Audio;
using Game.Core;
using Game.Data.Save;
using Game.Economy;
using Game.Inventory;
using Game.Infrastructure;
using Game.Progression.Missions;
using Game.Tutorial;

namespace Game.App
{
    public static class AppServices
    {
        public static bool IsInitialized { get; private set; }
        public static ISceneLoadingService SceneLoader { get; private set; }
        public static GameSessionService Session { get; private set; }
        public static ISaveService SaveService { get; private set; }
        public static SaveLoadResult LastSaveLoadResult { get; private set; }
        public static MissionDefinition[] Missions { get; private set; }
        public static CampaignProgressionService Campaign { get; private set; }
        public static TutorialService Tutorial { get; private set; }
        public static EconomyService Economy { get; private set; }
        public static InventoryService Inventory { get; private set; }
        public static IAudioService Audio { get; private set; }
        public static IAnalyticsService Analytics { get; private set; }
        public static IAdsService Ads { get; private set; }
        public static IPurchaseService Purchases { get; private set; }
        public static IRemoteConfigService RemoteConfig { get; private set; }
        public static INotificationService Notifications { get; private set; }
        public static MissionCompletionResult LastMissionCompletionResult { get; set; }

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            SceneLoader = new UnitySceneLoadingService();
            SaveService = new JsonFileSaveService();
            LastSaveLoadResult = SaveService.LoadOrCreate();
            Economy = new EconomyService(SaveService.Current);
            Inventory = new InventoryService(SaveService.Current);
            Audio = new NoOpAudioService(SaveService.Current);
            Analytics = new NoOpAnalyticsService();
            Ads = new NoOpAdsService();
            Purchases = new NoOpPurchaseService();
            RemoteConfig = new NoOpRemoteConfigService();
            Notifications = new NoOpNotificationService();
            Missions = MvpMissionLibrary.CreateAll();
            Campaign = new CampaignProgressionService(SaveService.Current, Missions);
            Tutorial = new TutorialService(SaveService.Current, TutorialStepLibrary.CreateFirstBattleSteps());
            Game.Core.BattleTutorialBridge.Feed = new AppBattleTutorialFeed(Tutorial, SaveService);
            Game.Core.BattleFeedbackBridge.Sink = new AppBattleFeedbackSink(Audio);
            Session = new GameSessionService(SaveService.Current, Campaign);
            LastMissionCompletionResult = default;
            IsInitialized = true;
        }
    }
}
