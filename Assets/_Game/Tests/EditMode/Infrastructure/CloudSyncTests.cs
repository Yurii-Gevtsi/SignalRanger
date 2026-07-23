using System;
using System.Collections.Generic;
using Game.Data.Save;
using Game.Infrastructure;
using NUnit.Framework;

namespace Game.Tests.EditMode.Infrastructure
{
    public sealed class CloudSyncPolicyTests
    {
        private static SaveData CreateSave(long lastWriteUtcTicks)
        {
            var data = SaveData.CreateNew();
            data.lastWriteUtcTicks = lastWriteUtcTicks;
            return data;
        }

        [Test]
        public void Decide_NoRemote_PushesLocal()
        {
            var local = CreateSave(100);

            Assert.That(CloudSyncPolicy.Decide(local, null, false), Is.EqualTo(CloudSyncDecision.PushLocal));
        }

        [Test]
        public void Decide_FreshInstallWithRemote_AdoptsRemote()
        {
            var local = CreateSave(200);
            var remote = CreateSave(100);

            Assert.That(CloudSyncPolicy.Decide(local, remote, true), Is.EqualTo(CloudSyncDecision.AdoptRemote));
        }

        [Test]
        public void Decide_RemoteSlightlyNewer_AdoptsRemote()
        {
            var now = DateTime.UtcNow.Ticks;
            var local = CreateSave(now);
            var remote = CreateSave(now + TimeSpan.FromMinutes(30).Ticks);

            Assert.That(CloudSyncPolicy.Decide(local, remote, false), Is.EqualTo(CloudSyncDecision.AdoptRemote));
        }

        [Test]
        public void Decide_RemoteMuchNewer_AsksPlayer()
        {
            var now = DateTime.UtcNow.Ticks;
            var local = CreateSave(now);
            var remote = CreateSave(now + TimeSpan.FromDays(3).Ticks);

            Assert.That(CloudSyncPolicy.Decide(local, remote, false), Is.EqualTo(CloudSyncDecision.AskPlayer));
        }

        [Test]
        public void Decide_LocalNewer_PushesLocal()
        {
            var now = DateTime.UtcNow.Ticks;
            var local = CreateSave(now + TimeSpan.FromHours(1).Ticks);
            var remote = CreateSave(now);

            Assert.That(CloudSyncPolicy.Decide(local, remote, false), Is.EqualTo(CloudSyncDecision.PushLocal));
        }

        [Test]
        public void Decide_RemoteFromNewerGameVersion_DoesNothing()
        {
            var local = CreateSave(100);
            var remote = CreateSave(200);
            remote.schemaVersion = SaveData.CurrentSchemaVersion + 1;

            Assert.That(CloudSyncPolicy.Decide(local, remote, false), Is.EqualTo(CloudSyncDecision.NoOp));
        }
    }

    public sealed class CloudSaveSyncCoordinatorTests
    {
        private sealed class FakeSaveService : ISaveService
        {
            public FakeSaveService(SaveData data)
            {
                Current = data;
            }

            public int SaveCount { get; private set; }
            public SaveData Current { get; }

            public event Action<SaveData> Saved;

            public SaveLoadResult LoadOrCreate() => default;

            public void Save(SaveData data)
            {
                SaveCount++;
                Saved?.Invoke(data);
            }

            public void Delete()
            {
            }
        }

        private sealed class FakeCloudSync : ICloudSyncService
        {
            public bool IsAvailable { get; set; } = true;
            public SaveData Remote { get; set; }
            public List<SaveData> Pushed { get; } = new List<SaveData>();

            public void PushSnapshot(SaveData saveData, Action<bool> onCompleted = null)
            {
                Pushed.Add(saveData);
                onCompleted?.Invoke(true);
            }

            public void PullSnapshot(Action<SaveData> onCompleted)
            {
                onCompleted?.Invoke(Remote);
            }
        }

        [Test]
        public void LocalSave_IsPushedToCloud()
        {
            var saveService = new FakeSaveService(SaveData.CreateNew());
            var cloud = new FakeCloudSync();
            using var coordinator = new CloudSaveSyncCoordinator(saveService, cloud, false);

            saveService.Save(saveService.Current);

            Assert.That(cloud.Pushed, Has.Count.EqualTo(1));
        }

        [Test]
        public void SignIn_WithoutRemote_PushesLocal()
        {
            var saveService = new FakeSaveService(SaveData.CreateNew());
            var cloud = new FakeCloudSync { Remote = null };
            using var coordinator = new CloudSaveSyncCoordinator(saveService, cloud, false);

            CloudSyncDecision? resolved = null;
            coordinator.OnSignedIn(decision => resolved = decision);

            Assert.That(resolved, Is.EqualTo(CloudSyncDecision.PushLocal));
            Assert.That(cloud.Pushed, Has.Count.EqualTo(1));
        }

        [Test]
        public void SignIn_FreshInstall_AdoptsRemoteIntoSameInstance()
        {
            var local = SaveData.CreateNew();
            var saveService = new FakeSaveService(local);
            var remote = SaveData.CreateNew();
            remote.economy.softCurrency = 777;
            remote.lastWriteUtcTicks = local.lastWriteUtcTicks + 1;
            var cloud = new FakeCloudSync { Remote = remote };
            using var coordinator = new CloudSaveSyncCoordinator(saveService, cloud, true);

            coordinator.OnSignedIn();

            Assert.That(saveService.Current, Is.SameAs(local), "референція SaveData має лишитися тією самою");
            Assert.That(local.economy.softCurrency, Is.EqualTo(777));
            Assert.That(saveService.SaveCount, Is.EqualTo(1));
        }

        [Test]
        public void SignIn_CloudUnavailable_DoesNothing()
        {
            var saveService = new FakeSaveService(SaveData.CreateNew());
            var cloud = new FakeCloudSync { IsAvailable = false };
            using var coordinator = new CloudSaveSyncCoordinator(saveService, cloud, false);

            CloudSyncDecision? resolved = null;
            coordinator.OnSignedIn(decision => resolved = decision);

            Assert.That(resolved, Is.EqualTo(CloudSyncDecision.NoOp));
            Assert.That(cloud.Pushed, Is.Empty);
        }

        [Test]
        public void Dispose_StopsPushingOnSave()
        {
            var saveService = new FakeSaveService(SaveData.CreateNew());
            var cloud = new FakeCloudSync();
            var coordinator = new CloudSaveSyncCoordinator(saveService, cloud, false);
            coordinator.Dispose();

            saveService.Save(saveService.Current);

            Assert.That(cloud.Pushed, Is.Empty);
        }
    }
}
