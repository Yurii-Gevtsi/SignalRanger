namespace Game.Data.Save
{
    public sealed class SaveMigrationV2ToV3 : ISaveMigration
    {
        public int FromVersion => 2;
        public int ToVersion => 3;

        public void Migrate(SaveData data)
        {
            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }
    }
}
