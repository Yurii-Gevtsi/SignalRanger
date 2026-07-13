using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Game.EditorTools.Arenas
{
    /// <summary>Builds the ocean kill-zone prefab and its sprite-only looping animations.</summary>
    public static class OceanKillZoneAssetBuilder
    {
        private const string Root = "Assets/_Game/Content/Resources/Visuals/Arenas/KillZones/Ocean";
        private const string WavesPath = Root + "/Sprites/Waves";
        private const string WavesClipPath = Root + "/Animations/Ocean_Waves_Loop.anim";
        private const string WavesControllerPath = Root + "/Controllers/Ocean_Waves.controller";
        private const string PrefabPath = Root + "/Prefabs/KillZone_Ocean.prefab";
        private const float PixelsPerUnit = 100f;
        private const float WavesLoopDuration = 5f;
        // The wave crest is intentionally aligned to the lower edge of the
        // widest supported camera view (size 7.0) for the training arena.
        private static readonly Vector3 RootPosition = new Vector3(0f, -10.5f, 0f);
        private const int WavesOrder = -4;

        [MenuItem("Game/Arenas/Build Ocean Kill-Zone")]
        public static void Build()
        {
            foreach (var path in SpritePaths(WavesPath))
            {
                ConfigureImporter(path, SpriteAlignment.BottomCenter, new Vector2(0.5f, 0f));
            }

            var waves = LoadSprites(WavesPath, 7);
            if (waves.Length != 7)
            {
                Debug.LogError("Ocean kill-zone needs seven wave frames.");
                return;
            }

            var wavesClip = CreateSpriteLoop(WavesClipPath, waves, "Ocean_Waves_Loop", WavesLoopDuration);
            var wavesController = CreateController(WavesControllerPath, wavesClip, "Ocean_Waves_Loop");
            CreatePrefab(waves[0], wavesController);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static IEnumerable<string> SpritePaths(string folder)
        {
            return Directory.GetFiles(folder, "*.png")
                .Select(path => path.Replace('\\', '/'))
                .OrderBy(path => path, System.StringComparer.Ordinal);
        }

        private static Sprite[] LoadSprites(string folder, int maximumFrames = int.MaxValue)
        {
            return SpritePaths(folder)
                .Select(AssetDatabase.LoadAssetAtPath<Sprite>)
                .Where(sprite => sprite != null)
                .Take(maximumFrames)
                .ToArray();
        }

        private static void ConfigureImporter(string path, SpriteAlignment alignment, Vector2 pivot)
        {
            if (!(AssetImporter.GetAtPath(path) is TextureImporter importer))
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = PixelsPerUnit;
            importer.mipmapEnabled = false;
            importer.isReadable = false;
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 2048;

            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)alignment;
            settings.spritePivot = pivot;
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
        }

        private static AnimationClip CreateSpriteLoop(string path, Sprite[] frames, string clipName, float duration)
        {
            var clip = new AnimationClip { name = clipName, frameRate = 60f };
            var keyframes = new List<ObjectReferenceKeyframe>();
            for (var index = 0; index < frames.Length; index++)
            {
                keyframes.Add(new ObjectReferenceKeyframe
                {
                    time = index * duration / frames.Length,
                    value = frames[index]
                });
            }

            keyframes.Add(new ObjectReferenceKeyframe { time = duration, value = frames[0] });
            AnimationUtility.SetObjectReferenceCurve(
                clip,
                EditorCurveBinding.PPtrCurve(string.Empty, typeof(SpriteRenderer), "m_Sprite"),
                keyframes.ToArray());

            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(clip, path);
            return clip;
        }

        private static AnimatorController CreateController(string path, AnimationClip clip, string stateName)
        {
            AssetDatabase.DeleteAsset(path);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            var state = controller.layers[0].stateMachine.AddState(stateName);
            state.motion = clip;
            controller.layers[0].stateMachine.defaultState = state;
            return controller;
        }

        private static void CreatePrefab(Sprite waveSprite, RuntimeAnimatorController wavesController)
        {
            var root = new GameObject("KillZone_Ocean");
            try
            {
                root.transform.position = RootPosition;
                var visuals = CreateChild(root.transform, "Visuals", Vector3.zero);
                var waves = AddSprite(CreateChild(visuals, "SurfaceWaves", Vector3.zero), waveSprite, WavesOrder, Color.white);
                waves.gameObject.AddComponent<Animator>().runtimeAnimatorController = wavesController;
                PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static Transform CreateChild(Transform parent, string name, Vector3 localPosition)
        {
            var child = new GameObject(name).transform;
            child.SetParent(parent, false);
            child.localPosition = localPosition;
            return child;
        }

        private static SpriteRenderer AddSprite(Transform target, Sprite sprite, int sortingOrder, Color color)
        {
            var renderer = target.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
            renderer.color = color;
            return renderer;
        }
    }
}
