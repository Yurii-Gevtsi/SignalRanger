using System;

namespace Game.Infrastructure
{
    public interface IPremiumStatusService
    {
        bool IsActive { get; }
        DateTime? ExpiresAtUtc { get; }
    }
}
