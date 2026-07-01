using System.Collections.Generic;

namespace Game.Infrastructure
{
    public sealed class NoOpAnalyticsService : IAnalyticsService
    {
        public void TrackEvent(string eventName, IReadOnlyDictionary<string, string> parameters = null)
        {
        }
    }
}
