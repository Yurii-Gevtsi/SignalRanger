using System;
using Game.Data.Save;

namespace Game.Infrastructure
{
    public sealed class NoOpAuthService : IAuthService
    {
        private readonly SaveData _saveData;

        public NoOpAuthService(SaveData saveData)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
        }

        public bool IsSignedIn => false;
        public AuthProvider Provider => AuthProvider.None;
        public string PlayerId => _saveData.profile.profileId;

        public void SignIn(Action<bool> onCompleted)
        {
            onCompleted?.Invoke(false);
        }
    }
}
