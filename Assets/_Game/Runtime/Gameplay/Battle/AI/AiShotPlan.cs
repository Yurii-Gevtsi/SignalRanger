using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    public readonly struct AiShotPlan
    {
        public AiShotPlan(Vector2 direction, float power)
        {
            Direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
            Power = power;
        }

        public Vector2 Direction { get; }
        public float Power { get; }
    }
}
