using Game.Infrastructure;
using NUnit.Framework;

namespace Game.Tests.EditMode.Infrastructure
{
    public sealed class NoOpInfrastructureServiceTests
    {
        [Test]
        public void RemoteConfig_ReturnsFallbackValues()
        {
            var service = new NoOpRemoteConfigService();

            Assert.That(service.GetBool("feature", true), Is.True);
            Assert.That(service.GetInt("value", 7), Is.EqualTo(7));
            Assert.That(service.GetFloat("scale", 1.25f), Is.EqualTo(1.25f));
            Assert.That(service.GetString("text", "fallback"), Is.EqualTo("fallback"));
        }

        [Test]
        public void Ads_AreNotReadyInNoOpBuild()
        {
            var service = new NoOpAdsService();

            Assert.That(service.IsRewardedAdReady("reward.default"), Is.False);
        }

        [Test]
        public void Purchases_AreUnavailableInNoOpBuild()
        {
            var service = new NoOpPurchaseService();

            var result = service.Purchase("coins.small");

            Assert.That(service.IsAvailable, Is.False);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ProductId, Is.EqualTo("coins.small"));
        }
    }
}
