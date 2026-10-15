using System;

namespace Game.Infrastructure
{
    public sealed class NoOpServerTimeService : IServerTimeService
    {
        public bool IsServerAuthoritative => false;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
