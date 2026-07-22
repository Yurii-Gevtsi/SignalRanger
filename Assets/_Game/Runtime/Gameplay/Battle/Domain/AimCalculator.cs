using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public static class AimCalculator
    {
        // Below this pull length the direction is unreadable noise; the current
        // aim direction is held instead.
        private const float MinReadablePullDistance = 0.05f;

        /// <summary>
        /// Slingshot aiming: the player presses anywhere comfortable and pulls.
        /// The shot launches opposite the pull (like drawing a bow), and power
        /// scales with how far the pull is dragged across
        /// [MinInputDistance, MaxInputDistance]. The pull is measured from the
        /// press anchor, not from <paramref name="origin"/>, so the finger can
        /// rest anywhere on screen while the arc is drawn from the firing unit.
        /// </summary>
        public static AimState CalculateFromPull(Vector2 origin, Vector2 pull, AimSettings settings)
        {
            var distance = pull.magnitude;
            if (distance < settings.MinInputDistance)
            {
                return new AimState(origin, settings.FallbackDirection, settings.MinPower, false);
            }

            var t = Mathf.InverseLerp(settings.MinInputDistance, settings.MaxInputDistance, distance);
            var power = Mathf.Lerp(settings.MinPower, settings.MaxPower, t);
            var launchDirection = -pull;
            return new AimState(origin, launchDirection, power, true);
        }

        /// <summary>
        /// Continues an aim already established earlier in the same drag. The
        /// dead zone only guards the *start* of a pull; once the player is
        /// aiming, sweeping the finger back near the anchor (e.g. across the
        /// firing unit) must keep the aim live and tracking — power simply
        /// rests at MinPower while the pull is short. Pulls too short to read
        /// a direction from keep the current direction.
        /// </summary>
        public static AimState ContinueFromPull(Vector2 origin, Vector2 pull, AimSettings settings, AimState current)
        {
            var distance = pull.magnitude;
            if (distance < MinReadablePullDistance)
            {
                return new AimState(origin, current.Direction, settings.MinPower, true);
            }

            var t = Mathf.InverseLerp(settings.MinInputDistance, settings.MaxInputDistance, distance);
            var power = Mathf.Lerp(settings.MinPower, settings.MaxPower, t);
            return new AimState(origin, -pull, power, true);
        }
    }
}
