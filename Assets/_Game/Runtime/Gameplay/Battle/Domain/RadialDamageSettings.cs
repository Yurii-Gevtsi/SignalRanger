using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct RadialDamageSettings
    {
        public RadialDamageSettings(int maxDamage, float radius, float selfDamageMultiplier)
        {
            MaxDamage = Mathf.Max(0, maxDamage);
            Radius = Mathf.Max(0.01f, radius);
            SelfDamageMultiplier = Mathf.Clamp01(selfDamageMultiplier);
        }

        public int MaxDamage { get; }
        public float Radius { get; }
        public float SelfDamageMultiplier { get; }
    }
}
