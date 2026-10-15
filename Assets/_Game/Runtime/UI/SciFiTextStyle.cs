using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using System.Collections.Generic;

namespace Game.UI
{
    public static class SciFiTextStyle
    {
        public const string BodyFontResourcePath = "Fonts/Exo2 SDF";
        public const string HeadingFontResourcePath = "Fonts/Orbitron SDF";
        public const string BodyFontSourcePath = "Fonts/Exo2";
        public const string HeadingFontSourcePath = "Fonts/Orbitron";

        private static readonly string[] FallbackFontSourcePaths =
        {
            "Fonts/NotoSansArabic-Regular",
            "Fonts/NotoSansCJKjp-Regular",
            "Fonts/NotoSansCJKkr-Regular",
            "Fonts/NotoSansCJKsc-Regular"
        };

        public static readonly Color MainText = new Color32(0xF2, 0xF7, 0xFF, 0xFF);
        public static readonly Color SecondaryText = new Color32(0xA9, 0xC7, 0xDA, 0xFF);
        public static readonly Color CyanText = new Color32(0x7A, 0xEC, 0xFF, 0xFF);
        public static readonly Color GoldText = new Color32(0xFF, 0xD1, 0x4A, 0xFF);
        public static readonly Color CrystalText = new Color32(0x34, 0xDF, 0xFF, 0xFF);
        public static readonly Color SuccessText = new Color32(0x42, 0xF5, 0x7B, 0xFF);
        public static readonly Color WarningText = new Color32(0xFF, 0x4D, 0x5A, 0xFF);
        public static readonly Color DisabledText = new Color32(0x66, 0x78, 0x8A, 0xFF);
        public static readonly Color ShadowText = new Color(0.02f, 0.03f, 0.05f, 0.88f);

        private static TMP_FontAsset _bodyFont;
        private static TMP_FontAsset _headingFont;

        public static TMP_FontAsset BodyFont
        {
            get
            {
                _bodyFont ??= LoadFontAsset(BodyFontResourcePath, BodyFontSourcePath);
                AddFallbackFonts(_bodyFont);
                return _bodyFont;
            }
        }

        public static TMP_FontAsset HeadingFont
        {
            get
            {
                _headingFont ??= LoadFontAsset(HeadingFontResourcePath, HeadingFontSourcePath);
                AddFallbackFonts(_headingFont);
                return _headingFont != null ? _headingFont : BodyFont;
            }
        }

        private static readonly List<TMP_FontAsset> _fallbackFonts = new();
        private static bool _fallbackFontsLoaded;

        public static void Apply(
            TMP_Text text,
            GameTextRole role = GameTextRole.Body,
            int? sizeOverride = null,
            TextAlignmentOptions? alignment = null)
        {
            if (text == null)
            {
                return;
            }

            var font = FontFor(role);
            if (font != null)
            {
                text.font = font;
            }

            var size = sizeOverride ?? SizeFor(role);
            text.fontSize = size;
            text.enableAutoSizing = true;
            text.fontSizeMin = Mathf.Max(12, Mathf.RoundToInt(size * 0.68f));
            text.fontSizeMax = size;
            text.fontStyle = FontStyleFor(role);
            text.color = ColorFor(role);
            text.characterSpacing = 0f;
            text.wordSpacing = 0f;
            text.textWrappingMode = role == GameTextRole.Button || role == GameTextRole.Numeric
                ? TextWrappingModes.NoWrap
                : TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;
            text.raycastTarget = false;

            if (alignment.HasValue)
            {
                text.alignment = alignment.Value;
            }
        }

        public static Color ColorFor(GameTextRole role)
        {
            return role switch
            {
                GameTextRole.Caption => SecondaryText,
                GameTextRole.Numeric => GoldText,
                GameTextRole.CurrencyGold => GoldText,
                GameTextRole.CurrencyCrystal => CrystalText,
                GameTextRole.Success => SuccessText,
                GameTextRole.Warning => WarningText,
                GameTextRole.Disabled => DisabledText,
                GameTextRole.DisplayTitle => GoldText,
                GameTextRole.ScreenTitle => MainText,
                _ => MainText
            };
        }

        public static int SizeFor(GameTextRole role)
        {
            return role switch
            {
                GameTextRole.DisplayTitle => 56,
                GameTextRole.ScreenTitle => 42,
                GameTextRole.Button => 30,
                GameTextRole.Caption => 22,
                GameTextRole.Numeric => 34,
                GameTextRole.CurrencyGold => 30,
                GameTextRole.CurrencyCrystal => 30,
                _ => 26
            };
        }

        private static TMP_FontAsset FontFor(GameTextRole role)
        {
            return role == GameTextRole.DisplayTitle || role == GameTextRole.ScreenTitle
                ? HeadingFont
                : BodyFont;
        }

        private static TMP_FontAsset LoadFontAsset(string tmpResourcePath, string sourceFontResourcePath)
        {
            var asset = Resources.Load<TMP_FontAsset>(tmpResourcePath);
            if (asset != null)
            {
                return asset;
            }

            var sourceFont = Resources.Load<Font>(sourceFontResourcePath);
            return sourceFont != null ? TMP_FontAsset.CreateFontAsset(sourceFont) : null;
        }

        private static void AddFallbackFonts(TMP_FontAsset fontAsset)
        {
            if (fontAsset == null)
            {
                return;
            }

            LoadFallbackFonts();
            if (fontAsset.fallbackFontAssetTable == null)
            {
                fontAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
            }

            foreach (var fallback in _fallbackFonts)
            {
                if (!fontAsset.fallbackFontAssetTable.Contains(fallback))
                {
                    fontAsset.fallbackFontAssetTable.Add(fallback);
                }
            }
        }

        private static void LoadFallbackFonts()
        {
            if (_fallbackFontsLoaded)
            {
                return;
            }

            _fallbackFontsLoaded = true;
            foreach (var sourcePath in FallbackFontSourcePaths)
            {
                var sourceFont = Resources.Load<Font>(sourcePath);
                if (sourceFont == null)
                {
                    Debug.LogWarning($"[Fonts] Fallback font not found at Resources/{sourcePath}.");
                    continue;
                }

                var fallback = TMP_FontAsset.CreateFontAsset(sourceFont, 90, 9, GlyphRenderMode.SDFAA,
                    512, 512, AtlasPopulationMode.Dynamic, true);
                if (fallback != null)
                {
                    _fallbackFonts.Add(fallback);
                }
            }
        }

        private static FontStyles FontStyleFor(GameTextRole role)
        {
            return role switch
            {
                GameTextRole.Body => FontStyles.Normal,
                GameTextRole.Caption => FontStyles.Normal,
                GameTextRole.Disabled => FontStyles.Normal,
                _ => FontStyles.Bold
            };
        }
    }
}
