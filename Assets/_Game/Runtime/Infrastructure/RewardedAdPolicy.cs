using System;
using System.Collections.Generic;

namespace Game.Infrastructure
{
    public enum RewardedAdBenefit
    {
        Generic,
        PveBattleExtension,
        PostBattleMultiplier
    }

    public enum RewardedAdDenial
    {
        None,
        DailyCapReached,
        NotPve,
        OneUseWeaponNotEligible,
        BattleAlreadyClaimed,
        InvalidBattle
    }

    public readonly struct RewardedAdDecision
    {
        public RewardedAdDecision(bool allowed, RewardedAdDenial denial)
        {
            Allowed = allowed;
            Denial = denial;
        }

        public bool Allowed { get; }
        public RewardedAdDenial Denial { get; }
    }

    /// <summary>Deterministic client-side guardrail; server validation remains authoritative for purchases.</summary>
    public sealed class RewardedAdPolicy
    {
        public const int MaxRewardedCompletionsPerUtcDay = 5;
        public const int MaxPveExtensionsPerUtcDay = 2;
        public const int MaxPostBattleMultiplier = 2;

        private readonly HashSet<string> _multiplierClaims = new HashSet<string>(StringComparer.Ordinal);
        private DateTime _day;
        private int _dailyCompletions;
        private int _dailyExtensions;

        public RewardedAdPolicy(DateTime utcNow)
        {
            _day = utcNow.ToUniversalTime().Date;
        }

        public int DailyCompletions => _dailyCompletions;
        public int DailyExtensions => _dailyExtensions;

        public RewardedAdDecision TryClaim(RewardedAdBenefit benefit, string battleId, bool isPve,
            bool isOneUseSuperWeapon, DateTime utcNow)
        {
            ResetIfNewDay(utcNow);
            if (_dailyCompletions >= MaxRewardedCompletionsPerUtcDay)
                return new RewardedAdDecision(false, RewardedAdDenial.DailyCapReached);

            if (benefit == RewardedAdBenefit.PveBattleExtension)
            {
                if (!isPve) return new RewardedAdDecision(false, RewardedAdDenial.NotPve);
                if (isOneUseSuperWeapon) return new RewardedAdDecision(false, RewardedAdDenial.OneUseWeaponNotEligible);
                if (_dailyExtensions >= MaxPveExtensionsPerUtcDay)
                    return new RewardedAdDecision(false, RewardedAdDenial.DailyCapReached);
            }

            if (benefit == RewardedAdBenefit.PostBattleMultiplier)
            {
                if (string.IsNullOrWhiteSpace(battleId)) return new RewardedAdDecision(false, RewardedAdDenial.InvalidBattle);
                if (!_multiplierClaims.Add(battleId)) return new RewardedAdDecision(false, RewardedAdDenial.BattleAlreadyClaimed);
            }

            _dailyCompletions++;
            if (benefit == RewardedAdBenefit.PveBattleExtension) _dailyExtensions++;
            return new RewardedAdDecision(true, RewardedAdDenial.None);
        }

        private void ResetIfNewDay(DateTime utcNow)
        {
            var day = utcNow.ToUniversalTime().Date;
            if (day == _day) return;
            _day = day;
            _dailyCompletions = 0;
            _dailyExtensions = 0;
            _multiplierClaims.Clear();
        }
    }
}
