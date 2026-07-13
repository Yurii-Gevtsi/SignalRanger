using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Game.EditorTools.Characters
{
    /// <summary>
    /// Builds the raccoon cut-out rig: sprite import settings, the rig prefab,
    /// the idle animation clip and its animator controller.
    /// Re-run after replacing the source art to regenerate everything.
    /// </summary>
    public static class RaccoonRigBuilder
    {
        private const string ArtFolder = "Assets/_Game/Art/Characters/Raccoon";
        private const string BodyTexturePath = ArtFolder + "/raccoon_body.png";
        private const string ArmDownTexturePath = ArtFolder + "/raccoon_arm_down.png";
        private const string ArmAimTexturePath = ArtFolder + "/raccoon_arm_aim.png";
        private const string FistTexturePath = ArtFolder + "/raccoon_fist.png";
        private const string IdleClipPath = ArtFolder + "/RaccoonIdle.anim";
        private const string ControllerPath = ArtFolder + "/RaccoonRig.controller";
        private const string PrefabPath = "Assets/_Game/Content/Resources/Visuals/Characters/RaccoonRig.prefab";

        // Source canvases are 1254x1254, PPU 100. Landmarks measured from the art:
        // canvas center (627,627), feet line y=1149,
        // left shoulder socket (335,645), right shoulder socket (690,655),
        // arm sprite shoulder joint (595,315).
        private const float FeetOffsetY = 5.22f;
        private static readonly Vector2 LeftShoulderLocal = new(-2.92f, 5.04f);
        private static readonly Vector2 RightShoulderLocal = new(0.63f, 4.94f);
        private static readonly Vector2 ArmDownPivot = new(0.4745f, 0.7488f);
        private static readonly Vector2 ArmAimPivot = new(0.343f, 0.733f);
        private const float LeftArmScale = 0.36f;
        private const float RightArmScale = 0.41f;
        private const int BodySortingOrder = 20;
        private const int ArmSortingOrder = 21;

        private const float IdleDuration = 2.4f;
        private const float BreathScaleY = 1.02f;
        private const float BreathScaleX = 0.993f;
        private const float ArmSwayDegrees = 2.5f;

        [MenuItem("Game/Characters/Build Raccoon Rig")]
        public static void Build()
        {
            ConfigureImporters();
            var clip = BuildIdleClip();
            var controller = BuildController(clip);
            BuildPrefab(controller);
            AssetDatabase.SaveAssets();
            Debug.Log($"Raccoon rig rebuilt at {PrefabPath}");
        }

        private static void ConfigureImporters()
        {
            ConfigureSprite(BodyTexturePath, SpriteAlignment.Center, Vector2.zero);
            ConfigureSprite(ArmDownTexturePath, SpriteAlignment.Custom, ArmDownPivot);
            ConfigureSprite(ArmAimTexturePath, SpriteAlignment.Custom, ArmAimPivot);
            ConfigureSprite(FistTexturePath, SpriteAlignment.Center, Vector2.zero);
        }

        private static void ConfigureSprite(string assetPath, SpriteAlignment alignment, Vector2 pivot)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError($"Texture not found: {assetPath}");
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 100f;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
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

        private static AnimationClip BuildIdleClip()
        {
            var clip = new AnimationClip { frameRate = 60f };

            var half = IdleDuration * 0.5f;
            clip.SetCurve("Torso", typeof(Transform), "m_LocalScale.x",
                SmoothLoop(1f, BreathScaleX, half, IdleDuration));
            clip.SetCurve("Torso", typeof(Transform), "m_LocalScale.y",
                SmoothLoop(1f, BreathScaleY, half, IdleDuration));
            clip.SetCurve("Torso", typeof(Transform), "m_LocalScale.z",
                AnimationCurve.Constant(0f, IdleDuration, 1f));
            SetEulerZCurve(clip, "Torso/ShoulderL", SmoothLoop(0f, ArmSwayDegrees, half, IdleDuration));
            SetEulerZCurve(clip, "Torso/ShoulderR", SmoothLoop(0f, -ArmSwayDegrees, half, IdleDuration));

            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            AssetDatabase.DeleteAsset(IdleClipPath);
            AssetDatabase.CreateAsset(clip, IdleClipPath);
            return clip;
        }

        private static void SetEulerZCurve(AnimationClip clip, string path, AnimationCurve curve)
        {
            var binding = EditorCurveBinding.FloatCurve(path, typeof(Transform), "localEulerAnglesRaw.z");
            AnimationUtility.SetEditorCurve(clip, binding, curve);
        }

        private static AnimationCurve SmoothLoop(float rest, float peak, float peakTime, float duration)
        {
            return new AnimationCurve(
                new Keyframe(0f, rest, 0f, 0f),
                new Keyframe(peakTime, peak, 0f, 0f),
                new Keyframe(duration, rest, 0f, 0f));
        }

        private static AnimatorController BuildController(AnimationClip idleClip)
        {
            AssetDatabase.DeleteAsset(ControllerPath);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            var idleState = controller.layers[0].stateMachine.AddState("Idle");
            idleState.motion = idleClip;
            return controller;
        }

        private static void BuildPrefab(AnimatorController controller)
        {
            var bodySprite = AssetDatabase.LoadAssetAtPath<Sprite>(BodyTexturePath);
            var armSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ArmDownTexturePath);
            if (bodySprite == null || armSprite == null)
            {
                Debug.LogError("Raccoon sprites failed to import; prefab not built.");
                return;
            }

            var root = new GameObject("RaccoonRig");
            try
            {
                var torso = CreateChild(root.transform, "Torso", new Vector3(0f, -FeetOffsetY, 0f));

                var body = CreateChild(torso, "Body", new Vector3(0f, FeetOffsetY, 0f));
                AddSprite(body, bodySprite, BodySortingOrder, false);

                var shoulderL = CreateChild(torso, "ShoulderL", LeftShoulderLocal);
                var armL = CreateChild(shoulderL, "ArmL", Vector3.zero);
                armL.localScale = new Vector3(LeftArmScale, LeftArmScale, 1f);
                AddSprite(armL, armSprite, ArmSortingOrder, false);

                var shoulderR = CreateChild(torso, "ShoulderR", RightShoulderLocal);
                var armR = CreateChild(shoulderR, "ArmR", Vector3.zero);
                armR.localScale = new Vector3(RightArmScale, RightArmScale, 1f);
                AddSprite(armR, armSprite, ArmSortingOrder, true);

                var animator = root.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;

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

        private static void AddSprite(Transform target, Sprite sprite, int sortingOrder, bool flipX)
        {
            var renderer = target.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
            renderer.flipX = flipX;
        }
    }
}
