using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct AimState
    {
        // The default target projects along the *normalized* launch direction —
        // callers may pass an unnormalized vector (e.g. the slingshot pull), and
        // the pull length must not scale the target away from origin.
        public AimState(Vector2 origin, Vector2 direction, float power, bool hasInput)
            : this(origin, direction, power, hasInput,
                origin + (direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right) * Mathf.Max(0f, power))
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
