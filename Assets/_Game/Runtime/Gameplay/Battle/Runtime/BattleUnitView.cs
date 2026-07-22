using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Gameplay.Battle.Runtime
{
    public sealed class BattleUnitView : MonoBehaviour
    {
        private static readonly int IsMovingParameter = Animator.StringToHash("IsMoving");
        private const float MinimumMovementAnimationSeconds = 0.5f;
        // The whole rig renders through one sorting group, so overlapping
        // units can never interleave body parts. The active unit's group sits
        // above idle units but below projectiles and impact effects (30+).
        private const int IdleSortingOrder = 20;
        private const int ActiveTurnSortingOrder = 27;
        private const float WeaponVisualScale = 0.085f;
        private const string HeadingFontResourcePath = "Fonts/Orbitron";
        private static readonly Vector3 WeaponMountPosition = new(-0.68f, 0.42f, 0f);
        private static readonly Vector3 HealthLabelPosition = new(0f, 1.48f, 0f);
        private static readonly Vector3 HealthLabelScale = new(0.20f, 0.20f, 1f);
        private static Sprite _solidWhiteSprite;
        private static Font _headingFont;

        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject activeIndicator;
        [SerializeField] private TextMesh healthLabel;
        [SerializeField] private Vector3 defeatedScale = new(0.9f, 0.35f, 1f);
        [SerializeField] private bool preserveSpriteColor;

        private Color _baseColor = Color.white;
        private Color _healthColor = Color.green;
        private SortingGroup _sortingGroup;
        private Vector3 _baseScale;
        private int _artFacingDirectionX = 1;
        private Transform _visualRoot;
        private Vector3 _visualRootBaseScale = Vector3.one;
        private Animator _locomotionAnimator;
        private Transform _weaponMount;
        private SpriteRenderer _weaponRenderer;
        private int _facingDirectionX = 1;
        private bool _hasMovingParameter;
        private bool _movementSignalledThisFrame;
        private float _movementAnimationSecondsRemaining;

        public Transform FirePoint => firePoint != null ? firePoint : transform;
        public Vector2 WeaponMuzzlePosition
        {
            get
            {
                if (_weaponMount == null || _weaponRenderer == null || !_weaponRenderer.enabled || _weaponRenderer.sprite == null)
                {
                    return FirePoint.position;
                }

                var muzzleLocalX = _weaponRenderer.sprite.bounds.extents.x;
                return _weaponMount.TransformPoint(new Vector3(muzzleLocalX, 0f, 0f));
            }
        }

        private void Awake()
        {
            EnsureReferences();
            _baseScale = transform.localScale;
            if (bodyRenderer != null)
            {
                _baseColor = bodyRenderer.color;
            }
        }

        public void SetColor(Color color)
        {
            EnsureReferences();
            _baseColor = preserveSpriteColor ? Color.white : color;
            if (bodyRenderer != null)
            {
                bodyRenderer.color = _baseColor;
            }
        }

        public void SetPreserveSpriteColor(bool preserve)
        {
            preserveSpriteColor = preserve;
            if (preserveSpriteColor && bodyRenderer != null)
            {
                _baseColor = Color.white;
                bodyRenderer.color = Color.white;
            }
        }

        public void SetHealthColor(Color color)
        {
            _healthColor = color;
            EnsureReferences();
            if (healthLabel != null)
            {
                healthLabel.color = _healthColor;
            }
        }

        public void SetHealthLabelPositionY(float positionY)
        {
            EnsureReferences();
            if (healthLabel != null)
            {
                var position = healthLabel.transform.localPosition;
                healthLabel.transform.localPosition = new Vector3(position.x, positionY, position.z);
            }
        }

        public void SetHealthLabelScale(float scale)
        {
            EnsureReferences();
            if (healthLabel != null)
            {
                var clampedScale = Mathf.Max(0.01f, scale);
                healthLabel.transform.localScale = new Vector3(clampedScale, clampedScale, 1f);
            }
        }

        /// <summary>Animated rig root; when set, facing flips its X scale instead of the sprite flipX.</summary>
        public void SetVisualRoot(Transform visualRoot)
        {
            _visualRoot = visualRoot;
            _visualRootBaseScale = visualRoot != null ? visualRoot.localScale : Vector3.one;
            EnsureScreenBackground(visualRoot);
            _locomotionAnimator = visualRoot != null ? visualRoot.GetComponentInChildren<Animator>() : null;
            _hasMovingParameter = HasAnimatorBoolParameter(_locomotionAnimator, IsMovingParameter);
            ForceMoving(false);
        }

        public void SetMoving(bool isMoving)
        {
            if (!isMoving)
            {
                return;
            }

            _movementSignalledThisFrame = true;
            _movementAnimationSecondsRemaining = Mathf.Max(
                _movementAnimationSecondsRemaining,
                MinimumMovementAnimationSeconds);
            ApplyMoving(true);
        }

        private void LateUpdate()
        {
            AdvanceMovementAnimation(Time.unscaledDeltaTime);
        }

        private void AdvanceMovementAnimation(float unscaledDeltaTime)
        {
            if (!_movementSignalledThisFrame)
            {
                _movementAnimationSecondsRemaining = Mathf.Max(
                    0f,
                    _movementAnimationSecondsRemaining - Mathf.Max(0f, unscaledDeltaTime));
            }

            ApplyMoving(_movementAnimationSecondsRemaining > 0f);
            _movementSignalledThisFrame = false;
        }

        /// <summary>Which way the source art looks; the flip in <see cref="SetFacing"/> is relative to it.</summary>
        public void SetArtFacing(int artFacingDirectionX)
        {
            _artFacingDirectionX = artFacingDirectionX >= 0 ? 1 : -1;
        }

        public void SetFacing(int facingDirectionX)
        {
            EnsureReferences();
            var sign = facingDirectionX >= 0 ? 1 : -1;
            _facingDirectionX = sign;
            if (firePoint != null)
            {
                firePoint.localPosition = new Vector3(0.45f * sign, 0.2f, 0f);
            }

            if (_visualRoot != null)
            {
                var flip = sign != _artFacingDirectionX ? -1f : 1f;
                _visualRoot.localScale = new Vector3(_visualRootBaseScale.x * flip, _visualRootBaseScale.y, _visualRootBaseScale.z);
            }
            else if (bodyRenderer != null)
            {
                bodyRenderer.flipX = sign != _artFacingDirectionX;
            }

            UpdateWeaponMountPosition();
        }

        public void SetEquippedWeapon(Sprite sprite, bool sourceFacesRight)
        {
            EnsureWeaponMount();
            _weaponRenderer.sprite = sprite;
            _weaponRenderer.flipX = !sourceFacesRight;
            _weaponRenderer.enabled = sprite != null;
            UpdateWeaponMountPosition();
        }

        public void ClearEquippedWeapon()
        {
            if (_weaponRenderer == null)
            {
                return;
            }

            _weaponRenderer.sprite = null;
            _weaponRenderer.enabled = false;
        }

        public void SetWeaponAim(Vector2 direction)
        {
            if (_weaponMount == null || _weaponRenderer == null || !_weaponRenderer.enabled || direction.sqrMagnitude < 0.0001f)
            {
                return;
            }

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _weaponMount.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void SetActiveTurn(bool isActive)
        {
            EnsureReferences();
            if (activeIndicator != null)
            {
                activeIndicator.SetActive(!preserveSpriteColor && isActive);
            }

            if (_sortingGroup != null)
            {
                _sortingGroup.sortingOrder = isActive ? ActiveTurnSortingOrder : IdleSortingOrder;
            }
        }

        public void SetHealth(int health, int maxHealth)
        {
            EnsureReferences();
            var healthRatio = maxHealth <= 0 ? 0f : Mathf.Clamp01((float)health / maxHealth);

            if (healthLabel != null)
            {
                healthLabel.text = health.ToString();
            }

            if (bodyRenderer != null && health > 0 && !preserveSpriteColor)
            {
                bodyRenderer.color = Color.Lerp(Color.red, _baseColor, healthRatio);
            }
        }

        public void SetDefeated(bool defeated)
        {
            EnsureReferences();
            transform.localScale = defeated ? defeatedScale : _baseScale;

            if (defeated)
            {
                ForceMoving(false);
            }

            if (bodyRenderer != null && defeated)
            {
                bodyRenderer.color = Color.gray;
            }
        }

        private void ForceMoving(bool isMoving)
        {
            _movementSignalledThisFrame = false;
            _movementAnimationSecondsRemaining = isMoving ? MinimumMovementAnimationSeconds : 0f;
            ApplyMoving(isMoving);
        }

        private void ApplyMoving(bool isMoving)
        {
            if (_hasMovingParameter && _locomotionAnimator != null)
            {
                _locomotionAnimator.SetBool(IsMovingParameter, isMoving);
            }
        }

        private static void EnsureScreenBackground(Transform visualRoot)
        {
            var screenRenderer = visualRoot?.Find("Body/Screen Background")?.GetComponent<SpriteRenderer>();
            if (screenRenderer == null || screenRenderer.sprite != null)
            {
                return;
            }

            if (_solidWhiteSprite == null)
            {
                _solidWhiteSprite = Sprite.Create(
                    Texture2D.whiteTexture,
                    new Rect(0f, 0f, 1f, 1f),
                    new Vector2(0.5f, 0.5f),
                    1f);
                _solidWhiteSprite.name = "Droid Screen Background";
                _solidWhiteSprite.hideFlags = HideFlags.HideAndDontSave;
            }

            screenRenderer.sprite = _solidWhiteSprite;
        }

        private static bool HasAnimatorBoolParameter(Animator animator, int parameterHash)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
            {
                return false;
            }

            foreach (var parameter in animator.parameters)
            {
                if (parameter.nameHash == parameterHash && parameter.type == AnimatorControllerParameterType.Bool)
                {
                    return true;
                }
            }

            return false;
        }

        private void EnsureWeaponMount()
        {
            if (_weaponMount != null && _weaponRenderer != null)
            {
                return;
            }

            var mountObject = new GameObject("Weapon Mount");
            _weaponMount = mountObject.transform;
            _weaponMount.SetParent(transform, false);
            _weaponMount.localScale = new Vector3(WeaponVisualScale, WeaponVisualScale, 1f);
            _weaponRenderer = mountObject.AddComponent<SpriteRenderer>();
            _weaponRenderer.sortingOrder = 24;
            _weaponRenderer.color = Color.white;
            _weaponRenderer.enabled = false;
            UpdateWeaponMountPosition();
        }

        private void UpdateWeaponMountPosition()
        {
            if (_weaponMount == null)
            {
                return;
            }

            _weaponMount.localPosition = new Vector3(
                WeaponMountPosition.x * _facingDirectionX,
                WeaponMountPosition.y,
                WeaponMountPosition.z);
        }

        private void EnsureReferences()
        {
            if (_sortingGroup == null)
            {
                _sortingGroup = GetComponent<SortingGroup>();
                if (_sortingGroup == null)
                {
                    _sortingGroup = gameObject.AddComponent<SortingGroup>();
                }

                _sortingGroup.sortingOrder = IdleSortingOrder;
            }

            if (bodyRenderer == null)
            {
                bodyRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (firePoint == null)
            {
                var firePointObject = new GameObject("Fire Point");
                firePoint = firePointObject.transform;
                firePoint.SetParent(transform);
                firePoint.localPosition = new Vector3(0.45f, 0.2f, 0f);
            }

            if (activeIndicator == null)
            {
                activeIndicator = new GameObject("Active Indicator");
                activeIndicator.transform.SetParent(transform);
                activeIndicator.transform.localPosition = new Vector3(0f, 0.72f, 0f);
                activeIndicator.transform.localScale = new Vector3(0.24f, 0.24f, 1f);

                var indicatorRenderer = activeIndicator.AddComponent<SpriteRenderer>();
                indicatorRenderer.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
                indicatorRenderer.color = Color.white;
                indicatorRenderer.sortingOrder = 30;
                activeIndicator.SetActive(false);
            }

            if (healthLabel == null)
            {
                var label = new GameObject("Health Label");
                label.transform.SetParent(transform);
                label.transform.localPosition = HealthLabelPosition;
                label.transform.localScale = HealthLabelScale;
                healthLabel = label.AddComponent<TextMesh>();
                healthLabel.alignment = TextAlignment.Center;
                healthLabel.anchor = TextAnchor.MiddleCenter;
                healthLabel.fontSize = 30;
                healthLabel.fontStyle = FontStyle.Bold;
                healthLabel.color = _healthColor;
                var labelRenderer = label.GetComponent<MeshRenderer>();
                if (labelRenderer != null)
                {
                    labelRenderer.sortingOrder = 33;
                }
            }

            if (healthLabel != null)
            {
                ApplyHealthLabelFont(healthLabel);
            }
        }

        private static void ApplyHealthLabelFont(TextMesh label)
        {
            _headingFont ??= Resources.Load<Font>(HeadingFontResourcePath);
            if (_headingFont == null)
            {
                return;
            }

            label.font = _headingFont;
            var renderer = label.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = _headingFont.material;
            }
        }
    }
}
