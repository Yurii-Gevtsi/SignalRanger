using System;
using Game.Infrastructure;
using NUnit.Framework;

namespace Game.Tests.EditMode.Infrastructure
{
    public sealed class MonetizationPolicyTests
    {
        [Test]
        public void ProductCatalogUsesApprovedPricesAndCrystalAmounts()
        {
            Assert.That(MonetizationProductCatalog.TryGet(MonetizationProductIds.MonthlyPass, out var pass), Is.True);
            Assert.That(pass.SuggestedUsdPrice, Is.EqualTo(4.99m));
            Assert.That(MonetizationProductCatalog.TryGet(MonetizationProductIds.PremiumCurrencyLarge, out var large), Is.True);
            Assert.That(large.SuggestedUsdPrice, Is.EqualTo(9.99m));
            Assert.That(large.CrystalAmount, Is.EqualTo(1200));
        }

        [Test]
        public void RewardedPolicy_AllowsFiveCompletions_ThenBlocksUntilUtcDayChanges()
        {
            var now = new DateTime(2026, 8, 14, 23, 50, 0, DateTimeKind.Utc);
            var policy = new RewardedAdPolicy(now);
            for (var i = 0; i < RewardedAdPolicy.MaxRewardedCompletionsPerUtcDay; i++)
                Assert.That(policy.TryClaim(RewardedAdBenefit.Generic, null, false, false, now).Allowed, Is.True);

            Assert.That(policy.TryClaim(RewardedAdBenefit.Generic, null, false, false, now).Denial,
                Is.EqualTo(RewardedAdDenial.DailyCapReached));
            Assert.That(policy.TryClaim(RewardedAdBenefit.Generic, null, false, false, now.AddMinutes(11)).Allowed, Is.True);
        }

        [Test]
        public void RewardedPolicy_ExtensionsArePveOnlyAndCappedAtTwo()
        {
            var now = DateTime.UtcNow;
            var policy = new RewardedAdPolicy(now);
            Assert.That(policy.TryClaim(RewardedAdBenefit.PveBattleExtension, "a", false, false, now).Denial,
                Is.EqualTo(RewardedAdDenial.NotPve));
            Assert.That(policy.TryClaim(RewardedAdBenefit.PveBattleExtension, "a", true, true, now).Denial,
                Is.EqualTo(RewardedAdDenial.OneUseWeaponNotEligible));
            Assert.That(policy.TryClaim(RewardedAdBenefit.PveBattleExtension, "a", true, false, now).Allowed, Is.True);
            Assert.That(policy.TryClaim(RewardedAdBenefit.PveBattleExtension, "b", true, false, now).Allowed, Is.True);
            Assert.That(policy.TryClaim(RewardedAdBenefit.PveBattleExtension, "c", true, false, now).Denial,
                Is.EqualTo(RewardedAdDenial.DailyCapReached));
        }

        [Test]
        public void RewardedPolicy_AllowsOneMultiplierPerBattle()
        {
            var now = DateTime.UtcNow;
            var policy = new RewardedAdPolicy(now);
            Assert.That(policy.TryClaim(RewardedAdBenefit.PostBattleMultiplier, "battle-1", true, false, now).Allowed, Is.True);
            Assert.That(policy.TryClaim(RewardedAdBenefit.PostBattleMultiplier, "battle-1", true, false, now).Denial,
                Is.EqualTo(RewardedAdDenial.BattleAlreadyClaimed));
            Assert.That(RewardedAdPolicy.MaxPostBattleMultiplier, Is.EqualTo(2));
        }

        [Test]
        public void PurchaseProcessor_ValidatesAndDeduplicatesTransaction()
        {
            var validator = new AcceptingValidator();
            var store = new InMemoryEntitlementStore();
            var processor = new PurchaseEntitlementProcessor(validator, store);
            var request = new ReceiptValidationRequest("player", "editor", MonetizationProductIds.ProgressPack,
                "tx-1", "opaque-receipt", DateTime.UtcNow);

            var first = processor.Process(request);
            var second = processor.Process(request);

            Assert.That(first.Success, Is.True);
            Assert.That(second.Success, Is.True);
            Assert.That(second.AlreadyProcessed, Is.True);
            Assert.That(store.GetSnapshot("player"), Has.Count.EqualTo(1));
            Assert.That(validator.Calls, Is.EqualTo(1));
        }

        private sealed class AcceptingValidator : IReceiptValidator
        {
            public int Calls { get; private set; }
            public ReceiptValidationResult Validate(ReceiptValidationRequest request)
            {
                Calls++;
                return ReceiptValidationResult.Valid(request.TransactionId);
            }
        }
    }
}
