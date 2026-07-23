using Game.Audio;
using Game.App.Content;
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
        private static bool _firstSessionAutoStartConsumed;
        private static bool _pendingLoadoutReviewConsumed;

        public static bool IsInitialized { get; private set; }
        public static ISceneLoadingService SceneLoader { get; private set; }
        public static GameSessionService Session { get; private set; }
        public static ISaveService SaveService { get; private set; }
        public static SaveLoadResult LastSaveLoadResult { get; private set; }
        public static RuntimeGameContentCatalog Content { get; private set; }
        public static MissionDefinition[] Missions { get; private set; }
        public static CampaignProgressionService Campaign { get; private set; }
        public static TutorialService Tutorial { get; private set; }
        public static IEconomyService Economy { get; private set; }
        public static InventoryService Inventory { get; private set; }
        public static IAudioService Audio { get; private set; }
        public static IAnalyticsService Analytics { get; private set; }
        public static IAdsService Ads { get; private set; }
        public static IPurchaseService Purchases { get; private set; }
        public static IRemoteConfigService RemoteConfig { get; private set; }
        public static INotificationService Notifications { get; private set; }
        public static IAuthService Auth { get; private set; }
        public static ICloudSyncService CloudSync { get; private set; }
        public static IServerTimeService ServerTime { get; private set; }
        public static IPremiumStatusService PremiumStatus { get; private set; }
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
            Auth = new NoOpAuthService(SaveService.Current);
            CloudSync = new NoOpCloudSyncService();
            ServerTime = new NoOpServerTimeService();
            PremiumStatus = new NoOpPremiumStatusService();
            Content = DefaultGameContentCatalog.Create();
            Missions = Content.Missions;
            Campaign = new CampaignProgressionService(SaveService.Current, Missions, Economy);
            Tutorial = new TutorialService(SaveService.Current, TutorialStepLibrary.CreateCampaignSteps());
            Game.Core.BattleTutorialBridge.Feed = new AppBattleTutorialFeed(Tutorial, SaveService);
            Game.Core.BattleFeedbackBridge.Sink = new AppBattleFeedbackSink(Audio);
            Game.Core.Battle.BattleHudFactoryBridge.Factory = new Game.UI.Battle.BattleHudViewFactory();
            Game.Core.Battle.BattleSettingsBridge.OpenHandler =
                (onClosed, onRestart, onMenu) => SettingsScreenController.Open(onClosed, onRestart, onMenu);
            Session = new GameSessionService(SaveService.Current, Campaign, Content);
            LastMissionCompletionResult = default;
            IsInitialized = true;
        }

        /// <summary>
        /// Викликається онлайн-бутстрапом після успішного входу: замінює no-op
        /// реалізації на живі адаптери і запускає зведення сейву з хмарою.
        /// </summary>
        public static void AttachOnlineServices(
            IAuthService auth,
            ICloudSyncService cloudSync,
            IRemoteConfigService remoteConfig)
        {
            if (!IsInitialized)
            {
                return;
            }

            if (auth != null)
            {
                Auth = auth;
            }

            if (remoteConfig != null)
            {
                RemoteConfig = remoteConfig;
            }

            if (cloudSync != null)
            {
                CloudSync = cloudSync;
                _cloudSyncCoordinator?.Dispose();
                _cloudSyncCoordinator = new CloudSaveSyncCoordinator(
                    SaveService,
                    cloudSync,
                    LastSaveLoadResult.Status == SaveLoadStatus.CreatedNew);
                _cloudSyncCoordinator.OnSignedIn(decision =>
                    Analytics?.TrackEvent($"cloud_sync_resolved.{decision}"));
            }
        }

        private static CloudSaveSyncCoordinator _cloudSyncCoordinator;

        public static bool TryConsumeFirstSessionAutoStart()
        {
            if (_firstSessionAutoStartConsumed)
            {
                return false;
            }

            var saveData = SaveService?.Current;
            if (LastSaveLoadResult.Status != SaveLoadStatus.CreatedNew
                || saveData?.campaign == null
                || saveData.tutorial == null)
            {
                return false;
            }

            var shouldAutoStart = saveData.campaign.completedMissionIds.Count == 0
                && saveData.campaign.highestUnlockedMissionIndex == 0
                && saveData.tutorial.completedStepIds.Count == 0;
            if (!shouldAutoStart)
            {
                return false;
            }

            _firstSessionAutoStartConsumed = true;
            return true;
        }

        public static bool TryConsumePendingLoadoutReview()
        {
            if (_pendingLoadoutReviewConsumed)
            {
                return false;
            }

            if (!LastMissionCompletionResult.Applied || !LastMissionCompletionResult.HasUnlock)
            {
                return false;
            }

            _pendingLoadoutReviewConsumed = true;
            return true;
        }

        public static void MarkPendingLoadoutReview(bool pending)
        {
            _pendingLoadoutReviewConsumed = !pending;
        }

        public static void ResetProgression()
        {
            var saveData = SaveService?.Current;
            if (saveData == null)
            {
                return;
            }

            saveData.ResetProgression();
            SaveService.Save(saveData);
            LastMissionCompletionResult = default;
            _firstSessionAutoStartConsumed = true;
            _pendingLoadoutReviewConsumed = true;
            Session?.SelectNextMission();
            Analytics?.TrackEvent("progression_reset");
        }
    }
}
