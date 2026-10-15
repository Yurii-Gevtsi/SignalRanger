using System.Collections;
using Game.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.App
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private bool _startupSequenceStarted;
        private bool _initialSceneResolved;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void ConfigureMobileOrientation()
        {
#if UNITY_ANDROID || UNITY_IOS
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
#endif
        }

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
            // The initial sceneLoaded callback normally arrives before Start. This
            // fallback covers editor/domain-reload edge cases without allowing a
            // directly opened Battle scene to construct a combat controller.
            if (!_initialSceneResolved)
            {
                HandleSceneLoaded(SceneManager.GetActiveScene());
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            HandleSceneLoaded(scene);
        }

        private void HandleSceneLoaded(Scene scene)
        {
            if (!_initialSceneResolved)
            {
                _initialSceneResolved = true;
#if UNITY_INCLUDE_TESTS
                // Preserve the framework's temporary runner scene until its tests
                // load an application scene themselves. Normal launches still boot.
                if (scene.name.StartsWith("InitTestScene", System.StringComparison.Ordinal)
                    && System.Guid.TryParse(scene.name.Substring("InitTestScene".Length), out _))
                {
                    return;
                }
#endif
                if (scene.name != GameSceneNames.Boot)
                {
                    // Every application launch has one route: Boot -> loading -> menu.
                    // This also makes pressing Play from Battle in the editor harmless.
                    SceneManager.LoadScene(GameSceneNames.Boot);
                    return;
                }
            }

            if (scene.name == GameSceneNames.Boot)
            {
                if (!_startupSequenceStarted)
                {
                    _startupSequenceStarted = true;
                    StartCoroutine(ShowStartupThenLoadMenu());
                }
                return;
            }

            if (scene.name == GameSceneNames.Menu)
            {
                ApplyPendingMissionCompletion();
                if (IntroSceneController.ShouldShow())
                {
                    IntroSceneController.EnsureCreated();
                    return;
                }

                MenuSceneController.EnsureCreated();
                return;
            }

            if (scene.name == GameSceneNames.Battle)
            {
                Game.Gameplay.Battle.Assembly.BattleSceneBootstrap.EnsureCreated();
            }
        }

        private IEnumerator ShowStartupThenLoadMenu()
        {
            AppServices.Session.SelectNextMission();
            yield return StartupSplashController.ShowWhileLoadingScene(GameSceneNames.Menu);
        }

        private static void ApplyPendingMissionCompletion()
        {
            if (!GameSessionContext.HasPendingMissionCompletion)
            {
                return;
            }

            var missionIndex = GameSessionContext.ConsumePendingMissionCompletion();

            // Winning is necessary but no longer always sufficient: a mission with an
            // objective only banks when the battle also met it. The mission stays
            // outstanding otherwise, so the player can retry it for the objective.
            var mission = AppServices.Campaign.GetMission(missionIndex);
            var facts = new Game.Progression.Missions.BattleObjectiveFacts(
                GameSessionContext.LastBattlePlayerWon,
                GameSessionContext.LastBattleHealthPercent,
                GameSessionContext.LastBattleOpponentRingOut);
            if (mission != null && !mission.Objective.IsSatisfiedBy(facts))
            {
                return;
            }

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
