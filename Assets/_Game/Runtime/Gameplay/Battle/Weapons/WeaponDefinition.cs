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
        [SerializeField] private string _descriptionKey = "weapon.basic.description";
        [SerializeField] private WeaponRole _role = WeaponRole.ArcingExplosive;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _iconId = "weapon.basic.icon";
        [SerializeField] private string _vfxId = "weapon.basic.vfx";
        [SerializeField] private string _sfxId = "weapon.basic.sfx";
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
        public string DescriptionKey => _descriptionKey;
        public WeaponRole Role => _role;
        public Sprite Icon => _icon;
        public string IconId => _iconId;
        public string VfxId => _vfxId;
        public string SfxId => _sfxId;
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
            && !string.IsNullOrWhiteSpace(_descriptionKey)
            && !string.IsNullOrWhiteSpace(_iconId)
            && !string.IsNullOrWhiteSpace(_vfxId)
            && !string.IsNullOrWhiteSpace(_sfxId)
            && MaxPower > 0f
            && Damage >= 0;

        public AimSettings CreateAimSettings(AimSettings baseSettings)
        {
            return new AimSettings(MinPower, MaxPower, baseSettings.MaxInputDistance, baseSettings.MinInputDistance, baseSettings.FallbackDirection);
        }

        public ProjectileDefinition CreateProjectileDefinition()
        {
            return CreateProjectileDefinition(Damage);
        }

        public ProjectileDefinition CreateProjectileDefinition(
            int damageOverride,
            float knockbackMultiplier = 1f,
            int terrainBounceLimit = 0,
            float timeoutSecondsOverride = -1f)
        {
            return ProjectileDefinition.CreateRuntime(
                $"projectile.{_weaponId}",
                ProjectileRadius,
                GravityScale,
                timeoutSecondsOverride > 0f ? timeoutSecondsOverride : TimeoutSeconds,
                UnityEngine.Mathf.Max(0, damageOverride),
                ImpactRadius,
                knockbackMultiplier,
                terrainBounceLimit);
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
            string[] tags,
            string descriptionKey = "",
            string iconId = "",
            string vfxId = "",
            string sfxId = "")
        {
            var definition = CreateInstance<WeaponDefinition>();
            definition._weaponId = weaponId;
            definition._displayKey = displayKey;
            definition._descriptionKey = string.IsNullOrWhiteSpace(descriptionKey)
                ? $"{weaponId}.description"
                : descriptionKey;
            definition._role = role;
            definition._iconId = string.IsNullOrWhiteSpace(iconId) ? $"{weaponId}.icon" : iconId;
            definition._vfxId = string.IsNullOrWhiteSpace(vfxId) ? $"{weaponId}.vfx" : vfxId;
            definition._sfxId = string.IsNullOrWhiteSpace(sfxId) ? $"{weaponId}.sfx" : sfxId;
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
