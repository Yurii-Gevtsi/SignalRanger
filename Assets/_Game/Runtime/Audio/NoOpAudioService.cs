using System;
using Game.Data.Save;
using UnityEngine;

namespace Game.Audio
{
    public sealed class NoOpAudioService : IAudioService
    {
        private readonly SaveData _saveData;

        public NoOpAudioService(SaveData saveData)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
        }

        public float MusicVolume => _saveData.settings.musicVolume;
        public float SfxVolume => _saveData.settings.sfxVolume;

        public void SetVolumes(float musicVolume, float sfxVolume)
        {
            _saveData.settings.musicVolume = Mathf.Clamp01(musicVolume);
            _saveData.settings.sfxVolume = Mathf.Clamp01(sfxVolume);
            _saveData.settings.hasVolumeSettings = true;
        }

        public void PlaySfx(string sfxId)
        {
        }

        public void PlayMusic(string musicId)
        {
        }

        public void StopMusic()
        {
        }
    }
}
