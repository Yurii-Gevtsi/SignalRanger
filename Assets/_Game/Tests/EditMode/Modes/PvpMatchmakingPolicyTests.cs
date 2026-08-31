using System.Collections.Generic;
using Game.Core.Modes;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class PvpMatchmakingPolicyTests
    {
        [Test]
        public void TheBandOpensTightAndWidensOnce()
        {
            Assert.That(PvpMatchmakingPolicy.ResolveBandFraction(0f),
                Is.EqualTo(PvpMatchmakingPolicy.InitialBandFraction));
            Assert.That(PvpMatchmakingPolicy.ResolveBandFraction(19.9f),
                Is.EqualTo(PvpMatchmakingPolicy.InitialBandFraction));
            Assert.That(PvpMatchmakingPolicy.ResolveBandFraction(20f),
                Is.EqualTo(PvpMatchmakingPolicy.WidenedBandFraction));
            Assert.That(PvpMatchmakingPolicy.ResolveBandFraction(44f),
                Is.EqualTo(PvpMatchmakingPolicy.WidenedBandFraction));
        }

        [Test]
        public void TheQueueOnlyFillsItselfAfterTheAuthoredWait()
        {
            Assert.That(PvpMatchmakingPolicy.ShouldBackfill(44.9f), Is.False);
            Assert.That(PvpMatchmakingPolicy.ShouldBackfill(45f), Is.True);
            Assert.That(PvpMatchmakingPolicy.BackfillAfterSeconds, Is.EqualTo(45f));
        }

        [Test]
        public void TheOpeningBandIsTenPercentOfHistoricalPower()
        {
            Assert.That(PvpMatchmakingPolicy.IsInBand(1000, 900, 0f), Is.True);
            Assert.That(PvpMatchmakingPolicy.IsInBand(1000, 1100, 0f), Is.True);
            Assert.That(PvpMatchmakingPolicy.IsInBand(1000, 899, 0f), Is.False);
            Assert.That(PvpMatchmakingPolicy.IsInBand(1000, 1101, 0f), Is.False);
        }

        [Test]
        public void TheWidenedBandAcceptsWhatTheOpeningOneRejected()
        {
            Assert.That(PvpMatchmakingPolicy.IsInBand(1000, 850, 0f), Is.False);
            Assert.That(PvpMatchmakingPolicy.IsInBand(1000, 850, 25f), Is.True);
        }

        [Test]
        public void CallSignsAreStableForASeed()
        {
            Assert.That(BotCallSignGenerator.Generate("match.7"), Is.EqualTo(BotCallSignGenerator.Generate("match.7")));
        }

        [Test]
        public void CallSignsLookLikeHandlesNotLikeLabels()
        {
            for (var i = 0; i < 50; i++)
            {
                var callSign = BotCallSignGenerator.Generate($"match.{i}");

                Assert.That(callSign, Is.Not.Empty);
                Assert.That(callSign, Does.Not.Contain(" "));
                Assert.That(callSign.Length, Is.InRange(8, 20));
            }
        }

        [Test]
        public void CallSignsAreVariedEnoughToNotReadAsGenerated()
        {
            var seen = new HashSet<string>();
            for (var i = 0; i < 200; i++)
            {
                seen.Add(BotCallSignGenerator.Generate($"match.{i}"));
            }

            Assert.That(seen.Count, Is.GreaterThan(150));
        }

        [Test]
        public void SomeCallSignsCarryATrailingNumberAndSomeDoNot()
        {
            var withNumber = 0;
            for (var i = 0; i < 200; i++)
            {
                var callSign = BotCallSignGenerator.Generate($"match.{i}");
                if (char.IsDigit(callSign[callSign.Length - 1]))
                {
                    withNumber++;
                }
            }

            Assert.That(withNumber, Is.InRange(60, 140));
        }
    }
}
