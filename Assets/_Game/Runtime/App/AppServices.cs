using Game.Audio;
using Game.App.Content;
using Game.Core;
using Game.Core.Localization;
using Game.Data.Save;
using Game.Economy;
using Game.Inventory;
using Game.Infrastructure;
using Game.Progression.Missions;
using Game.Progression;
using Game.Tutorial;

namespace Game.App
{
    public static class AppServices
    {
        private static bool _pendingLoadoutReviewConsumed;
        private static IAdsService _activityAdsSource;

        public static bool IsInitialized { get; private set; }
        public static bool IsFirstLoginToday { get; private set; }
        public static ISceneLoadingService SceneLoader { get; private set; }
        public static ILocalizationService Localization { get; private set; }
        public static GameSessionService Session { get; private set; }
        public static ISaveService SaveService { get; private set; }
        public static SaveLoadResult LastSaveLoadResult { get; private set; }
        public static RuntimeGameContentCatalog Content { get; private set; }
        public static MissionDefinition[] Missions { get; private set; }
        public static CampaignProgressionService Campaign { get; private set; }
        public static TutorialService Tutorial { get; private set; }
        public static IEconomyService Economy { get; private set; }
        public static WeaponUpgradeService WeaponUpgrades { get; private set; }
        public static BattleRewardService BattleRewards { get; private set; }
        public static ActivityProgressService ActivityProgress { get; private set; }
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
            SceneLoadingBridge.LoadHandler = SceneLoader.Load;
            SaveService = new JsonFileSaveService();
            LastSaveLoadResult = SaveService.LoadOrCreate();
            Localization = new JsonLocalizationService(SaveService.Current.settings.languageCode);
            LocalizationRuntime.Service = Localization;
            Economy = new EconomyService(SaveService.Current);
            Game.Data.Progression.CommanderProgressionTable.ForceMaxLevel = SaveService.Current.settings.adminModeEnabled;
            WeaponUpgrades = new WeaponUpgradeService(SaveService.Current, Economy);
            BattleRewards = new BattleRewardService(Economy);
            ActivityProgress = new ActivityProgressService(SaveService.Current);
            IsFirstLoginToday = ActivityProgress.RecordLogin();
            var loginPeriod = SaveService.Current.activity.dailyPeriodStartUtcTicks;
            Economy.Grant(
                200,
                "daily.login",
                "day.1",
                $"daily.login.{loginPeriod}");
            ConfigureActivityProgressBridge();
            Inventory = new InventoryService(SaveService.Current);
            Audio = new NoOpAudioService(SaveService.Current);
            Analytics = new NoOpAnalyticsService();
            Ads = new NoOpAdsService();
            SubscribeActivityProgressToAds();
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
            Game.Data.Save.WeaponProgressionBridge.Resolver =
                weaponId => SaveService.Current.inventory.GetOrCreateWeaponProgress(weaponId);
            Game.Data.Save.WeaponProgressionBridge.UnlockedResolver = Inventory.IsWeaponUnlocked;
            Session = new GameSessionService(SaveService.Current, Campaign, Content);
            LastMissionCompletionResult = default;
            SaveService.Save(SaveService.Current);
            IsInitialized = true;
        }

        /// <summary>
        /// QA/debug toggle: max commander level and unlimited currency/blueprints
        /// while enabled. Non-destructive - it overrides checks rather than mutating
        /// the player's real saved balance/experience, so turning it back off
        /// restores their true progress.
        /// </summary>
        public static void SetAdminMode(bool enabled)
        {
            if (!IsInitialized)
            {
                return;
            }

            SaveService.Current.settings.adminModeEnabled = enabled;
            Game.Data.Progression.CommanderProgressionTable.ForceMaxLevel = enabled;
            SaveService.Save(SaveService.Current);
        }

        public static void SetLanguage(string languageCode)
        {
            if (!IsInitialized || Localization == null)
            {
                return;
            }

            Localization.SetLanguage(languageCode);
            SaveService.Current.settings.languageCode = Localization.LanguageCode;
            SaveService.Current.settings.hasSelectedLanguage = true;
            SaveService.Save(SaveService.Current);
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

        public static void AttachAdsService(IAdsService ads)
        {
            if (!IsInitialized || ads == null)
            {
                return;
            }

            Ads = ads;
            SubscribeActivityProgressToAds();
        }

        private static void SubscribeActivityProgressToAds()
        {
            if (_activityAdsSource != null)
            {
                _activityAdsSource.RewardedAdCompleted -= OnRewardedAdCompleted;
            }

            _activityAdsSource = Ads;
            if (_activityAdsSource != null)
            {
                _activityAdsSource.RewardedAdCompleted += OnRewardedAdCompleted;
            }
        }

        private static void ConfigureActivityProgressBridge()
        {
            Game.Core.BattleActivityBridge.ShotFired = weaponId =>
            {
                ActivityProgress.RecordShotFired(weaponId);
            };
            Game.Core.BattleActivityBridge.PlayerDamageDealt = amount =>
            {
                ActivityProgress.RecordPlayerDamage(amount);
            };
            Game.Core.BattleActivityBridge.TerrainDestroyed = cells =>
            {
                ActivityProgress.RecordTerrainDestroyed(cells);
            };
            Game.Core.BattleActivityBridge.PlayerMoved = (distance, wasJumping) =>
            {
                ActivityProgress.RecordPlayerMovement(distance, wasJumping);
            };
            Game.Core.BattleActivityBridge.ResourcesCollected = count =>
            {
                ActivityProgress.RecordResourcesCollected(count);
                SaveService.Save(SaveService.Current);
            };
            Game.Core.BattleActivityBridge.BattleFinished = (playerWon, isStoryBattle, isArenaBattle) =>
            {
                ActivityProgress.RecordBattleFinished(playerWon, isStoryBattle, isArenaBattle);
                if (playerWon && isStoryBattle)
                {
                    ActivityProgress.RecordStoryMissionCompleted();
                }
                SaveService.Save(SaveService.Current);
            };
        }

        private static void OnRewardedAdCompleted(string placementId)
        {
            ActivityProgress?.RecordRewardedAdCompleted();
            SaveService?.Save(SaveService.Current);
            Analytics?.TrackEvent($"rewarded_ad_completed.{placementId}");
        }

        private static CloudSaveSyncCoordinator _cloudSyncCoordinator;

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
            _pendingLoadoutReviewConsumed = true;
            Session?.SelectNextMission();
            Analytics?.TrackEvent("progression_reset");
        }
    }
}
