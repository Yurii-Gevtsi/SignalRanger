using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Motion
{
    /// <summary>Owns the hook's straight flight and the fixed anchor used by a droid on a rope.</summary>
    public sealed class RopeTraversal
    {
        private const float HookStepLength = 0.06f;

        public RopeTraversal(string ownerUnitId, Vector2 origin, Vector2 direction, float maximumLength, float hookSpeed,
            float sectorDegrees, float swingAcceleration)
        {
            OwnerUnitId = ownerUnitId;
            Origin = origin;
            Direction = ClampToUpwardSector(direction, sectorDegrees);
            MaximumLength = Mathf.Max(0.5f, maximumLength);
            HookSpeed = Mathf.Max(0.1f, hookSpeed);
            SwingAcceleration = Mathf.Max(0.1f, swingAcceleration);
            HookPosition = origin;
        }

        public string OwnerUnitId { get; }
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public Vector2 HookPosition { get; private set; }
        public Vector2 Anchor { get; private set; }
        public float MaximumLength { get; }
        public float HookSpeed { get; }
        public float SwingAcceleration { get; }
        public bool IsAttached { get; private set; }

        public bool StepHook(BattleTerrain terrain, float deltaTime)
        {
            if (IsAttached || terrain == null)
            {
                return IsAttached;
            }

            var displacement = Direction * HookSpeed * Mathf.Max(0f, deltaTime);
            var steps = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / HookStepLength));
            var step = displacement / steps;
            for (var i = 0; i < steps; i++)
            {
                HookPosition += step;
                if (Vector2.Distance(Origin, HookPosition) > MaximumLength)
                {
                    return false;
                }

                if (!terrain.Grid.IsSolidWorld(HookPosition))
                {
                    continue;
                }

                Anchor = HookPosition;
                IsAttached = true;
                return true;
            }

            return true;
        }

        public void Attach(UnitMotor motor)
        {
            if (!IsAttached || motor == null)
            {
                return;
            }

            motor.AttachRope(
                Anchor,
                Mathf.Min(MaximumLength, Vector2.Distance(motor.Position, Anchor)),
                MaximumLength,
                SwingAcceleration);
        }

        public void SetInput(UnitMotor motor, int horizontalDirection, int verticalDirection)
        {
            if (IsAttached)
            {
                motor?.SetRopeInput(horizontalDirection, verticalDirection);
            }
        }

        public void Detach(UnitMotor motor)
        {
            motor?.DetachRope();
        }

        private static Vector2 ClampToUpwardSector(Vector2 direction, float sectorDegrees)
        {
            var radians = Mathf.Atan2(direction.y, direction.x);
            var halfSector = Mathf.Deg2Rad * Mathf.Clamp(sectorDegrees, 0f, 179f) * 0.5f;
            radians = Mathf.Clamp(radians, Mathf.PI * 0.5f - halfSector, Mathf.PI * 0.5f + halfSector);
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }
    }
}
