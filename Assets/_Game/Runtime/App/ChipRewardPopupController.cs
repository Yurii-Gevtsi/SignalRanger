using System.Collections;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Gameplay.Battle.Weapons;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Reusable "chip obtained" popup. Shown whenever the player is granted a
    /// weapon blueprint chip, regardless of the source (daily login, quests,
    /// battle loot) - callers only need <see cref="Show"/>. Requests queue so
    /// simultaneous grants never overlap two cards on screen at once.
    /// </summary>
    public sealed class ChipRewardPopupController : MonoBehaviour
    {
        private readonly struct Request
        {
            public Request(string weaponId, int amount, int balanceBefore, int balanceAfter, int requiredForNextRank)
            {
                WeaponId = weaponId;
                Amount = amount;
                BalanceBefore = balanceBefore;
                BalanceAfter = balanceAfter;
                RequiredForNextRank = requiredForNextRank;
            }

            public string WeaponId { get; }
            public int Amount { get; }
            public int BalanceBefore { get; }
            public int BalanceAfter { get; }
            public int RequiredForNextRank { get; }
        }

        private const string ShopResourceRoot = "UI/Shop/";
        private const float CountUpDelaySeconds = 2f;
        private const float CountUpDurationSeconds = 1.2f;

        private static readonly Queue<Request> PendingRequests = new();
        private static ChipRewardPopupController _activeInstance;

        private static readonly Vector2 CardSize = new(640f, 760f);
        private static readonly Vector2 WeaponIconSize = new(360f, 360f);
        private static readonly Vector2 RowIconSize = new(70f, 70f);

        private readonly Dictionary<string, Sprite> _sprites = new();
        private Sprite _whiteSprite;
        private GameObject _overlay;
        private TMP_Text _balanceText;
        private bool _isShowing;

        public static void Show(string weaponId, int amount, int balanceBefore, int balanceAfter, int requiredForNextRank)
        {
            if (string.IsNullOrWhiteSpace(weaponId) || amount <= 0)
            {
                return;
            }

            PendingRequests.Enqueue(new Request(weaponId, amount, balanceBefore, balanceAfter, requiredForNextRank));

            if (_activeInstance == null)
            {
                var root = new GameObject("Chip Reward Popup");
                _activeInstance = root.AddComponent<ChipRewardPopupController>();
            }

            _activeInstance.TryProcessNext();
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        }

        private void TryProcessNext()
        {
            if (_isShowing || PendingRequests.Count == 0)
            {
                return;
            }

            _isShowing = true;
            var request = PendingRequests.Dequeue();
            Build(request);
        }

        private void Build(Request request)
        {
            var canvasObject = new GameObject("Chip Reward Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 220;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            _overlay = CreatePanel("Chip Reward Overlay", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.55f)).gameObject;
            var overlayButton = _overlay.AddComponent<Button>();
            overlayButton.targetGraphic = _overlay.GetComponent<Image>();
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(Dismiss);

            var card = CreateImage("Chip Reward Card", _overlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), CardSize, Vector2.zero, "UI/Common/rect_card", false, Color.white);
            var cardImage = card.GetComponent<Image>();
            cardImage.raycastTarget = true;
            card.gameObject.AddComponent<Button>().onClick.AddListener(() => { });

            CreateText("Chip Reward Title", card, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(560f, 70f), new Vector2(0f, -60f), 40,
                TextAlignmentOptions.Center, GameText.ChipRewardTitle(), GameTextRole.DisplayTitle);

            var weapon = ResolveWeapon(request.WeaponId);
            var weaponIconId = weapon != null
                ? ResolveCurrentWeaponIconId(weapon.WeaponId, weapon.IconId)
                : string.Empty;
            CreateImage("Chip Reward Weapon Icon", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), WeaponIconSize, new Vector2(0f, 60f),
                string.IsNullOrWhiteSpace(weaponIconId) ? string.Empty : "UI/Weapons/" + weaponIconId, true, Color.white);

            var weaponName = weapon != null ? WeaponDisplayNames.Get(weapon.WeaponId) : string.Empty;
            CreateText("Chip Reward Weapon Name", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(560f, 60f), new Vector2(0f, -150f), 32,
                TextAlignmentOptions.Center, weaponName, GameTextRole.Body);

            var row = CreateRect("Chip Reward Row", card, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(560f, 100f), new Vector2(0f, 80f));

            CreateImage("Chip Reward Gain Icon", row, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), RowIconSize, new Vector2(20f, 0f), "UI/Common/chip", true, Color.white);
            CreateText("Chip Reward Gain Value", row, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), new Vector2(140f, 70f), new Vector2(100f, 0f), 40,
                TextAlignmentOptions.MidlineLeft, $"+{request.Amount}", GameTextRole.CurrencyGold);

            _balanceText = CreateText("Chip Reward Balance Value", row, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f),
                new Vector2(1f, 0.5f), new Vector2(260f, 70f), new Vector2(-20f, 0f), 40,
                TextAlignmentOptions.MidlineRight, $"{request.BalanceBefore}/{request.RequiredForNextRank}", GameTextRole.Numeric);
            _balanceText.color = SciFiTextStyle.GoldText;

            var closeRect = CreateRect("Chip Reward Close", card, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 0.5f), new Vector2(110f, 110f), new Vector2(-70f, -70f));
            var closeHit = closeRect.gameObject.AddComponent<Image>();
            closeHit.sprite = _whiteSprite;
            closeHit.color = new Color(1f, 1f, 1f, 0f);
            closeHit.raycastTarget = true;
            var closeButton = closeRect.gameObject.AddComponent<Button>();
            closeButton.targetGraphic = closeHit;
            closeButton.transition = Selectable.Transition.None;
            closeButton.onClick.AddListener(Dismiss);
            var closeLabel = CreateText("Chip Reward Close Label", closeRect, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 64, TextAlignmentOptions.Center,
                "×", GameTextRole.DisplayTitle);
            closeLabel.color = new Color(1f, 0.18f, 0.16f, 1f);

            StartCoroutine(AnimateBalance(request));
        }

        private IEnumerator AnimateBalance(Request request)
        {
            yield return new WaitForSeconds(CountUpDelaySeconds);

            var elapsed = 0f;
            while (elapsed < CountUpDurationSeconds)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / CountUpDurationSeconds);
                var current = Mathf.RoundToInt(Mathf.Lerp(request.BalanceBefore, request.BalanceAfter, t));
                ApplyBalance(current, request.RequiredForNextRank);
                yield return null;
            }

            ApplyBalance(request.BalanceAfter, request.RequiredForNextRank);
        }

        private void ApplyBalance(int current, int required)
        {
            if (_balanceText == null)
            {
                return;
            }

            _balanceText.text = $"{current}/{required}";
            _balanceText.color = current >= required ? SciFiTextStyle.SuccessText : SciFiTextStyle.GoldText;
        }

        private void Dismiss()
        {
            if (_overlay != null)
            {
                Destroy(_overlay.transform.parent.gameObject);
            }

            _isShowing = false;
            _balanceText = null;

            if (PendingRequests.Count > 0)
            {
                TryProcessNext();
            }
            else
            {
                _activeInstance = null;
                Destroy(gameObject);
            }
        }

        private static WeaponDefinition ResolveWeapon(string weaponId)
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            for (var i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null && weapons[i].WeaponId == weaponId)
                {
                    return weapons[i];
                }
            }

            return null;
        }

        private static string ResolveCurrentWeaponIconId(string weaponId, string baseIconId)
        {
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weaponId);
            var resource = WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                baseIconId, progress.rank, (WeaponUpgradeBranch)progress.branch);
            const string prefix = "UI/Weapons/";
            return resource.StartsWith(prefix, System.StringComparison.Ordinal)
                ? resource.Substring(prefix.Length)
                : baseIconId;
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = color;
            image.raycastTarget = true;
            return image;
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect, Color tint)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName);
            image.color = tint;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value,
            GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;
            text.raycastTarget = false;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0.02f, 0.03f, 0.05f, 0.9f);
            shadow.effectDistance = new Vector2(0f, -2f);
            return text;
        }

        private Sprite LoadSprite(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                return _whiteSprite;
            }

            if (_sprites.TryGetValue(resourceName, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourceName);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
            _sprites[resourceName] = sprite;
            return sprite;
        }

        private static RectTransform CreateRect(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition)
        {
            var rectObject = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)rectObject.transform;
            rect.SetParent(parent.transform, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
            return rect;
        }
    }
}
