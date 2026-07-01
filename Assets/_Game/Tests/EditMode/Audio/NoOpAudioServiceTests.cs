using Game.Audio;
using Game.Data.Save;
using NUnit.Framework;

namespace Game.Tests.EditMode.Audio
{
    public sealed class NoOpAudioServiceTests
    {
        [Test]
        public void SetVolumes_ClampsAndPersistsToSave()
        {
            var saveData = SaveData.CreateNew();
            var service = new NoOpAudioService(saveData);

            service.SetVolumes(1.5f, -0.5f);

            Assert.That(service.MusicVolume, Is.EqualTo(1f));
            Assert.That(service.SfxVolume, Is.EqualTo(0f));
            Assert.That(saveData.settings.hasVolumeSettings, Is.True);
        }
    }
}
