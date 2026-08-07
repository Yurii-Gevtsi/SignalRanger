using System;

namespace Game.Infrastructure
{
    public sealed class NoOpAdsService : IAdsService
    {
        public event Action<string> RewardedAdCompleted;

        public bool IsRewardedAdReady(string placementId)
        {
            return false;
        }

        public void ShowRewardedAd(string placementId)
        {
        }

        public void ShowInterstitial(string placementId)
        {
        }
    }
}
