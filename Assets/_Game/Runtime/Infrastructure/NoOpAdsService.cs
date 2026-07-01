namespace Game.Infrastructure
{
    public sealed class NoOpAdsService : IAdsService
    {
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
