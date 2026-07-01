using System;
using UnityEngine;

namespace Game.Data.Save
{
    public static class SaveDataSerializer
    {
        public static string ToJson(SaveData data, bool prettyPrint = true)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.EnsureDefaults();
            return JsonUtility.ToJson(data, prettyPrint);
        }

        public static SaveData FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("Save JSON is empty.", nameof(json));
            }

            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null)
            {
                throw new InvalidOperationException("Save JSON could not be parsed.");
            }

            data.EnsureDefaults();
            return data;
        }
    }
}
