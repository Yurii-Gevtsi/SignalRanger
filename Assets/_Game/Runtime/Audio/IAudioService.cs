namespace Game.Audio
{
    public interface IAudioService
    {
        float MusicVolume { get; }
        float SfxVolume { get; }
        void SetVolumes(float musicVolume, float sfxVolume);
        void PlaySfx(string sfxId);
        void PlayMusic(string musicId);
        void StopMusic();
    }
}
