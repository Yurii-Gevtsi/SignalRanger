using UnityEngine;

namespace Game.Gameplay.Battle.Runtime
{
    public sealed class BattleUnitView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject activeIndicator;
        [SerializeField] private Transform healthFill;
        [SerializeField] private TextMesh healthLabel;
        [SerializeField] private Vector3 defeatedScale = new(0.9f, 0.35f, 1f);
        [SerializeField] private bool preserveSpriteColor;

        private Color _baseColor = Color.white;
        private Vector3 _baseScale;
        private int _artFacingDirectionX = 1;

        public Transform FirePoint => firePoint != null ? firePoint : transform;

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

        /// <summary>Which way the source art looks; the flip in <see cref="SetFacing"/> is relative to it.</summary>
        public void SetArtFacing(int artFacingDirectionX)
        {
            _artFacingDirectionX = artFacingDirectionX >= 0 ? 1 : -1;
        }

        public void SetFacing(int facingDirectionX)
        {
            EnsureReferences();
            var sign = facingDirectionX >= 0 ? 1 : -1;
            if (firePoint != null)
            {
                firePoint.localPosition = new Vector3(0.45f * sign, 0.2f, 0f);
            }

            if (bodyRenderer != null)
            {
                bodyRenderer.flipX = sign != _artFacingDirectionX;
            }
        }

        public void SetActiveTurn(bool isActive)
        {
            EnsureReferences();
            if (activeIndicator != null)
            {
                activeIndicator.SetActive(!preserveSpriteColor && isActive);
            }
        }

        public void SetHealth(int health, int maxHealth)
        {
            EnsureReferences();
            var healthRatio = maxHealth <= 0 ? 0f : Mathf.Clamp01((float)health / maxHealth);

            if (healthFill != null)
            {
                healthFill.localScale = new Vector3(healthRatio, 1f, 1f);
                healthFill.localPosition = new Vector3(-0.5f + healthRatio * 0.5f, healthFill.localPosition.y, healthFill.localPosition.z);
            }

            if (healthLabel != null)
            {
                healthLabel.text = $"{health}/{maxHealth}";
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

            if (bodyRenderer != null && defeated)
            {
                bodyRenderer.color = Color.gray;
            }
        }

        private void EnsureReferences()
        {
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

            if (healthFill == null)
            {
                var healthRoot = new GameObject("Health Bar");
                healthRoot.transform.SetParent(transform);
                healthRoot.transform.localPosition = new Vector3(0f, 0.98f, 0f);
                healthRoot.transform.localScale = new Vector3(0.9f, 0.08f, 1f);

                var background = new GameObject("Background");
                background.transform.SetParent(healthRoot.transform);
                background.transform.localPosition = Vector3.zero;
                background.transform.localScale = Vector3.one;
                var backgroundRenderer = background.AddComponent<SpriteRenderer>();
                backgroundRenderer.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
                backgroundRenderer.color = new Color(0.05f, 0.05f, 0.05f, 0.85f);
                backgroundRenderer.sortingOrder = 31;

                var fill = new GameObject("Fill");
                fill.transform.SetParent(healthRoot.transform);
                fill.transform.localPosition = Vector3.zero;
                fill.transform.localScale = Vector3.one;
                var fillRenderer = fill.AddComponent<SpriteRenderer>();
                fillRenderer.sprite = backgroundRenderer.sprite;
                fillRenderer.color = new Color(0.2f, 0.95f, 0.25f, 0.95f);
                fillRenderer.sortingOrder = 32;
                healthFill = fill.transform;

                var label = new GameObject("Health Label");
                label.transform.SetParent(healthRoot.transform);
                label.transform.localPosition = new Vector3(0f, 0.28f, 0f);
                label.transform.localScale = new Vector3(0.16f, 0.16f, 1f);
                healthLabel = label.AddComponent<TextMesh>();
                healthLabel.alignment = TextAlignment.Center;
                healthLabel.anchor = TextAnchor.MiddleCenter;
                healthLabel.fontSize = 38;
                healthLabel.color = Color.white;
                var labelRenderer = label.GetComponent<MeshRenderer>();
                if (labelRenderer != null)
                {
                    labelRenderer.sortingOrder = 33;
                }
            }
        }
    }
}
