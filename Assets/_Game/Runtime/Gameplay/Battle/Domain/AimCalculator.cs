using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public static class AimCalculator
    {
        public static AimState Calculate(Vector2 origin, Vector2 pointerWorldPosition, AimSettings settings)
        {
            var offset = pointerWorldPosition - origin;
            if (offset.magnitude < settings.MinInputDistance)
            {
                return new AimState(origin, settings.FallbackDirection, settings.MinPower, false);
            }

            var power = Mathf.Clamp(offset.magnitude * settings.PowerScale, settings.MinPower, settings.MaxPower);
            return new AimState(origin, offset.normalized, power, true);
        }
    }
}
