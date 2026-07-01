using Game.Core;
using Game.Data.Save;

namespace Game.App
{
    public static class AppServices
    {
        public static bool IsInitialized { get; private set; }
        public static ISceneLoadingService SceneLoader { get; private set; }
        public static GameSessionService Session { get; private set; }
        public static ISaveService SaveService { get; private set; }
        public static SaveLoadResult LastSaveLoadResult { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            SceneLoader = new UnitySceneLoadingService();
            SaveService = new JsonFileSaveService();
            LastSaveLoadResult = SaveService.LoadOrCreate();
            Session = new GameSessionService(SaveService.Current);
            IsInitialized = true;
        }
    }
}
