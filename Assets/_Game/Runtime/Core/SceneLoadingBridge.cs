using System;

namespace Game.Core
{
    public static class SceneLoadingBridge
    {
        public static Action<string> LoadHandler { get; set; }

        public static bool TryLoad(string sceneName)
        {
            if (LoadHandler == null)
            {
                return false;
            }

            LoadHandler.Invoke(sceneName);
            return true;
        }
    }
}
