using System;
using Game.App;
using Game.Infrastructure;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Game.Ads
{
    public sealed class GoogleMobileAdsService : IAdsService
    {
        private const string RewardedPlacement = "reward.default";
        private const string ProductionRewardedAdUnitId = "ca-app-pub-7156983977329719/2022971178";
        private const string TestRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

        private RewardedAd _rewardedAd;
        private bool _initialized;

        public event Action<string> RewardedAdCompleted;

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            MobileAds.Initialize(_ =>
            {
                Debug.Log("[AdMob] SDK initialized.");
                LoadRewardedAd();
            });
        }

        public bool IsRewardedAdReady(string placementId)
        {
            return placementId == RewardedPlacement && _rewardedAd != null && _rewardedAd.CanShowAd();
        }

        public void ShowRewardedAd(string placementId)
        {
            if (!IsRewardedAdReady(placementId))
            {
                Debug.Log("[AdMob] Rewarded ad is not ready.");
                LoadRewardedAd();
                return;
            }

            _rewardedAd.Show(_ =>
            {
                Debug.Log("[AdMob] Rewarded ad completed.");
                RewardedAdCompleted?.Invoke(placementId);
            });
        }

        public void ShowInterstitial(string placementId)
        {
            Debug.Log("[AdMob] Interstitial is not configured yet.");
        }

        private void LoadRewardedAd()
        {
            var adUnitId = Debug.isDebugBuild ? TestRewardedAdUnitId : ProductionRewardedAdUnitId;
            _rewardedAd = null;

            RewardedAd.Load(adUnitId, new AdRequest(), (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogWarning($"[AdMob] Rewarded load failed: {error}");
                    return;
                }

                _rewardedAd = ad;
                Debug.Log($"[AdMob] Rewarded ad loaded ({(Debug.isDebugBuild ? "test" : "production")}).");
                ad.OnAdFullScreenContentClosed += LoadRewardedAd;
                ad.OnAdFullScreenContentFailed += _ => LoadRewardedAd();
            });
        }
    }
}
