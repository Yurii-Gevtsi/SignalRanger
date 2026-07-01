namespace Game.Data.Save
{
    public sealed class SaveMigrationV0ToV1 : ISaveMigration
    {
        public int FromVersion => 0;
        public int ToVersion => 1;

        public void Migrate(SaveData data)
        {
            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }
    }
}
