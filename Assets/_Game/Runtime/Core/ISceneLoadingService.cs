using System.Collections;

namespace Game.Core
{
    public interface ISceneLoadingService
    {
        void Load(string sceneName);
        IEnumerator LoadAsync(string sceneName);
    }
}
