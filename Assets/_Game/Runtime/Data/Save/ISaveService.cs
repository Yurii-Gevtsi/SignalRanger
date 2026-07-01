namespace Game.Data.Save
{
    public interface ISaveService
    {
        SaveData Current { get; }
        SaveLoadResult LoadOrCreate();
        void Save(SaveData data);
        void Delete();
    }
}
