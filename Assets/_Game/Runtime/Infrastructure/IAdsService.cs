using System;

namespace Game.Infrastructure
{
    public interface IAdsService
    {
        event Action<string> RewardedAdCompleted;
        bool IsRewardedAdReady(string placementId);
        void ShowRewardedAd(string placementId);
        void ShowInterstitial(string placementId);
    }
}
