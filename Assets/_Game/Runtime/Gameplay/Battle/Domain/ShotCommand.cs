using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct ShotCommand
    {
        public ShotCommand(string shooterUnitId, string weaponId, Vector2 direction, float power)
        {
            ShooterUnitId = shooterUnitId;
            WeaponId = weaponId;
            Direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
            Power = Mathf.Max(0f, power);
        }

        public string ShooterUnitId { get; }
        public string WeaponId { get; }
        public Vector2 Direction { get; }
        public float Power { get; }
    }
}
