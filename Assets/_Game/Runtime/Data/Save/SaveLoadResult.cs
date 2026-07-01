namespace Game.Data.Save
{
    public enum SaveLoadStatus
    {
        Loaded = 0,
        CreatedNew = 1,
        RecoveredFromCorrupt = 2
    }

    public readonly struct SaveLoadResult
    {
        public SaveLoadResult(SaveData data, SaveLoadStatus status, string message)
        {
            Data = data;
            Status = status;
            Message = message ?? string.Empty;
        }

        public SaveData Data { get; }
        public SaveLoadStatus Status { get; }
        public string Message { get; }
    }
}
