namespace Game.Core
{
    using Game.Core.Battle;

    /// <summary>
    /// Feed that turns battle events into coaching messages. Implemented in the app
    /// layer over the tutorial service; the battle scene only knows this Core contract,
    /// so gameplay never depends on the app/tutorial assemblies.
    /// </summary>
    public interface IBattleTutorialFeed
    {
        /// <summary>Reports a battle event; returns a coaching cue to show, or empty.</summary>
        BattleTutorialCue ReportTrigger(string triggerKey);
        bool IsActiveMissionTutorialCompleted();
        void SkipActiveMissionTutorial();
    }

    /// <summary>
    /// Static bridge between the battle scene (Core/Gameplay) and the tutorial service (App).
    /// The app sets <see cref="Feed"/> during startup; the battle scene reports events through it.
    /// </summary>
    public static class BattleTutorialBridge
    {
        public const string MissionStart = "mission-start";
        public const string AimStarted = "aim-started";
        public const string ShotFired = "shot-fired";
        public const string ProjectileImpact = "projectile-impact";
        public const string BattleWon = "battle-won";

        public static IBattleTutorialFeed Feed { get; set; }

        public static BattleTutorialCue Report(string triggerKey)
        {
            return Feed?.ReportTrigger(triggerKey) ?? BattleTutorialCue.Empty;
        }

        public static void SkipActiveMissionTutorial()
        {
            Feed?.SkipActiveMissionTutorial();
        }

        public static bool IsActiveMissionTutorialCompleted()
        {
            return Feed?.IsActiveMissionTutorialCompleted() ?? false;
        }
    }
}
