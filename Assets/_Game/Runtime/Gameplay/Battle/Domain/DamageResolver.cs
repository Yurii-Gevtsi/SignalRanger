using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public static class DamageResolver
    {
        public static IReadOnlyList<DamageEvent> ResolveRadialDamage(
            IEnumerable<BattleUnitState> units,
            Vector2 impactPoint,
            string sourceUnitId,
            string sourceId,
            RadialDamageSettings settings)
        {
            var events = new List<DamageEvent>();

            foreach (var unit in units)
            {
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                var distance = Vector2.Distance(impactPoint, unit.Position);
                if (distance > settings.Radius)
                {
                    continue;
                }

                var falloff = Mathf.InverseLerp(settings.Radius, 0f, distance);
                var damage = Mathf.CeilToInt(settings.MaxDamage * falloff);

                if (unit.UnitId == sourceUnitId)
                {
                    damage = Mathf.CeilToInt(damage * settings.SelfDamageMultiplier);
                }

                if (damage <= 0)
                {
                    continue;
                }

                events.Add(unit.ApplyDamage(damage, sourceId));
            }

            return events;
        }
    }
}
