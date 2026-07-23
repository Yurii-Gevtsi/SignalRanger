using System;

namespace Game.Infrastructure
{
    public interface IServerTimeService
    {
        bool IsServerAuthoritative { get; }
        DateTime UtcNow { get; }
    }
}
