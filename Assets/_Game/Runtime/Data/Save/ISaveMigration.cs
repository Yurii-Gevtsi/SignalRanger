namespace Game.Data.Save
{
    public interface ISaveMigration
    {
        int FromVersion { get; }
        int ToVersion { get; }
        void Migrate(SaveData data);
    }
}
