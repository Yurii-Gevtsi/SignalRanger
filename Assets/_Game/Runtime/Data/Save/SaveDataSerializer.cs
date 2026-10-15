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

            new SaveMigrationService().MigrateToCurrent(data);
            return data;
        }

        /// <summary>
        /// Переписує вміст наявного інстанса даними з іншого сейву, не змінюючи
        /// саму референцію: сервіси, що тримають цей SaveData, лишаються валідними
        /// (потрібно для прийняття хмарного сейву в живій сесії).
        /// </summary>
        public static void OverwriteInto(SaveData source, SaveData target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            new SaveMigrationService().MigrateToCurrent(source);
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(source), target);
            target.EnsureDefaults();
        }
    }
}
