using System;
using System.IO;
using UnityEngine;

namespace Game.Data.Save
{
    public sealed class JsonFileSaveService : ISaveService
    {
        public const string DefaultFileName = "player-save.json";

        private readonly string _savePath;

        public JsonFileSaveService()
            : this(Path.Combine(Application.persistentDataPath, DefaultFileName))
        {
        }

        public JsonFileSaveService(string savePath)
        {
            if (string.IsNullOrWhiteSpace(savePath))
            {
                throw new ArgumentException("Save path cannot be empty.", nameof(savePath));
            }

            _savePath = savePath;
        }

        public SaveData Current { get; private set; }

        public SaveLoadResult LoadOrCreate()
        {
            if (!File.Exists(_savePath))
            {
                Current = SaveData.CreateNew();
                Save(Current);
                return new SaveLoadResult(Current, SaveLoadStatus.CreatedNew, "Created new local save.");
            }

            try
            {
                var json = File.ReadAllText(_savePath);
                Current = SaveDataSerializer.FromJson(json);
                return new SaveLoadResult(Current, SaveLoadStatus.Loaded, "Loaded local save.");
            }
            catch (Exception exception)
            {
                var backupPath = TryBackupCorruptFile();
                Current = SaveData.CreateNew();
                Save(Current);

                var message = string.IsNullOrEmpty(backupPath)
                    ? $"Recovered from corrupt save: {exception.Message}"
                    : $"Recovered from corrupt save. Backup: {backupPath}";
                return new SaveLoadResult(Current, SaveLoadStatus.RecoveredFromCorrupt, message);
            }
        }

        public void Save(SaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var directory = Path.GetDirectoryName(_savePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_savePath, SaveDataSerializer.ToJson(data));
            Current = data;
        }

        public void Delete()
        {
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
            }

            Current = null;
        }

        private string TryBackupCorruptFile()
        {
            try
            {
                var directory = Path.GetDirectoryName(_savePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileName = Path.GetFileNameWithoutExtension(_savePath);
                var extension = Path.GetExtension(_savePath);
                var backupFileName = $"{fileName}.corrupt-{DateTime.UtcNow:yyyyMMddHHmmssfff}{extension}";
                var backupPath = string.IsNullOrEmpty(directory)
                    ? backupFileName
                    : Path.Combine(directory, backupFileName);

                File.Copy(_savePath, backupPath, false);
                return backupPath;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
