using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data.Save
{
    public sealed class SaveMigrationService
    {
        private readonly IReadOnlyList<ISaveMigration> _migrations;

        public SaveMigrationService()
            : this(new ISaveMigration[]
            {
                new SaveMigrationV0ToV1(),
                new SaveMigrationV1ToV2(),
                new SaveMigrationV2ToV3(),
                new SaveMigrationV3ToV4(),
                new SaveMigrationV4ToV5(),
                new SaveMigrationV5ToV6(),
                new SaveMigrationV6ToV7(),
                new SaveMigrationV7ToV8(),
                new SaveMigrationV8ToV9()
            })
        {
        }

        public SaveMigrationService(IEnumerable<ISaveMigration> migrations)
        {
            _migrations = migrations?.OrderBy(migration => migration.FromVersion).ToArray()
                ?? Array.Empty<ISaveMigration>();
        }

        public void MigrateToCurrent(SaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.schemaVersion > SaveData.CurrentSchemaVersion)
            {
                throw new InvalidOperationException(
                    $"Save schema {data.schemaVersion} is newer than supported schema {SaveData.CurrentSchemaVersion}.");
            }

            while (data.schemaVersion < SaveData.CurrentSchemaVersion)
            {
                var migration = _migrations.FirstOrDefault(item => item.FromVersion == data.schemaVersion);
                if (migration == null)
                {
                    throw new InvalidOperationException($"Missing save migration from schema {data.schemaVersion}.");
                }

                migration.Migrate(data);
                if (data.schemaVersion != migration.ToVersion)
                {
                    throw new InvalidOperationException(
                        $"Save migration from schema {migration.FromVersion} did not advance to {migration.ToVersion}.");
                }
            }

            data.EnsureDefaults();
        }
    }
}
