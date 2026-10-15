using Game.App;
using UnityEngine;

namespace Game.Ads
{
    public sealed class GoogleMobileAdsBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (FindAnyObjectByType<GoogleMobileAdsBootstrap>() != null)
            {
                return;
            }

            var root = new GameObject("Google Mobile Ads Bootstrap");
            DontDestroyOnLoad(root);
            root.AddComponent<GoogleMobileAdsBootstrap>();
#endif
        }

        private void Awake()
        {
            AppServices.Initialize();
            var ads = new GoogleMobileAdsService();
            AppServices.AttachAdsService(ads);
            ads.Initialize();
        }
    }
}
