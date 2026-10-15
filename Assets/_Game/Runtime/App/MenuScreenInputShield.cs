using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Stops the persistent main-menu canvas from receiving taps through a screen
    /// opened above it. The shield is deliberately the first sibling so all screen
    /// controls created afterwards remain interactive.
    /// </summary>
    public static class MenuScreenInputShield
    {
        private static Sprite _sprite;

        public static RectTransform Create(Transform parent, string name)
        {
            var objectRoot = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)objectRoot.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
            rect.SetAsFirstSibling();

            var image = objectRoot.AddComponent<Image>();
            image.sprite = GetSprite();
            image.color = new Color(0f, 0f, 0f, 0f);
            image.raycastTarget = true;
            return rect;
        }

        private static Sprite GetSprite()
        {
            if (_sprite != null)
            {
                return _sprite;
            }

            _sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            return _sprite;
        }
    }
}
