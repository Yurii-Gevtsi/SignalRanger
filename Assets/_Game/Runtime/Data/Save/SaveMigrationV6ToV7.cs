namespace Game.Data.Save
{
    public sealed class SaveMigrationV6ToV7 : ISaveMigration
    {
        public int FromVersion => 6;
        public int ToVersion => 7;

        public void Migrate(SaveData data)
        {
            data.activity ??= SaveActivityProgressData.CreateDefault();
            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }
    }
}
