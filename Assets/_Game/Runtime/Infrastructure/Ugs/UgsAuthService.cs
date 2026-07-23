using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Infrastructure.Ugs
{
    /// <summary>
    /// Unity Authentication: анонімний вхід із збереженням сесії на пристрої.
    /// Прив'язка Google Play Games (LinkWithGooglePlayGamesAsync) додається на
    /// етапі 1 після налаштування Play Console — див. docs/setup/ugs-setup.md §4.2.
    /// </summary>
    public sealed class UgsAuthService : IAuthService
    {
        public bool IsSignedIn =>
            UnityServices.State == ServicesInitializationState.Initialized
            && AuthenticationService.Instance.IsSignedIn;

        public AuthProvider Provider => IsSignedIn ? AuthProvider.Anonymous : AuthProvider.None;

        public string PlayerId => IsSignedIn ? AuthenticationService.Instance.PlayerId : string.Empty;

        public async void SignIn(Action<bool> onCompleted)
        {
            try
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }

                onCompleted?.Invoke(true);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Anonymous sign-in failed: {exception.Message}");
                onCompleted?.Invoke(false);
            }
        }
    }
}
