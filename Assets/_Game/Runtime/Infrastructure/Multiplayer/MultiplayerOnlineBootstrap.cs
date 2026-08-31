using Game.App;
using UnityEngine;

namespace Game.Infrastructure.Multiplayer
{
    /// <summary>
    /// Swaps the offline duel adapters for the live ones once the online bootstrap
    /// has signed in. Nothing here runs before that: matchmaking needs a player
    /// identity, and a session needs a signed-in player to join it.
    /// <para>
    /// Until this fires - no project linked, no network, packages absent - the game
    /// keeps the local stand-in adapters and duels remain playable offline.
    /// </para>
    /// </summary>
    internal static class MultiplayerOnlineBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            // Subscribing after the fact would miss the event, so a bootstrap that
            // lost the race attaches immediately instead.
            if (AppServices.AreOnlineServicesAttached)
            {
                Attach();
                return;
            }

            AppServices.OnlineServicesAttached += Attach;
        }

        private static void Attach()
        {
            AppServices.OnlineServicesAttached -= Attach;

            var matchmaker = new UgsMatchmakerService();
            AppServices.AttachMatchmakingService(matchmaker);
            // The transport reads whichever session matchmaking ended up joining, so
            // pairing and messaging stay independently replaceable.
            AppServices.AttachPvpSessionService(
                new MultiplayerPvpSessionService(() => matchmaker.CurrentSession));
            Debug.Log("[Multiplayer] Duel matchmaking and transport attached.");
        }
    }
}
