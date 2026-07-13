using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public static class AimCalculator
    {
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
    }
}
