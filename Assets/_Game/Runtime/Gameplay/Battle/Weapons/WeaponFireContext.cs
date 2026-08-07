using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Everything a weapon effect needs to fire once: which weapon, who fired it,
    /// the resolved aim, and the spawner used to place projectiles.
    /// </summary>
    public readonly struct WeaponFireContext
    {
        public WeaponFireContext(
            WeaponDefinition weapon,
            string ownerUnitId,
            Vector2 origin,
            Vector2 direction,
            float power,
            IProjectileSpawner spawner,
            WeaponEvolutionProfile evolution = default)
        {
            Weapon = weapon;
            OwnerUnitId = ownerUnitId;
            Origin = origin;
            Direction = direction;
            Power = power;
            Spawner = spawner;
            Evolution = evolution;
        }

        public WeaponDefinition Weapon { get; }
        public string OwnerUnitId { get; }
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public float Power { get; }
        public IProjectileSpawner Spawner { get; }
        public WeaponEvolutionProfile Evolution { get; }
    }
}
