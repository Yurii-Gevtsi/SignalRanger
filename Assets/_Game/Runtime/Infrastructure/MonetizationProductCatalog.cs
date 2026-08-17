using System;
using System.Collections.Generic;

namespace Game.Infrastructure
{
    public static class MonetizationProductIds
    {
        public const string StarterConveniencePack = "pack.starter_convenience";
        public const string ProgressPack = "pack.progress";
        public const string MonthlyPass = "pass.monthly";
        public const string PremiumCurrencySmall = "currency.crystals.small";
        public const string PremiumCurrencyMedium = "currency.crystals.medium";
        public const string PremiumCurrencyLarge = "currency.crystals.large";
    }

    public enum MonetizationProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }

    public readonly struct MonetizationProduct
    {
        public MonetizationProduct(string productId, MonetizationProductType type, decimal suggestedUsdPrice,
            int crystalAmount = 0)
        {
            ProductId = productId ?? string.Empty;
            Type = type;
            SuggestedUsdPrice = suggestedUsdPrice < 0m ? 0m : suggestedUsdPrice;
            CrystalAmount = crystalAmount < 0 ? 0 : crystalAmount;
        }

        public string ProductId { get; }
        public MonetizationProductType Type { get; }
        public decimal SuggestedUsdPrice { get; }
        public int CrystalAmount { get; }
    }

    public static class MonetizationProductCatalog
    {
        private static readonly IReadOnlyList<MonetizationProduct> Products = new[]
        {
            new MonetizationProduct(MonetizationProductIds.StarterConveniencePack, MonetizationProductType.Consumable, 1.99m),
            new MonetizationProduct(MonetizationProductIds.ProgressPack, MonetizationProductType.Consumable, 4.99m),
            new MonetizationProduct(MonetizationProductIds.MonthlyPass, MonetizationProductType.Subscription, 4.99m),
            new MonetizationProduct(MonetizationProductIds.PremiumCurrencySmall, MonetizationProductType.Consumable, 0.99m, 100),
            new MonetizationProduct(MonetizationProductIds.PremiumCurrencyMedium, MonetizationProductType.Consumable, 4.99m, 550),
            new MonetizationProduct(MonetizationProductIds.PremiumCurrencyLarge, MonetizationProductType.Consumable, 9.99m, 1200)
        };

        public static IReadOnlyList<MonetizationProduct> All => Products;

        public static bool TryGet(string productId, out MonetizationProduct product)
        {
            for (var i = 0; i < Products.Count; i++)
            {
                if (string.Equals(Products[i].ProductId, productId, StringComparison.Ordinal))
                {
                    product = Products[i];
                    return true;
                }
            }

            product = default;
            return false;
        }
    }
}
