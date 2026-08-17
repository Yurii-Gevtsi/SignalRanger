using System.Collections;
using Game.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.App
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private bool _startupSequenceStarted;

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
            // Keep the branded entry splash even when a platform/editor launches the
            // menu scene directly instead of honoring the Boot scene build entry.
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == GameSceneNames.Menu && !_startupSequenceStarted)
            {
                _startupSequenceStarted = true;
                StartCoroutine(ShowStartupOverExistingMenu());
                return;
            }

            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
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

        private IEnumerator ShowStartupOverExistingMenu()
        {
            AppServices.Session.SelectNextMission();
            yield return StartupSplashController.ShowOverCurrentScene();
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
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
