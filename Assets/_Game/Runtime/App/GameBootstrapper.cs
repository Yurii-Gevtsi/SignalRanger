using Game.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.App
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateRuntimeRoot()
        {
            if (FindAnyObjectByType<GameBootstrapper>() != null)
            {
                return;
            }

            var root = new GameObject("Game Bootstrapper");
            root.AddComponent<GameBootstrapper>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            AppServices.Initialize();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameSceneNames.Boot)
            {
                AppServices.Session.SelectNextMission();
                AppServices.SceneLoader.Load(GameSceneNames.Menu);
                return;
            }

            if (scene.name == GameSceneNames.Menu)
            {
                ApplyPendingMissionCompletion();
                MenuSceneController.EnsureCreated();
                return;
            }

            if (scene.name == GameSceneNames.Battle)
            {
                Game.Gameplay.Battle.Assembly.BattleSceneBootstrap.EnsureCreated();
            }
        }

        private static void ApplyPendingMissionCompletion()
        {
            if (!GameSessionContext.HasPendingMissionCompletion)
            {
                return;
            }

            var missionIndex = GameSessionContext.ConsumePendingMissionCompletion();
            var result = AppServices.Campaign.CompleteMission(missionIndex);
            AppServices.LastMissionCompletionResult = result;
            AppServices.MarkPendingLoadoutReview(result.Applied && result.HasUnlock);
            if (result.Applied)
            {
                AppServices.SaveService.Save(AppServices.SaveService.Current);
            }

            AppServices.Session.SelectNextMission();
        }
    }
}
