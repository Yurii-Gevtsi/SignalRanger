using System;
using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    /// <summary>
    /// Ballistic projectile integrated against the shared terrain mask in
    /// small sub-steps, so it can never skip through thin ground the way a
    /// physics-engine body could between frames. Gravity matches the old
    /// rigidbody setup (Physics2D.gravity * GravityScale) to keep the aim
    /// preview and AI ballistics unchanged.
    /// </summary>
    public sealed class ProjectileController : MonoBehaviour
    {
        private const float MaxFrameDelta = 0.05f;
        private const float SubStepLength = 0.04f;

        private ProjectileDefinition _definition;
        private IProjectileWorld _world;
        private string _ownerUnitId;
        private Vector2 _velocity;
        private float _remainingLifetime;
        private bool _resolved;

        public event Action<ProjectileImpact> Impacted;

        public void Launch(ProjectileDefinition definition, string ownerUnitId, Vector2 velocity, IProjectileWorld world)
        {
            _definition = definition;
            _ownerUnitId = ownerUnitId;
            _velocity = velocity;
            _world = world;
            _remainingLifetime = definition.TimeoutSeconds;
            _resolved = false;
        }

        private void Update()
        {
            if (_resolved || _definition == null || _world == null)
            {
                return;
            }

            var deltaTime = Mathf.Min(Time.deltaTime, MaxFrameDelta);
            _remainingLifetime -= deltaTime;
            if (_remainingLifetime <= 0f)
            {
                Resolve(transform.position);
                return;
            }

            var gravity = Physics2D.gravity * _definition.GravityScale;
            _velocity += gravity * deltaTime;

            var position = (Vector2)transform.position;
            var displacement = _velocity * deltaTime;
            var stepCount = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / SubStepLength));
            var step = displacement / stepCount;

            for (var i = 0; i < stepCount; i++)
            {
                position += step;

                if (position.y < _world.KillY)
                {
                    Resolve(position);
                    return;
                }

                if (_world.IsSolid(position))
                {
                    Resolve(position);
                    return;
                }

                if (_world.TryHitUnit(position, _definition.Radius, _ownerUnitId, out var hitPoint))
                {
                    Resolve(hitPoint);
                    return;
                }
            }

            transform.position = position;
        }

        private void Resolve(Vector2 point)
        {
            _resolved = true;
            Impacted?.Invoke(new ProjectileImpact(
                _definition.ProjectileId,
                _ownerUnitId,
                point,
                _velocity,
                _definition.Damage,
                _definition.ImpactRadius));

            Destroy(gameObject);
        }
    }
}
