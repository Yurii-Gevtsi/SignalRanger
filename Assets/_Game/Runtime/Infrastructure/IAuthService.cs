using System;

namespace Game.Infrastructure
{
    public enum AuthProvider
    {
        None,
        Anonymous,
        GooglePlayGames
    }

    public interface IAuthService
    {
        bool IsSignedIn { get; }
        AuthProvider Provider { get; }
        string PlayerId { get; }
        void SignIn(Action<bool> onCompleted);
    }
}
