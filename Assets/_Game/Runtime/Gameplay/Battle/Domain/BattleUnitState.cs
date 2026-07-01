using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public sealed class BattleUnitState
    {
        public BattleUnitState(string unitId, BattleTeamId teamId, Vector2 position, int maxHealth)
        {
            UnitId = unitId;
            TeamId = teamId;
            Position = position;
            MaxHealth = Mathf.Max(1, maxHealth);
            Health = MaxHealth;
        }

        public string UnitId { get; }
        public BattleTeamId TeamId { get; }
        public Vector2 Position { get; private set; }
        public int MaxHealth { get; }
        public int Health { get; private set; }
        public bool IsDefeated => Health <= 0;

        public void MoveTo(Vector2 position)
        {
            Position = position;
        }

        public DamageEvent ApplyDamage(int amount, string sourceId)
        {
            var clampedAmount = Mathf.Max(0, amount);
            var healthBefore = Health;
            Health = Mathf.Max(0, Health - clampedAmount);

            return new DamageEvent(
                UnitId,
                TeamId,
                sourceId,
                clampedAmount,
                healthBefore,
                Health);
        }
    }
}
