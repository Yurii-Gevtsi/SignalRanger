using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    [CreateAssetMenu(menuName = "Game/Battle/Projectile Definition", fileName = "ProjectileDefinition")]
    public sealed class ProjectileDefinition : ScriptableObject
    {
        [SerializeField] private string projectileId = "projectile.basic";
        [SerializeField] private float radius = 0.14f;
        [SerializeField] private float gravityScale = 1f;
        [SerializeField] private float timeoutSeconds = 8f;
        [SerializeField] private int damage = 40;
        [SerializeField] private float impactRadius = 1.5f;

        public string ProjectileId => projectileId;
        public float Radius => Mathf.Max(0.01f, radius);
        public float GravityScale => Mathf.Max(0f, gravityScale);
        public float TimeoutSeconds => Mathf.Max(0.1f, timeoutSeconds);
        public int Damage => Mathf.Max(0, damage);
        public float ImpactRadius => Mathf.Max(0.01f, impactRadius);

        public static ProjectileDefinition CreateRuntime(
            string id,
            float radius,
            float gravityScale,
            float timeoutSeconds,
            int damage,
            float impactRadius)
        {
            var definition = CreateInstance<ProjectileDefinition>();
            definition.projectileId = id;
            definition.radius = radius;
            definition.gravityScale = gravityScale;
            definition.timeoutSeconds = timeoutSeconds;
            definition.damage = damage;
            definition.impactRadius = impactRadius;
            return definition;
        }
    }
}
