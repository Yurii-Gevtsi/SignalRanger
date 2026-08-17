using System;
#if UNITY_ANDROID && !UNITY_EDITOR
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Infrastructure.Ugs
{
    /// <summary>
    /// Unity Authentication: Google Play Games на Android із fallback на
    /// анонімний вхід із збереженням сесії на пристрої.
    /// </summary>
    public sealed class UgsAuthService : IAuthService
    {
        private const string GooglePlayProfileSelectionPromptedKey = "game.gpgs.profile-selection-prompted.v1";
        private AuthProvider _provider;

        public bool IsSignedIn =>
            UnityServices.State == ServicesInitializationState.Initialized
            && AuthenticationService.Instance.IsSignedIn;

        public AuthProvider Provider => IsSignedIn ? _provider : AuthProvider.None;

        public string PlayerId => IsSignedIn ? AuthenticationService.Instance.PlayerId : string.Empty;

        public void SignIn(Action<bool> onCompleted)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            SignInWithGooglePlayGames(onCompleted);
#else
            if (IsSignedIn)
            {
                _provider = AuthProvider.Anonymous;
                onCompleted?.Invoke(true);
            }
            else
            {
                SignInAnonymously(onCompleted);
            }
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private void SignInWithGooglePlayGames(Action<bool> onCompleted)
        {
            try
            {
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate(status =>
                {
                    Debug.Log($"[GPGS] Authentication status: {status}");
                    if (status != SignInStatus.Success)
                    {
                        if (status == SignInStatus.Canceled
                            && PlayerPrefs.GetInt(GooglePlayProfileSelectionPromptedKey, 0) == 0)
                        {
                            // The account picker is a one-time choice, independent of
                            // progression data and therefore unaffected by a reset.
                            PlayerPrefs.SetInt(GooglePlayProfileSelectionPromptedKey, 1);
                            PlayerPrefs.Save();
                            Debug.Log("[GPGS] Automatic sign-in was canceled; requesting the one-time profile selection.");
                            PlayGamesPlatform.Instance.ManuallyAuthenticate(interactiveStatus =>
                            {
                                Debug.Log($"[GPGS] Interactive authentication status: {interactiveStatus}");
                                if (interactiveStatus == SignInStatus.Success)
                                {
                                    RequestServerSideAccess(onCompleted);
                                }
                                else
                                {
                                    SignInAnonymously(onCompleted);
                                }
                            });
                            return;
                        }

                        Debug.LogWarning($"[UGS] Google Play Games sign-in failed: {status}");
                        SignInAnonymously(onCompleted);
                        return;
                    }

                    RequestServerSideAccess(onCompleted);
                });
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Google Play Games initialization failed: {exception.Message}");
                SignInAnonymously(onCompleted);
            }
        }

        private void RequestServerSideAccess(Action<bool> onCompleted)
        {
            PlayGamesPlatform.Instance.RequestServerSideAccess(false, authCode =>
            {
                if (string.IsNullOrWhiteSpace(authCode))
                {
                    Debug.LogWarning("[UGS] Google Play Games returned an empty server auth code.");
                    SignInAnonymously(onCompleted);
                    return;
                }

                SignInWithGooglePlayGamesAsync(authCode, onCompleted);
            });
        }

        private async void SignInWithGooglePlayGamesAsync(string authCode, Action<bool> onCompleted)
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
                }
                else
                {
                    await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
                }

                _provider = AuthProvider.GooglePlayGames;
                Debug.Log("[UGS] Google Play Games authentication succeeded.");
                onCompleted?.Invoke(true);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Google Play Games UGS sign-in failed: {exception.Message}");
                SignInAnonymously(onCompleted);
            }
        }
#endif

        private async void SignInAnonymously(Action<bool> onCompleted)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                _provider = AuthProvider.Anonymous;
                onCompleted?.Invoke(true);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UGS] Anonymous sign-in failed: {exception.Message}");
                _provider = AuthProvider.None;
                onCompleted?.Invoke(false);
            }
        }
    }
}
