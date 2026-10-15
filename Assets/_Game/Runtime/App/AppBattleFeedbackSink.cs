using Game.Audio;
using Game.Core;

namespace Game.App
{
    /// <summary>
    /// Routes battle feedback keys to SFX ids on the audio service. Keeps the mapping
    /// in one place so gameplay only emits abstract keys and the audio layer decides
    /// which clip (if any) plays.
    /// </summary>
    public sealed class AppBattleFeedbackSink : IBattleFeedbackSink
    {
        private readonly IAudioService _audio;

        public AppBattleFeedbackSink(IAudioService audio)
        {
            _audio = audio;
        }

        public void Report(string feedbackKey)
        {
            if (_audio == null || string.IsNullOrEmpty(feedbackKey))
            {
                return;
            }

            _audio.PlaySfx($"sfx.battle.{feedbackKey}");
        }
    }
}
