using System;

namespace Game.Data.Save
{
    public interface ISaveService
    {
        SaveData Current { get; }
        event Action<SaveData> Saved;
        SaveLoadResult LoadOrCreate();
        void Save(SaveData data);
        void Delete();
    }
}
