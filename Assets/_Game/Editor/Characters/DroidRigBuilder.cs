using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Game.EditorTools.Characters
{
    /// <summary>
    /// Imports the layered battle droid, removes the baked checkerboard from the
    /// supplied platform frames, and rebuilds its prefab and locomotion clips.
    /// </summary>
    public static class DroidRigBuilder
    {
        private const string ArtFolder = "Assets/_Game/Art/Characters/Droid";
        private const string SourceFolder = ArtFolder + "/Source";
        private const string BodyTexturePath = ArtFolder + "/droid_no_platform_no_face.png";
        private const string FaceTexturePath = ArtFolder + "/face.png";
        private const string IdleClipPath = ArtFolder + "/DroidIdle.anim";
        private const string MoveClipPath = ArtFolder + "/DroidMove.anim";
        private const string ControllerPath = ArtFolder + "/DroidRig.controller";
        private const string PrefabPath = "Assets/_Game/Content/Resources/Visuals/Characters/DroidRig.prefab";

        private const string MovingParameter = "IsMoving";
        private const float PixelsPerUnit = 100f;
        private const float PlatformFrameSeconds = 0.125f;
        private const int PlatformSortingOrder = 19;
        private const int ScreenSortingOrder = 20;
        private const int BodySortingOrder = 21;
        private const int FaceSortingOrder = 22;

        private static readonly Vector3 PlatformPosition = new(0f, -1.82f, 0f);
        private static readonly Vector3 PlatformScale = new(0.6f, 0.6f, 1f);
        private static readonly Vector3 BodyPosition = new(0f, 1.8f, 0f);
        private static readonly Vector3 BodyScale = new(1f, 1f, 1f);
        private static readonly Vector3 FacePosition = new(1.49f, 0.54f, 0f);
        private static readonly Vector3 FaceScale = new(0.35f, 0.35f, 1f);
        private static readonly Vector3 ScreenScale = new(3.18f, 2.72f, 1f);

        private static readonly string[] PlatformSourcePaths =
        {
            SourceFolder + "/platform_only1.png",
            SourceFolder + "/platform_only2.png",
            SourceFolder + "/platform_only3.png",
            SourceFolder + "/platform_only4.png"
        };

        private static readonly string[] PlatformTexturePaths =
        {
            ArtFolder + "/platform_only1.png",
            ArtFolder + "/platform_only2.png",
            ArtFolder + "/platform_only3.png",
            ArtFolder + "/platform_only4.png"
        };

        [MenuItem("Game/Characters/Build Droid Rig")]
        public static void Build()
        {
            ConfigureSourceImporters();
            CreateTransparentPlatformFrames();
            ConfigureRuntimeImporters();

            var platformFrames = LoadPlatformFrames();
            var idleClip = BuildIdleClip(platformFrames[0]);
            var moveClip = BuildMoveClip(platformFrames);
            var controller = BuildController(idleClip, moveClip);
            BuildPrefab(controller, platformFrames[0]);

            AssetDatabase.SaveAssets();
            Debug.Log($"Droid rig rebuilt at {PrefabPath} with bodyY={BodyPosition.y}");
        }

        private static void ConfigureSourceImporters()
        {
            foreach (var sourcePath in PlatformSourcePaths)
            {
                ConfigureSprite(sourcePath, true);
            }
        }

        private static void ConfigureRuntimeImporters()
        {
            ConfigureSprite(BodyTexturePath, false);
            ConfigureSprite(FaceTexturePath, false);
            foreach (var platformPath in PlatformTexturePaths)
            {
                ConfigureSprite(platformPath, false);
            }
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

            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Center;
            settings.spritePivot = new Vector2(0.5f, 0.5f);
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
        }

        private static void CreateTransparentPlatformFrames()
        {
            for (var i = 0; i < PlatformSourcePaths.Length; i++)
            {
                var source = AssetDatabase.LoadAssetAtPath<Texture2D>(PlatformSourcePaths[i]);
                if (source == null || !source.isReadable)
                {
                    throw new InvalidDataException($"Readable platform source is missing: {PlatformSourcePaths[i]}");
                }

                var cleanPixels = RemoveConnectedCheckerboard(source.GetPixels32(), source.width, source.height);
                var cleanTexture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
                try
                {
                    cleanTexture.SetPixels32(cleanPixels);
                    cleanTexture.Apply(false, false);
                    File.WriteAllBytes(ToAbsolutePath(PlatformTexturePaths[i]), cleanTexture.EncodeToPNG());
                }
                finally
                {
                    Object.DestroyImmediate(cleanTexture);
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private static Color32[] RemoveConnectedCheckerboard(Color32[] source, int width, int height)
        {
            var output = (Color32[])source.Clone();
            var visited = new bool[source.Length];
            var queue = new Queue<int>();

            for (var x = 0; x < width; x++)
            {
                TryEnqueueBackground(x, 0, width, height, source, visited, queue);
                TryEnqueueBackground(x, height - 1, width, height, source, visited, queue);
            }

            for (var y = 1; y < height - 1; y++)
            {
                TryEnqueueBackground(0, y, width, height, source, visited, queue);
                TryEnqueueBackground(width - 1, y, width, height, source, visited, queue);
            }

            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                output[index] = new Color32(255, 255, 255, 0);
                var x = index % width;
                var y = index / width;

                TryEnqueueBackground(x - 1, y, width, height, source, visited, queue);
                TryEnqueueBackground(x + 1, y, width, height, source, visited, queue);
                TryEnqueueBackground(x, y - 1, width, height, source, visited, queue);
                TryEnqueueBackground(x, y + 1, width, height, source, visited, queue);
            }

            return output;
        }

        private static void TryEnqueueBackground(
            int x,
            int y,
            int width,
            int height,
            Color32[] pixels,
            bool[] visited,
            Queue<int> queue)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return;
            }

            var index = y * width + x;
            if (visited[index])
            {
                return;
            }

            visited[index] = true;
            if (IsCheckerboardPixel(pixels[index]))
            {
                queue.Enqueue(index);
            }
        }

        private static bool IsCheckerboardPixel(Color32 pixel)
        {
            var minimum = Mathf.Min(pixel.r, Mathf.Min(pixel.g, pixel.b));
            var maximum = Mathf.Max(pixel.r, Mathf.Max(pixel.g, pixel.b));
            return minimum >= 235 && maximum - minimum <= 20;
        }

        private static Sprite[] LoadPlatformFrames()
        {
            var frames = new Sprite[PlatformTexturePaths.Length];
            for (var i = 0; i < PlatformTexturePaths.Length; i++)
            {
                frames[i] = AssetDatabase.LoadAssetAtPath<Sprite>(PlatformTexturePaths[i]);
                if (frames[i] == null)
                {
                    throw new InvalidDataException($"Platform frame failed to import: {PlatformTexturePaths[i]}");
                }
            }

            return frames;
        }

        private static AnimationClip BuildIdleClip(Sprite idleFrame)
        {
            var clip = new AnimationClip { frameRate = 8f, name = "DroidIdle" };
            SetPlatformFrames(clip, new[] { new ObjectReferenceKeyframe { time = 0f, value = idleFrame } });
            SaveClip(clip, IdleClipPath, false);
            return clip;
        }

        private static AnimationClip BuildMoveClip(IReadOnlyList<Sprite> frames)
        {
            var clip = new AnimationClip { frameRate = 8f, name = "DroidMove" };
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

        private static AnimatorController BuildController(AnimationClip idleClip, AnimationClip moveClip)
        {
            AssetDatabase.DeleteAsset(ControllerPath);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            controller.AddParameter(MovingParameter, AnimatorControllerParameterType.Bool);

            var stateMachine = controller.layers[0].stateMachine;
            var idleState = stateMachine.AddState("Idle");
            idleState.motion = idleClip;
            var moveState = stateMachine.AddState("Move");
            moveState.motion = moveClip;
            stateMachine.defaultState = idleState;

            AddTransition(idleState, moveState, true);
            AddTransition(moveState, idleState, false);
            return controller;
        }

        private static void AddTransition(AnimatorState from, AnimatorState to, bool moving)
        {
            var transition = from.AddTransition(to);
            transition.hasExitTime = false;
            transition.hasFixedDuration = true;
            transition.duration = 0f;
            transition.AddCondition(
                moving ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot,
                0f,
                MovingParameter);
        }

        private static void BuildPrefab(RuntimeAnimatorController controller, Sprite idlePlatform)
        {
            var bodySprite = AssetDatabase.LoadAssetAtPath<Sprite>(BodyTexturePath);
            var faceSprite = AssetDatabase.LoadAssetAtPath<Sprite>(FaceTexturePath);
            if (bodySprite == null || faceSprite == null)
            {
                throw new InvalidDataException("Droid body or face failed to import.");
            }

            var root = new GameObject("DroidRig");
            try
            {
                var platform = CreateChild(root.transform, "Platform", PlatformPosition);
                platform.localScale = PlatformScale;
                AddSprite(platform, idlePlatform, PlatformSortingOrder);

                var body = CreateChild(root.transform, "Body", BodyPosition);
                body.localScale = BodyScale;
                AddSprite(body, bodySprite, BodySortingOrder);

                var screen = CreateChild(body, "Screen Background", FacePosition);
                screen.localScale = ScreenScale;
                var screenRenderer = AddSprite(screen, null, ScreenSortingOrder);
                screenRenderer.color = Color.white;

                var face = CreateChild(body, "Face", FacePosition);
                face.localScale = FaceScale;
                AddSprite(face, faceSprite, FaceSortingOrder);

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

        private static Transform CreateChild(Transform parent, string name, Vector3 localPosition)
        {
            var child = new GameObject(name).transform;
            child.SetParent(parent, false);
            child.localPosition = localPosition;
            return child;
        }

        private static SpriteRenderer AddSprite(Transform target, Sprite sprite, int sortingOrder)
        {
            var renderer = target.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }

        private static string ToAbsolutePath(string assetPath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName
                ?? throw new DirectoryNotFoundException("Unity project root was not found.");
            return Path.Combine(projectRoot, assetPath.Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
