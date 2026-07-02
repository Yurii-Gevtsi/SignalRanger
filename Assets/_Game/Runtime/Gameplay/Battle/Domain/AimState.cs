using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct AimState
    {
        public AimState(Vector2 origin, Vector2 direction, float power, bool hasInput)
            : this(origin, direction, power, hasInput, origin + direction * power)
        {
        }

        public AimState(Vector2 origin, Vector2 direction, float power, bool hasInput, Vector2 target)
        {
            Origin = origin;
            Direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
            Power = Mathf.Max(0f, power);
            HasInput = hasInput;
            Target = target;
        }

        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public float Power { get; }
        public bool HasInput { get; }
        public Vector2 Target { get; }
    }
}
