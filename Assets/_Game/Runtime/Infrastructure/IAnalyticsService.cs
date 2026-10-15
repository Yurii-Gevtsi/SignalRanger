using System.Collections.Generic;

namespace Game.Infrastructure
{
    public interface IAnalyticsService
    {
        void TrackEvent(string eventName, IReadOnlyDictionary<string, string> parameters = null);
    }
}
