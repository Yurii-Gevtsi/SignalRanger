using System.Collections;
using Game.Core;

namespace Game.App
{
    public sealed class UnitySceneLoadingService : ISceneLoadingService
    {
        public void Load(string sceneName)
        {
            LoadingScreenController.LoadScene(sceneName);
        }

        public IEnumerator LoadAsync(string sceneName)
        {
            yield return LoadingScreenController.LoadSceneAsync(sceneName);
        }
    }
}
