namespace Game.Data.Save
{
    public sealed class SaveMigrationV5ToV6 : ISaveMigration
    {
        public int FromVersion => 5;
        public int ToVersion => 6;

        public void Migrate(SaveData data)
        {
            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }
    }
}
