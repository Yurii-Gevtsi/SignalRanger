using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public static class AimCalculator
    {
        public static AimState Calculate(Vector2 origin, Vector2 pointerWorldPosition, AimSettings settings)
        {
            var offset = pointerWorldPosition - origin;
            var distance = offset.magnitude;
            if (distance < settings.MinInputDistance)
            {
                return new AimState(origin, settings.FallbackDirection, settings.MinPower, false);
            }

            // Power scales with drag length across [MinInputDistance, MaxInputDistance],
            // so a short controlled drag spans the whole power range regardless of how far
            // the target is. Aim high for an arc; drag longer for more power.
            var t = Mathf.InverseLerp(settings.MinInputDistance, settings.MaxInputDistance, distance);
            var power = Mathf.Lerp(settings.MinPower, settings.MaxPower, t);
            return new AimState(origin, offset.normalized, power, true);
        }
    }
}
