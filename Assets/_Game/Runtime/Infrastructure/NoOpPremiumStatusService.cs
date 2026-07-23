using System;

namespace Game.Infrastructure
{
    public sealed class NoOpPremiumStatusService : IPremiumStatusService
    {
        public bool IsActive => false;
        public DateTime? ExpiresAtUtc => null;
    }
}
