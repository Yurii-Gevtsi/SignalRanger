namespace Game.Data.Save
{
    public sealed class SaveMigrationV7ToV8 : ISaveMigration
    {
        public int FromVersion => 7;
        public int ToVersion => 8;

        public void Migrate(SaveData data)
        {
            data.profile ??= SaveProfileData.CreateDefault();
            data.inventory ??= SaveInventoryData.CreateDefault();
            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }
    }
}
