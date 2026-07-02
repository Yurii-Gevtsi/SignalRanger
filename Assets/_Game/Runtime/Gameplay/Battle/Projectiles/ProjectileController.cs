using System;
using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public sealed class ProjectileController : MonoBehaviour
    {
        private ProjectileDefinition _definition;
        private Rigidbody2D _body;
        private Collider2D _collider;
        private string _ownerUnitId;
        private float _remainingLifetime;
        private bool _resolved;

        public event Action<ProjectileImpact> Impacted;

        public void Launch(ProjectileDefinition definition, string ownerUnitId, Vector2 velocity, Collider2D ignoredOwnerCollider = null)
        {
            _definition = definition;
            _ownerUnitId = ownerUnitId;
            _remainingLifetime = definition.TimeoutSeconds;
            _resolved = false;

            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _body.gravityScale = definition.GravityScale;
            _body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _body.linearVelocity = velocity;

            if (_collider is CircleCollider2D circle)
            {
                circle.radius = definition.Radius;
            }

            if (ignoredOwnerCollider != null)
            {
                Physics2D.IgnoreCollision(_collider, ignoredOwnerCollider, true);
            }
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (_resolved || _definition == null)
            {
                return;
            }

            _remainingLifetime -= Time.deltaTime;
            if (_remainingLifetime <= 0f)
            {
                Resolve(transform.position);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_resolved || _definition == null)
            {
                return;
            }

            var point = collision.contactCount > 0
                ? collision.GetContact(0).point
                : (Vector2)transform.position;

            Resolve(point);
        }

        private void Resolve(Vector2 point)
        {
            _resolved = true;
            Impacted?.Invoke(new ProjectileImpact(
                _definition.ProjectileId,
                _ownerUnitId,
                point,
                _body != null ? _body.linearVelocity : Vector2.zero,
                _definition.Damage,
                _definition.ImpactRadius));

            Destroy(gameObject);
        }
    }
}
