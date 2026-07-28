using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Game.EditorTools.Characters
{
    public static class SigmaRigBuilder
    {
        private const string ArtFolder = "Assets/_Game/Art/Characters/Sigma";
        private const string SourceFolder = ArtFolder + "/Source";
        private const string SourceBodyPath = SourceFolder + "/body.png";
        private const string BodyTexturePath = ArtFolder + "/body.png";
        private const string PrefabPath = "Assets/_Game/Content/Resources/Visuals/Characters/SigmaRig.prefab";
        private const string IdleClipPath = ArtFolder + "/SigmaIdle.anim";
        private const string MoveClipPath = ArtFolder + "/SigmaMove.anim";
        private const string ControllerPath = ArtFolder + "/SigmaRig.controller";
        private const float PixelsPerUnit = 100f;
        private const float PlatformFrameSeconds = 0.15f;

        private static readonly string[] SourcePlatformPaths =
        {
            SourceFolder + "/platform_1.png",
            SourceFolder + "/platform_2.png",
            SourceFolder + "/platform_3.png"
        };

        private static readonly string[] PlatformTexturePaths =
        {
            ArtFolder + "/platform_1.png",
            ArtFolder + "/platform_2.png",
            ArtFolder + "/platform_3.png"
        };

        [MenuItem("Game/Characters/Build Sigma Rig")]
        public static void Build()
        {
            ConfigureSprite(SourceBodyPath, true);
            foreach (var sourcePath in SourcePlatformPaths)
            {
                ConfigureSprite(sourcePath, true);
            }

            CreateTransparentTexture(SourceBodyPath, BodyTexturePath);
            for (var i = 0; i < SourcePlatformPaths.Length; i++)
            {
                CreateTransparentTexture(SourcePlatformPaths[i], PlatformTexturePaths[i]);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            ConfigureSprite(BodyTexturePath, false);
            foreach (var platformPath in PlatformTexturePaths)
            {
                ConfigureSprite(platformPath, false);
            }

            var frames = LoadPlatformFrames();
            var idle = BuildIdleClip(frames[0]);
            var move = BuildMoveClip(frames);
            var controller = BuildController(idle, move);
            BuildPrefab(controller, frames[0]);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Sigma rig rebuilt at {PrefabPath}.");
        }

        private static void ConfigureSprite(string assetPath, bool readable)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError($"Texture not found: {assetPath}");
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = PixelsPerUnit;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.isReadable = readable;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 2048;
            importer.SaveAndReimport();
        }

        private static void CreateTransparentTexture(string sourcePath, string outputPath)
        {
            var source = AssetDatabase.LoadAssetAtPath<Texture2D>(sourcePath);
            if (source == null || !source.isReadable)
            {
                throw new InvalidDataException($"Readable Sigma source is missing: {sourcePath}");
            }

            var pixels = source.GetPixels32();
            var output = (Color32[])pixels.Clone();
            var visited = new bool[pixels.Length];
            var queue = new Queue<int>();

            for (var x = 0; x < source.width; x++)
            {
                TryEnqueue(x, 0, source, pixels, visited, queue);
                TryEnqueue(x, source.height - 1, source, pixels, visited, queue);
            }

            for (var y = 1; y < source.height - 1; y++)
            {
                TryEnqueue(0, y, source, pixels, visited, queue);
                TryEnqueue(source.width - 1, y, source, pixels, visited, queue);
            }

            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                output[index] = new Color32(255, 255, 255, 0);
                var x = index % source.width;
                var y = index / source.width;
                TryEnqueue(x - 1, y, source, pixels, visited, queue);
                TryEnqueue(x + 1, y, source, pixels, visited, queue);
                TryEnqueue(x, y - 1, source, pixels, visited, queue);
                TryEnqueue(x, y + 1, source, pixels, visited, queue);
            }

            var texture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            try
            {
                texture.SetPixels32(output);
                texture.Apply(false, false);
                File.WriteAllBytes(ToAbsolutePath(outputPath), texture.EncodeToPNG());
            }
            finally
            {
                Object.DestroyImmediate(texture);
            }
        }

        private static void TryEnqueue(
            int x,
            int y,
            Texture2D texture,
            Color32[] pixels,
            bool[] visited,
            Queue<int> queue)
        {
            if (x < 0 || y < 0 || x >= texture.width || y >= texture.height)
            {
                return;
            }

            var index = y * texture.width + x;
            if (visited[index])
            {
                return;
            }

            visited[index] = true;
            if (IsMagentaBackground(pixels[index]))
            {
                queue.Enqueue(index);
            }
        }

        private static bool IsMagentaBackground(Color32 pixel)
        {
            return pixel.r > 150 && pixel.b > 150 && pixel.g < 110
                && pixel.r + pixel.b > pixel.g * 4;
        }

        private static Sprite[] LoadPlatformFrames()
        {
            var frames = new Sprite[PlatformTexturePaths.Length];
            for (var i = 0; i < PlatformTexturePaths.Length; i++)
            {
                frames[i] = AssetDatabase.LoadAssetAtPath<Sprite>(PlatformTexturePaths[i]);
                if (frames[i] == null)
                {
                    throw new InvalidDataException($"Sigma platform frame failed to import: {PlatformTexturePaths[i]}");
                }
            }

            return frames;
        }

        private static AnimationClip BuildIdleClip(Sprite frame)
        {
            var clip = new AnimationClip { frameRate = 8f, name = "SigmaIdle" };
            SetPlatformFrames(clip, new[] { new ObjectReferenceKeyframe { time = 0f, value = frame } });
            SaveClip(clip, IdleClipPath, false);
            return clip;
        }

        private static AnimationClip BuildMoveClip(IReadOnlyList<Sprite> frames)
        {
            var clip = new AnimationClip { frameRate = 8f, name = "SigmaMove" };
            var keys = new ObjectReferenceKeyframe[frames.Count + 1];
            for (var i = 0; i < frames.Count; i++)
            {
                keys[i] = new ObjectReferenceKeyframe { time = i * PlatformFrameSeconds, value = frames[i] };
            }

            keys[^1] = new ObjectReferenceKeyframe { time = frames.Count * PlatformFrameSeconds, value = frames[0] };
            SetPlatformFrames(clip, keys);
            SaveClip(clip, MoveClipPath, true);
            return clip;
        }

        private static void SetPlatformFrames(AnimationClip clip, ObjectReferenceKeyframe[] keys)
        {
            var binding = EditorCurveBinding.PPtrCurve("Platform", typeof(SpriteRenderer), "m_Sprite");
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
        }

        private static void SaveClip(AnimationClip clip, string path, bool loop)
        {
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(clip, path);
        }

        private static AnimatorController BuildController(AnimationClip idle, AnimationClip move)
        {
            AssetDatabase.DeleteAsset(ControllerPath);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            controller.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);
            var machine = controller.layers[0].stateMachine;
            var idleState = machine.AddState("Idle");
            idleState.motion = idle;
            var moveState = machine.AddState("Move");
            moveState.motion = move;
            machine.defaultState = idleState;

            var toMove = idleState.AddTransition(moveState);
            toMove.hasExitTime = false;
            toMove.duration = 0f;
            toMove.AddCondition(AnimatorConditionMode.If, 0f, "IsMoving");
            var toIdle = moveState.AddTransition(idleState);
            toIdle.hasExitTime = false;
            toIdle.duration = 0f;
            toIdle.AddCondition(AnimatorConditionMode.IfNot, 0f, "IsMoving");
            return controller;
        }

        private static void BuildPrefab(RuntimeAnimatorController controller, Sprite idlePlatform)
        {
            var bodySprite = AssetDatabase.LoadAssetAtPath<Sprite>(BodyTexturePath);
            if (bodySprite == null)
            {
                throw new InvalidDataException("Sigma body failed to import.");
            }

            var root = new GameObject("SigmaRig");
            try
            {
                var platform = CreateChild(root.transform, "Platform", new Vector3(0f, -1.82f, 0f));
                platform.localScale = new Vector3(0.9f, 0.9f, 1f);
                AddSprite(platform, idlePlatform, 19);

                var body = CreateChild(root.transform, "Body", new Vector3(0f, 1.8f, 0f));
                body.localScale = new Vector3(0.9f, 0.9f, 1f);
                AddSprite(body, bodySprite, 21);

                var animator = root.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
                animator.applyRootMotion = false;
                PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static Transform CreateChild(Transform parent, string name, Vector3 position)
        {
            var child = new GameObject(name).transform;
            child.SetParent(parent, false);
            child.localPosition = position;
            return child;
        }

        private static void AddSprite(Transform target, Sprite sprite, int sortingOrder)
        {
            var renderer = target.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
        }

        private static string ToAbsolutePath(string assetPath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName
                ?? throw new DirectoryNotFoundException("Unity project root was not found.");
            return Path.Combine(projectRoot, assetPath.Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
