namespace Game.Data.Save
{
    public sealed class SaveMigrationV3ToV4 : ISaveMigration
    {
        public int FromVersion => 3;
        public int ToVersion => 4;

        public void Migrate(SaveData data)
        {
            data.schemaVersion = ToVersion;
            data.ledger ??= SaveLedgerData.CreateDefault();
            data.EnsureDefaults();
        }
    }
}
