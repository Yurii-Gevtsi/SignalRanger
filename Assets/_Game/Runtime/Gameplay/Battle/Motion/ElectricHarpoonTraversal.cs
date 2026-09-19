using UnityEngine;

namespace Game.Gameplay.Battle.Motion
{
    /// <summary>
    /// Owns the straight flight of an electric harpoon and the target currently
    /// caught by its hook. Damage and pulling are resolved by BattleSimulation.
    /// </summary>
    public sealed class ElectricHarpoonTraversal
    {
        public ElectricHarpoonTraversal(
            string ownerUnitId,
            Vector2 origin,
            Vector2 direction,
            float maximumLength,
            float hookSpeed,
            float pullSpeed,
            float damagePerSecond)
        {
            OwnerUnitId = ownerUnitId;
            Origin = origin;
            Direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
            MaximumLength = Mathf.Max(0.5f, maximumLength);
            HookSpeed = Mathf.Max(0.1f, hookSpeed);
            PullSpeed = Mathf.Max(0.1f, pullSpeed);
            DamagePerSecond = Mathf.Max(0f, damagePerSecond);
            HookPosition = origin;
        }

        public string OwnerUnitId { get; }
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public Vector2 HookPosition { get; private set; }
        public string TargetUnitId { get; private set; }
        public float MaximumLength { get; }
        public float HookSpeed { get; }
        public float PullSpeed { get; }
        public float DamagePerSecond { get; }
        public bool IsAttached => !string.IsNullOrWhiteSpace(TargetUnitId);

        public bool StepHook(float deltaTime)
        {
            if (IsAttached)
            {
                return true;
            }

            HookPosition += Direction * HookSpeed * Mathf.Max(0f, deltaTime);
            return Vector2.Distance(Origin, HookPosition) <= MaximumLength;
        }

        public void Attach(string targetUnitId, Vector2 targetPosition)
        {
            if (string.IsNullOrWhiteSpace(targetUnitId))
            {
                return;
            }

            TargetUnitId = targetUnitId;
            HookPosition = targetPosition;
        }
    }
}
