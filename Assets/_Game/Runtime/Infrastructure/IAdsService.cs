namespace Game.Infrastructure
{
    public interface IAdsService
    {
        bool IsRewardedAdReady(string placementId);
        void ShowRewardedAd(string placementId);
        void ShowInterstitial(string placementId);
    }
}
