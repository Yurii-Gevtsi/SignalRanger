namespace Game.Data.Save
{
    public sealed class SaveMigrationV1ToV2 : ISaveMigration
    {
        public int FromVersion => 1;
        public int ToVersion => 2;

        public void Migrate(SaveData data)
        {
            data.EnsureDefaults();

            if (!data.settings.hasVolumeSettings)
            {
                data.settings.musicVolume = 0.8f;
                data.settings.sfxVolume = 1f;
                data.settings.hasVolumeSettings = true;
            }

            data.schemaVersion = ToVersion;
        }
    }
}
