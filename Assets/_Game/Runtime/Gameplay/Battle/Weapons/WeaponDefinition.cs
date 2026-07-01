using System.Collections.Generic;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Projectiles;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    [CreateAssetMenu(menuName = "Game/Battle/Weapon Definition", fileName = "WeaponDefinition")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        [SerializeField] private string _weaponId = "weapon.basic";
        [SerializeField] private string _displayKey = "weapon.basic.name";
        [SerializeField] private WeaponRole _role = WeaponRole.ArcingExplosive;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _damage = 40;
        [SerializeField] private float _impactRadius = 1.5f;
        [SerializeField] private float _projectileRadius = 0.13f;
        [SerializeField] private float _gravityScale = 1f;
        [SerializeField] private float _timeoutSeconds = 8f;
        [SerializeField] private float _minPower = 8f;
        [SerializeField] private float _maxPower = 20f;
        [SerializeField] private int _ammoCount = -1;
        [SerializeField] private string _tutorialHintKey;
        [SerializeField] private string[] _tags;

        public string WeaponId => _weaponId;
        public string DisplayKey => _displayKey;
        public WeaponRole Role => _role;
        public Sprite Icon => _icon;
        public int Damage => Mathf.Max(0, _damage);
        public float ImpactRadius => Mathf.Max(0.01f, _impactRadius);
        public float ProjectileRadius => Mathf.Max(0.01f, _projectileRadius);
        public float GravityScale => Mathf.Max(0f, _gravityScale);
        public float TimeoutSeconds => Mathf.Max(0.1f, _timeoutSeconds);
        public float MinPower => Mathf.Max(0f, _minPower);
        public float MaxPower => Mathf.Max(MinPower, _maxPower);
        public int AmmoCount => _ammoCount;
        public string TutorialHintKey => _tutorialHintKey;
        public IReadOnlyList<string> Tags => _tags;
        public bool HasLimitedAmmo => _ammoCount >= 0;

        public bool IsValid => !string.IsNullOrWhiteSpace(_weaponId)
            && !string.IsNullOrWhiteSpace(_displayKey)
            && MaxPower > 0f
            && Damage >= 0;

        public AimSettings CreateAimSettings(AimSettings baseSettings)
        {
            return new AimSettings(MinPower, MaxPower, baseSettings.PowerScale, baseSettings.MinInputDistance, baseSettings.FallbackDirection);
        }

        public ProjectileDefinition CreateProjectileDefinition()
        {
            return ProjectileDefinition.CreateRuntime(
                $"projectile.{_weaponId}",
                ProjectileRadius,
                GravityScale,
                TimeoutSeconds,
                Damage,
                ImpactRadius);
        }

        public static WeaponDefinition CreateRuntime(
            string weaponId,
            string displayKey,
            WeaponRole role,
            int damage,
            float impactRadius,
            float projectileRadius,
            float gravityScale,
            float timeoutSeconds,
            float minPower,
            float maxPower,
            int ammoCount,
            string tutorialHintKey,
            string[] tags)
        {
            var definition = CreateInstance<WeaponDefinition>();
            definition._weaponId = weaponId;
            definition._displayKey = displayKey;
            definition._role = role;
            definition._damage = damage;
            definition._impactRadius = impactRadius;
            definition._projectileRadius = projectileRadius;
            definition._gravityScale = gravityScale;
            definition._timeoutSeconds = timeoutSeconds;
            definition._minPower = minPower;
            definition._maxPower = maxPower;
            definition._ammoCount = ammoCount;
            definition._tutorialHintKey = tutorialHintKey;
            definition._tags = tags;
            return definition;
        }
    }
}
