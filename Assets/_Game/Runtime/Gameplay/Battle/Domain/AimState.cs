using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct AimState
    {
        public AimState(Vector2 origin, Vector2 direction, float power, bool hasInput)
        {
            Origin = origin;
            Direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
            Power = Mathf.Max(0f, power);
            HasInput = hasInput;
        }

        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public float Power { get; }
        public bool HasInput { get; }
    }
}
