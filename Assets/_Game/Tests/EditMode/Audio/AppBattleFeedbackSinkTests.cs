using Game.App;
using Game.Audio;
using Game.Core;
using NUnit.Framework;

namespace Game.Tests.EditMode.Audio
{
    public sealed class AppBattleFeedbackSinkTests
    {
        private sealed class RecordingAudio : IAudioService
        {
            public string LastSfx { get; private set; }
            public int PlayCount { get; private set; }

            public float MusicVolume => 1f;
            public float SfxVolume => 1f;

            public void SetVolumes(float musicVolume, float sfxVolume)
            {
            }

            public void PlaySfx(string sfxId)
            {
                LastSfx = sfxId;
                PlayCount++;
            }

            public void PlayMusic(string musicId)
            {
            }

            public void StopMusic()
            {
            }
        }

        [Test]
        public void Report_PlaysPrefixedSfxForKey()
        {
            var audio = new RecordingAudio();
            var sink = new AppBattleFeedbackSink(audio);

            sink.Report(BattleFeedbackBridge.Shot);

            Assert.That(audio.LastSfx, Is.EqualTo("sfx.battle.shot"));
            Assert.That(audio.PlayCount, Is.EqualTo(1));
        }

        [Test]
        public void Report_IgnoresEmptyKey()
        {
            var audio = new RecordingAudio();
            var sink = new AppBattleFeedbackSink(audio);

            sink.Report(string.Empty);

            Assert.That(audio.PlayCount, Is.EqualTo(0));
        }
    }
}
