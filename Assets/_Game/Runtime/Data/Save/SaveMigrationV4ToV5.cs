namespace Game.Data.Save
{
    public sealed class SaveMigrationV4ToV5 : ISaveMigration
    {
        public int FromVersion => 4;
        public int ToVersion => 5;

        public void Migrate(SaveData data)
        {
            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }
    }
}
