using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.EditorTools.Art
{
    internal enum Unity2DArtImportProfile
    {
        None,
        GeneratedUiTexture,
        GeneratedSprite,
        ArenaVisual,
        ArenaMask
    }

    internal static class Unity2DArtImporterAutomation
    {
        private const string ArenaRoot = "Assets/_Game/Content/Art/Arenas/";
        private const string RuntimeArenaRoot = "Assets/_Game/Content/Resources/Visuals/Arenas/DesertSciFi/";
        private const string GeneratedUiRoot = "Assets/_Game/Content/Resources/UI/Generated/";
        private const string SupportDropBeamPath = "Assets/_Game/Content/Resources/UI/Battle/resource_drop_beam.png";
        private const string GeneratedArtRoot = "Assets/_Game/Content/Art/Generated/";
        private const string GeneratedCharacterRoot = "Assets/_Game/Art/Characters/Generated/";

        private const int UiMaxTextureSize = 2048;
        private const int SpriteMaxTextureSize = 2048;
        private const int ArenaMaxTextureSize = 4096;
        private const float UiAndSpritePixelsPerUnit = 100f;
        private const float ArenaPixelsPerUnit = 256f;

        [MenuItem("Game/Art/Importers/Apply Profiles To Selected Textures")]
        private static void ApplyProfilesToSelection()
        {
            var changed = 0;
            var skipped = 0;
            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!TryGetProfile(assetPath, out var profile))
                {
                    skipped++;
                    continue;
                }

                if (AssetImporter.GetAtPath(assetPath) is not TextureImporter importer)
                {
                    skipped++;
                    continue;
                }

                ConfigureImporter(importer, profile);
                importer.SaveAndReimport();
                changed++;
            }

            Debug.Log($"Unity 2D art importer: changed {changed} managed texture(s), skipped {skipped}.");
        }

        [MenuItem("Game/Art/Importers/Validate Managed Art")]
        private static void ValidateManagedArt()
        {
            var paths = AssetDatabase.GetAllAssetPaths()
                .Where(IsTexturePath)
                .Where(path => TryGetProfile(path, out _))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var issues = ValidatePaths(paths);
            LogValidationResult("managed art", paths.Length, issues);
        }

        [MenuItem("Game/Art/Importers/Reimport Managed Art")]
        private static void ReimportManagedArt()
        {
            var paths = AssetDatabase.GetAllAssetPaths()
                .Where(IsTexturePath)
                .Where(path => TryGetProfile(path, out _))
                .ToArray();
            for (var i = 0; i < paths.Length; i++)
            {
                AssetDatabase.ImportAsset(paths[i], ImportAssetOptions.ForceUpdate);
            }

            Debug.Log($"Unity 2D art importer: reimported {paths.Length} managed texture(s).");
        }

        [MenuItem("Game/Art/Importers/Validate Selected Art")]
        private static void ValidateSelectedArt()
        {
            var paths = Selection.assetGUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(IsTexturePath)
                .ToArray();
            var issues = ValidatePaths(paths);
            LogValidationResult("selected art", paths.Length, issues);
        }

        internal static bool TryGetProfile(string assetPath, out Unity2DArtImportProfile profile)
        {
            profile = Unity2DArtImportProfile.None;
            if (!IsTexturePath(assetPath))
            {
                return false;
            }

            var normalizedPath = assetPath.Replace('\\', '/');
            var fileName = Path.GetFileNameWithoutExtension(normalizedPath);
            // This runtime Texture2D is created into a Sprite at runtime. It must
            // retain its RGB channels; Single Channel import turns the cyan beam black.
            if (string.Equals(normalizedPath, SupportDropBeamPath, StringComparison.OrdinalIgnoreCase))
            {
                profile = Unity2DArtImportProfile.GeneratedUiTexture;
                return true;
            }

            if (IsUnder(normalizedPath, GeneratedUiRoot))
            {
                profile = Unity2DArtImportProfile.GeneratedUiTexture;
                return true;
            }

            if (IsUnder(normalizedPath, GeneratedArtRoot) || IsUnder(normalizedPath, GeneratedCharacterRoot))
            {
                profile = Unity2DArtImportProfile.GeneratedSprite;
                return true;
            }

            var isManagedArena = IsUnder(normalizedPath, ArenaRoot) || IsUnder(normalizedPath, RuntimeArenaRoot);
            if (isManagedArena && fileName.EndsWith("_mask", StringComparison.OrdinalIgnoreCase))
            {
                profile = Unity2DArtImportProfile.ArenaMask;
                return true;
            }

            if (isManagedArena && fileName.EndsWith("_visual", StringComparison.OrdinalIgnoreCase))
            {
                profile = Unity2DArtImportProfile.ArenaVisual;
                return true;
            }

            return false;
        }

        internal static void ConfigureImporter(TextureImporter importer, Unity2DArtImportProfile profile)
        {
            switch (profile)
            {
                case Unity2DArtImportProfile.GeneratedUiTexture:
                    ConfigureUiTexture(importer);
                    break;
                case Unity2DArtImportProfile.GeneratedSprite:
                    ConfigureSprite(importer, UiAndSpritePixelsPerUnit, SpriteMaxTextureSize, false, true);
                    ConfigureAndroidCompression(importer, SpriteMaxTextureSize, TextureImporterFormat.ASTC_4x4);
                    break;
                case Unity2DArtImportProfile.ArenaVisual:
                    ConfigureSprite(importer, ArenaPixelsPerUnit, ArenaMaxTextureSize, true, true);
                    ConfigureAndroidCompression(importer, ArenaMaxTextureSize, TextureImporterFormat.ASTC_4x4);
                    break;
                case Unity2DArtImportProfile.ArenaMask:
                    ConfigureMask(importer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(profile), profile, "Unsupported art import profile.");
            }
        }

        private static void ConfigureUiTexture(TextureImporter importer)
        {
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = true;
            importer.alphaIsTransparency = true;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.maxTextureSize = UiMaxTextureSize;
            importer.textureCompression = TextureImporterCompression.Compressed;
            importer.compressionQuality = 100;
            importer.crunchedCompression = false;
            ConfigureAndroidCompression(importer, UiMaxTextureSize, TextureImporterFormat.ASTC_4x4);
        }

        private static void ConfigureSprite(
            TextureImporter importer,
            float pixelsPerUnit,
            int maxTextureSize,
            bool readable,
            bool alphaIsTransparency)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = pixelsPerUnit;
            importer.isReadable = readable;
            importer.sRGBTexture = true;
            importer.alphaIsTransparency = alphaIsTransparency;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.maxTextureSize = maxTextureSize;
            importer.textureCompression = TextureImporterCompression.Compressed;
            importer.compressionQuality = 100;
            importer.crunchedCompression = false;

            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Center;
            settings.spritePivot = new Vector2(0.5f, 0.5f);
            settings.spriteMeshType = (int)SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
        }

        private static void ConfigureMask(TextureImporter importer)
        {
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.sRGBTexture = false;
            importer.alphaIsTransparency = false;
            importer.isReadable = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.maxTextureSize = ArenaMaxTextureSize;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.compressionQuality = 100;
            importer.crunchedCompression = false;
            ConfigureAndroidCompression(importer, ArenaMaxTextureSize, TextureImporterFormat.RGBA32);
        }

        private static void ConfigureAndroidCompression(
            TextureImporter importer,
            int maxTextureSize,
            TextureImporterFormat format)
        {
            var android = importer.GetPlatformTextureSettings("Android");
            android.name = "Android";
            android.overridden = true;
            android.maxTextureSize = maxTextureSize;
            android.format = format;
            android.textureCompression = format == TextureImporterFormat.RGBA32
                ? TextureImporterCompression.Uncompressed
                : TextureImporterCompression.Compressed;
            android.compressionQuality = 100;
            android.crunchedCompression = false;
            importer.SetPlatformTextureSettings(android);
        }

        private static List<string> ValidatePaths(IEnumerable<string> paths)
        {
            var issues = new List<string>();
            foreach (var assetPath in paths)
            {
                if (!TryGetProfile(assetPath, out var profile))
                {
                    issues.Add($"{assetPath}: unmanaged path or unsupported filename");
                    continue;
                }

                if (!IsStableFileStem(Path.GetFileNameWithoutExtension(assetPath)))
                {
                    issues.Add($"{assetPath}: managed asset name must use lower-case letters, digits, and underscores");
                }

                if (AssetImporter.GetAtPath(assetPath) is not TextureImporter importer)
                {
                    issues.Add($"{assetPath}: TextureImporter missing");
                    continue;
                }

                ValidateImporter(assetPath, importer, profile, issues);
                if (profile == Unity2DArtImportProfile.ArenaVisual || profile == Unity2DArtImportProfile.ArenaMask)
                {
                    ValidateArenaPair(assetPath, profile, issues);
                }
            }

            return issues;
        }

        private static void ValidateImporter(
            string assetPath,
            TextureImporter importer,
            Unity2DArtImportProfile profile,
            ICollection<string> issues)
        {
            if (importer.mipmapEnabled)
            {
                issues.Add($"{assetPath}: mipmaps must be disabled");
            }

            if (importer.filterMode != (profile == Unity2DArtImportProfile.ArenaMask ? FilterMode.Point : FilterMode.Bilinear))
            {
                issues.Add($"{assetPath}: unexpected filter mode {importer.filterMode}");
            }

            if (importer.wrapMode != TextureWrapMode.Clamp)
            {
                issues.Add($"{assetPath}: wrap mode must be Clamp");
            }

            if (profile == Unity2DArtImportProfile.ArenaMask)
            {
                if (importer.textureType != TextureImporterType.Default || importer.sRGBTexture || !importer.isReadable
                    || importer.textureCompression != TextureImporterCompression.Uncompressed)
                {
                    issues.Add($"{assetPath}: mask profile mismatch (Default, linear, readable, uncompressed required)");
                }

                ValidateAndroidSettings(assetPath, importer, ArenaMaxTextureSize, TextureImporterFormat.RGBA32, issues);

                return;
            }

            if (profile == Unity2DArtImportProfile.GeneratedUiTexture)
            {
                if (importer.textureType != TextureImporterType.Default || !importer.sRGBTexture)
                {
                    issues.Add($"{assetPath}: generated UI must remain a sRGB Default Texture2D");
                }

                ValidateAndroidSettings(assetPath, importer, UiMaxTextureSize, TextureImporterFormat.ASTC_4x4, issues);

                return;
            }

            var expectedPpu = profile == Unity2DArtImportProfile.ArenaVisual
                ? ArenaPixelsPerUnit
                : UiAndSpritePixelsPerUnit;
            if (importer.textureType != TextureImporterType.Sprite
                || importer.spriteImportMode != SpriteImportMode.Single
                || Mathf.Abs(importer.spritePixelsPerUnit - expectedPpu) > 0.01f
                || !importer.sRGBTexture
                || profile == Unity2DArtImportProfile.ArenaVisual && !importer.isReadable)
            {
                issues.Add($"{assetPath}: sprite profile mismatch (PPU {expectedPpu}, Single, sRGB required)");
            }

            var maxTextureSize = profile == Unity2DArtImportProfile.ArenaVisual
                ? ArenaMaxTextureSize
                : SpriteMaxTextureSize;
            ValidateAndroidSettings(assetPath, importer, maxTextureSize, TextureImporterFormat.ASTC_4x4, issues);
        }

        private static void ValidateAndroidSettings(
            string assetPath,
            TextureImporter importer,
            int maxTextureSize,
            TextureImporterFormat expectedFormat,
            ICollection<string> issues)
        {
            var android = importer.GetPlatformTextureSettings("Android");
            if (!android.overridden || android.maxTextureSize != maxTextureSize || android.format != expectedFormat)
            {
                issues.Add($"{assetPath}: Android override must be {expectedFormat} at max {maxTextureSize}");
            }

            var expectedCompression = expectedFormat == TextureImporterFormat.RGBA32
                ? TextureImporterCompression.Uncompressed
                : TextureImporterCompression.Compressed;
            if (android.textureCompression != expectedCompression || android.crunchedCompression)
            {
                issues.Add($"{assetPath}: Android compression settings do not match the managed profile");
            }
        }

        private static void ValidateArenaPair(
            string assetPath,
            Unity2DArtImportProfile profile,
            ICollection<string> issues)
        {
            var extension = Path.GetExtension(assetPath);
            var pairPath = profile == Unity2DArtImportProfile.ArenaVisual
                ? assetPath.Substring(0, assetPath.Length - "_visual".Length - extension.Length) + "_mask" + extension
                : assetPath.Substring(0, assetPath.Length - "_mask".Length - extension.Length) + "_visual" + extension;
            if (!File.Exists(pairPath))
            {
                issues.Add($"{assetPath}: missing visual/mask pair {pairPath}");
                return;
            }

            var current = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            var pair = AssetDatabase.LoadAssetAtPath<Texture2D>(pairPath);
            if (current == null || pair == null)
            {
                issues.Add($"{assetPath}: pair could not be loaded for dimension validation");
                return;
            }

            if (current.width != pair.width || current.height != pair.height)
            {
                issues.Add($"{assetPath}: visual/mask dimensions differ ({current.width}x{current.height} vs {pair.width}x{pair.height})");
            }
        }

        private static void LogValidationResult(string scope, int assetCount, IReadOnlyCollection<string> issues)
        {
            if (issues.Count == 0)
            {
                Debug.Log($"Unity 2D art validation passed for {scope}: {assetCount} managed texture(s).");
                return;
            }

            Debug.LogWarning($"Unity 2D art validation found {issues.Count} issue(s) in {scope} ({assetCount} texture(s)).\n- {string.Join("\n- ", issues)}");
        }

        private static bool IsTexturePath(string assetPath)
        {
            var extension = Path.GetExtension(assetPath);
            return extension.Equals(".png", StringComparison.OrdinalIgnoreCase)
                || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
                || extension.Equals(".tga", StringComparison.OrdinalIgnoreCase)
                || extension.Equals(".psd", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsUnder(string assetPath, string root)
        {
            return assetPath.StartsWith(root, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsStableFileStem(string fileStem)
        {
            if (string.IsNullOrWhiteSpace(fileStem))
            {
                return false;
            }

            foreach (var character in fileStem)
            {
                if ((character >= 'a' && character <= 'z')
                    || (character >= '0' && character <= '9')
                    || character == '_')
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }

    internal sealed class Unity2DArtImporterPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            if (!Unity2DArtImporterAutomation.TryGetProfile(assetPath, out var profile)
                || assetImporter is not TextureImporter importer)
            {
                return;
            }

            Unity2DArtImporterAutomation.ConfigureImporter(importer, profile);
        }
    }
}
