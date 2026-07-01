using System.Collections;
using Game.Core;
using UnityEngine.SceneManagement;

namespace Game.App
{
    public sealed class UnitySceneLoadingService : ISceneLoadingService
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public IEnumerator LoadAsync(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            while (operation != null && !operation.isDone)
            {
                yield return null;
            }
        }
    }
}
