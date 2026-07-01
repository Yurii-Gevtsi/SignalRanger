namespace Game.Core
{
    /// <summary>
    /// Sink for battle feedback events (fire, impact, defeat, ...). Implemented in the
    /// app layer to route events to audio/VFX services. Gameplay only knows this Core
    /// contract, so it never depends on the audio/app assemblies.
    /// </summary>
    public interface IBattleFeedbackSink
    {
        void Report(string feedbackKey);
    }

    /// <summary>
    /// Static bridge from the battle scene to feedback services (audio now, VFX later).
    /// The app sets <see cref="Sink"/> during startup; the battle reports keyed events.
    /// </summary>
    public static class BattleFeedbackBridge
    {
        public const string Aim = "aim";
        public const string Shot = "shot";
        public const string Impact = "impact";
        public const string Damage = "damage";
        public const string Defeat = "defeat";
        public const string Finished = "finished";

        public static IBattleFeedbackSink Sink { get; set; }

        public static void Report(string feedbackKey)
        {
            Sink?.Report(feedbackKey);
        }
    }
}
