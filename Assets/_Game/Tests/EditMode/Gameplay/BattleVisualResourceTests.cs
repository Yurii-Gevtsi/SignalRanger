using NUnit.Framework;
using Game.App;
using Game.Core;
using Game.Gameplay.Battle.Weapons;
using Game.Gameplay.Battle.Assembly;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Visuals;
using Game.UI.Battle;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleVisualResourceTests
    {
        [Test]
        public void TrainingArena_UsesReadableDestructibleLayersAndReusableAnimatedBackground()
        {
            var ground = Resources.Load<Sprite>("Visuals/Arenas/Training/ground");
            var ship = Resources.Load<Sprite>("Visuals/Arenas/Training/ship");
            var boxes = Resources.Load<Sprite>("Visuals/Arenas/Training/boxes");
            var rock = Resources.Load<Sprite>("Visuals/Arenas/Training/rock");
            var laserKillZone = Resources.Load<Sprite>("Visuals/Arenas/KillZones/LaserLine/laser_line");
            var backgroundPrefab = Resources.Load<GameObject>("Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground");

            Assert.IsNotNull(ground);
            Assert.IsNotNull(ship);
            Assert.IsNotNull(boxes);
            Assert.IsNotNull(rock);
            Assert.IsNotNull(laserKillZone);
            Assert.IsNotNull(backgroundPrefab);
            Assert.IsNotNull(backgroundPrefab.GetComponent<AnimatedSkyBackground>());
            Assert.AreEqual(new Vector3(0f, 1f, 0f), backgroundPrefab.transform.localPosition);
            Assert.AreEqual(new Vector3(1.5f, 1.2f, 1f), backgroundPrefab.transform.localScale);
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/empty_back"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/cloud1"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/cloud2"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/small_planet"));
            Assert.IsNotNull(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Sky1/station"));

            var backgroundInstance = Object.Instantiate(backgroundPrefab);
            try
            {
                Assert.IsNotNull(backgroundInstance.transform.Find("Empty Background"));
                Assert.IsNotNull(backgroundInstance.transform.Find("Orbiting Planet"));
                Assert.IsNotNull(backgroundInstance.transform.Find("Floating Station"));
                Assert.That(backgroundInstance.transform.Find("Cloud 2 Near").localEulerAngles.z,
                    Is.EqualTo(355f).Within(0.01f));
                Assert.That(backgroundInstance.transform.Find("Cloud 2 Far").localEulerAngles.z,
                    Is.EqualTo(355f).Within(0.01f));
                Assert.AreEqual(7, backgroundInstance.transform.childCount);
            }
            finally
            {
                Object.DestroyImmediate(backgroundInstance);
            }

            Assert.IsTrue(ground.texture.isReadable);
            Assert.IsTrue(ship.texture.isReadable);
            Assert.IsTrue(boxes.texture.isReadable);
            Assert.IsTrue(rock.texture.isReadable);
            Assert.AreEqual(2172, ground.texture.width);
            Assert.AreEqual(724, ground.texture.height);
            Assert.That(ground.pixelsPerUnit, Is.EqualTo(256f).Within(0.01f));
            Assert.AreEqual(0, ground.texture.GetPixel(0, 0).a);
            Assert.AreEqual(0, laserKillZone.texture.GetPixel(0, 0).a);
        }

        [Test]
        public void CharacterResources_ContainDroidAndSigmaBattleRigsAndMenuRaccoon()
        {
            var droidRig = Resources.Load<GameObject>("Visuals/Characters/DroidRig");
            var sigmaRig = Resources.Load<GameObject>("Visuals/Characters/SigmaRig");
            Assert.IsNotNull(droidRig);
            Assert.IsNotNull(sigmaRig);
            Assert.IsNotNull(Resources.Load<Sprite>("Visuals/Characters/racoon-full"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/oct_full"));

            var animator = droidRig.GetComponent<Animator>();
            Assert.IsNotNull(animator);
            Assert.IsTrue(System.Array.Exists(animator.parameters,
                parameter => parameter.name == "IsMoving" && parameter.type == AnimatorControllerParameterType.Bool));
            Assert.IsNotNull(droidRig.transform.Find("Platform")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(droidRig.transform.Find("Body")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(droidRig.transform.Find("Body/Screen Background")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(droidRig.transform.Find("Body/Face")?.GetComponent<SpriteRenderer>());

            var platform = droidRig.transform.Find("Platform");
            var body = droidRig.transform.Find("Body");
            var screen = droidRig.transform.Find("Body/Screen Background");
            var face = droidRig.transform.Find("Body/Face");
            Assert.AreEqual(new Vector3(0.6f, 0.6f, 1f), platform.localScale);
            Assert.AreEqual(new Vector3(1f, 1f, 1f), body.localScale);
            Assert.AreEqual(new Vector3(0.35f, 0.35f, 1f), face.localScale);
            Assert.AreEqual(1.8f, body.localPosition.y);
            Assert.AreEqual(Color.white, screen.GetComponent<SpriteRenderer>().color);
            Assert.AreEqual(new Vector3(3.18f, 2.72f, 1f), screen.localScale);

            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat-builder-soft"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat-builder"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat2-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/ent-guardian-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/raccoon-agent-clean"));

            var sigmaAnimator = sigmaRig.GetComponent<Animator>();
            Assert.IsNotNull(sigmaAnimator);
            Assert.IsTrue(System.Array.Exists(sigmaAnimator.parameters,
                parameter => parameter.name == "IsMoving" && parameter.type == AnimatorControllerParameterType.Bool));
            Assert.IsNotNull(sigmaRig.transform.Find("Platform")?.GetComponent<SpriteRenderer>());
            Assert.IsNotNull(sigmaRig.transform.Find("Body")?.GetComponent<SpriteRenderer>());
            Assert.IsNull(sigmaRig.transform.Find("Body/Face"));
        }

        [Test]
        public void BattleHudResources_UseArsenalAndWeaponIconsWithoutDedicatedFireArt()
        {
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Battle/arsenal"));
            Assert.IsNull(Resources.Load<Texture2D>("UI/Battle/fire_btn"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Weapons/bazooka_simple"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Battle/card"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Menu/settings_btn"));
        }

        [Test]
        public void WeaponsScreenResources_ContainShelfBackgroundAndOwnedWeaponIcons()
        {
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Menu/weapons_menu"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Common/architect_btn"));

            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                if (weapon == null || WeaponPresentationText.IsDropOnly(weapon))
                {
                    continue;
                }

                Assert.IsNotNull(Resources.Load<Texture2D>($"UI/Weapons/{weapon.IconId}"), weapon.WeaponId);
            }
        }

        [Test]
        public void CombatProjectileResources_ExistIndependentlyFromHeldWeaponArt()
        {
            var projectileSpriteIds = new[]
            {
                "blaster_shot",
                "impulse_shot",
                "bur_lazer",
                "bur_rocket",
                "mortira_shot",
                "emp_shot",
                "ice_shot",
                "toxic_shot",
                "fire_shot_1",
                "missle_fly",
                "missle_tail"
            };

            foreach (var spriteId in projectileSpriteIds)
            {
                Assert.IsNotNull(Resources.Load<Texture2D>($"UI/Weapons/{spriteId}"), spriteId);
            }
        }

        [Test]
        public void ElectroStatusOverlay_HasFourAnimationFrames()
        {
            for (var frame = 1; frame <= 4; frame++)
            {
                var texture = Resources.Load<Texture2D>($"UI/Generated/electro_overlay_0{frame}");
                Assert.IsNotNull(texture, frame.ToString());
            }
        }

        [Test]
        public void LoadingScreenResources_ContainAllBackgroundsAndSpinner()
        {
            for (var i = 1; i <= 5; i++)
            {
                Assert.IsNotNull(Resources.Load<Texture2D>($"UI/Loaders/{i}"), $"Loader background {i} is missing.");
            }

            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Loaders/loader3"));
            AssertMagentaBackgroundIsTransparent(Resources.Load<Texture2D>("UI/Loaders/loader3"));
            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Common/card2"));
            AssertMagentaBackgroundIsTransparent(Resources.Load<Texture2D>("UI/Common/architect_btn"));

            Assert.IsNotNull(Resources.Load<Texture2D>("UI/Startup/startup_02"));
        }

        [Test]
        public void LoadingScreenPreview_BuildsMissionTextSpinnerAndPercent()
        {
            var root = new GameObject("Loading Screen Test");
            try
            {
                var controller = root.AddComponent<LoadingScreenController>();
                controller.InitializeForPreview(GameSceneNames.Battle);

                var canvas = root.transform.Find("Loading Canvas");
                Assert.IsNotNull(canvas);
                Assert.IsNotNull(RequireRect(canvas, "Loading Background").GetComponent<Image>().sprite);
                Assert.IsNotNull(RequireRect(canvas, "Mission Description Card").GetComponent<Image>().sprite);
                var missionDescription = RequireRect(canvas, "Mission Description");
                Assert.IsNotNull(missionDescription.GetComponent<TMPro.TMP_Text>());
                Assert.AreEqual(new Vector2(-420f, 245f), missionDescription.anchoredPosition);
                Assert.AreEqual(new Vector2(430f, 150f), missionDescription.sizeDelta);
                Assert.IsFalse(missionDescription.GetComponent<TMPro.TMP_Text>().enableAutoSizing);
                Assert.AreEqual("Навчальна місія", missionDescription.GetComponent<TMPro.TMP_Text>().text);
                var ring = RequireRect(canvas, "Loading Ring");
                Assert.AreEqual(new Vector2(224f, 224f), ring.sizeDelta);
                Assert.AreEqual(new Vector2(0f, -400f), ring.anchoredPosition);
                Assert.AreEqual(new Vector3(-1f, 1f, 1f), ring.localScale);
                Assert.AreEqual("loader3", ring.GetComponent<Image>().sprite.texture.name);

                var percent = RequireRect(canvas, "Loading Percent");
                Assert.AreEqual("0%", percent.GetComponent<TMPro.TMP_Text>().text);
                Assert.AreEqual(ring.anchoredPosition, percent.anchoredPosition);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void WeaponsScreen_ShowsShelfActionsAndDetailsWithoutPurchaseButtons()
        {
            var root = new GameObject("Weapons Screen Test");
            try
            {
                root.AddComponent<WeaponsScreenController>();
                var canvas = root.transform.Find("Weapons Canvas");
                Assert.IsNotNull(canvas);

                var settings = RequireRect(canvas, "Settings Button");
                Assert.AreEqual(new Vector2(140f, 140f), settings.sizeDelta);
                Assert.That(settings.anchoredPosition.x, Is.EqualTo(-26f).Within(0.01f));

                var bureau = RequireRect(canvas, "Constructor Bureau Button");
                Assert.AreEqual(new Vector2(220f, 220f), bureau.sizeDelta);
                Assert.That(bureau.anchoredPosition.x, Is.EqualTo(-80f).Within(0.01f));
                Assert.That(bureau.anchoredPosition.y, Is.EqualTo(30f).Within(0.01f));
                Assert.That(bureau.localScale.x, Is.EqualTo(1.2f).Within(0.01f));
                Assert.That(bureau.localScale.y, Is.EqualTo(1.5f).Within(0.01f));
                Assert.AreEqual("architect_btn", bureau.GetComponent<Image>().sprite.texture.name);
                Assert.IsNotNull(bureau.Find("Constructor Bureau Label"));

                var firstWeapon = RequireRect(canvas, "Shelf Weapon 0");
                Assert.AreEqual(new Vector2(195f, 195f), firstWeapon.sizeDelta);
                Assert.That(firstWeapon.anchoredPosition.x, Is.EqualTo(-730f).Within(0.01f));
                Assert.That(firstWeapon.anchoredPosition.y, Is.EqualTo(338f).Within(0.01f));
                Assert.IsNotNull(firstWeapon.GetComponent<Button>());

                var detailsOverlay = canvas.Find("Weapon Details Overlay");
                Assert.IsNotNull(detailsOverlay);
                Assert.IsFalse(detailsOverlay.gameObject.activeSelf);

                firstWeapon.GetComponent<Button>().onClick.Invoke();
                Assert.IsTrue(detailsOverlay.gameObject.activeSelf);
                var details = RequireRect(detailsOverlay, "Weapon Details");
                Assert.AreEqual(new Vector2(1400f, 940f), details.sizeDelta);
                Assert.That(details.anchoredPosition.x, Is.EqualTo(400f).Within(0.01f));
                Assert.IsFalse(details.GetComponent<Image>().raycastTarget);
                var detailsHitArea = RequireRect(details, "Weapon Details Hit Area");
                Assert.AreEqual(new Vector2(1240f, 760f), detailsHitArea.sizeDelta);
                Assert.IsTrue(detailsHitArea.GetComponent<Image>().raycastTarget);
                var detailChildren = detailsOverlay.GetComponentsInChildren<Transform>(true);
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Buy Coins"));
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Buy Crystals"));
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Weapon Details Icon"));
                Assert.IsFalse(System.Array.Exists(detailChildren, child => child.name == "Weapon Details Level"));
                Assert.IsTrue(System.Array.Exists(detailChildren, child => child.name == "Close Weapon Details"));
                var close = RequireRect(details, "Close Weapon Details");
                Assert.AreEqual(new Vector2(-260f, -250f), close.anchoredPosition);
                var name = RequireRect(details, "Weapon Details Name");
                Assert.That(name.anchoredPosition.y, Is.EqualTo(-250f).Within(0.01f));
                Assert.That(name.anchorMin.x, Is.EqualTo(0.20f).Within(0.01f));
                Assert.That(name.anchorMax.x, Is.EqualTo(0.82f).Within(0.01f));
                var description = RequireRect(details, "Weapon Details Description");
                Assert.That(description.offsetMax.y, Is.EqualTo(-20f).Within(0.01f));
                Assert.That(description.anchorMin.x, Is.EqualTo(0.20f).Within(0.01f));
                Assert.That(description.anchorMax.x, Is.EqualTo(0.80f).Within(0.01f));

                var rightSideWeapon = RequireRect(canvas, "Shelf Weapon 5");
                rightSideWeapon.GetComponent<Button>().onClick.Invoke();
                Assert.That(details.anchoredPosition.x, Is.EqualTo(-550f).Within(0.01f));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void TrainingArenaTheme_UsesDroidRigWithoutBattleRaccoonFallback()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.AreEqual("Visuals/Characters/DroidRig", theme.PlayerRigResourcePath);
            Assert.IsFalse(theme.HasPlayerSkin);
            Assert.AreEqual(1, theme.PlayerSkinFacingDirectionX);
            Assert.AreEqual(new Vector2(0f, -0.08f), theme.UnitSpriteOffset);
        }

        [Test]
        public void DesertArenaTheme_ReusesAnimatedSkyAndCharacterRigs()
        {
            var arena = System.Array.Find(
                PrototypeArenaLibrary.CreateAll(),
                item => item.ArenaId == "arena.desert-sci-fi");
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsNotNull(theme);
            Assert.AreEqual("Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground", theme.BackgroundPrefabResourcePath);
            Assert.AreEqual("Visuals/Characters/DroidRig", theme.PlayerRigResourcePath);
            Assert.AreEqual("Visuals/Characters/SigmaRig", theme.OpponentRigResourcePath);
            Assert.AreEqual(new Vector2(0.18f, 0.18f), theme.UnitSpriteScale);
            Assert.IsTrue(theme.HideProceduralTerrain);
            Assert.IsFalse(theme.UseProceduralTerrainColliders);
        }

        [Test]
        public void DesertArena_AllModularVisualAndMaskResourcesAreReadableAndPixelMatched()
        {
            var arena = System.Array.Find(
                PrototypeArenaLibrary.CreateAll(),
                item => item.ArenaId == "arena.desert-sci-fi");

            Assert.IsNotNull(arena);
            foreach (var surface in arena.SpriteSurfaces)
            {
                var visual = Resources.Load<Sprite>(surface.VisualResourcePath);
                var mask = Resources.Load<Texture2D>(surface.MaskResourcePath);
                Assert.IsNotNull(visual, surface.VisualResourcePath);
                Assert.IsNotNull(mask, surface.MaskResourcePath);
                Assert.IsTrue(visual.texture.isReadable, surface.VisualResourcePath);
                Assert.IsTrue(mask.isReadable, surface.MaskResourcePath);
                Assert.AreEqual(visual.texture.width, mask.width, surface.InstanceId);
                Assert.AreEqual(visual.texture.height, mask.height, surface.InstanceId);
                Assert.AreEqual(256f, visual.pixelsPerUnit, surface.InstanceId);
                Assert.Greater(surface.Scale, 0f, surface.InstanceId);
            }
        }

        [Test]
        public void DroidRig_PlatformAnimatesOnlyWhenPresentedPositionChanges()
        {
            var viewObject = new GameObject("Droid View Test");
            Sprite weaponSprite = null;
            try
            {
                var prefab = Resources.Load<GameObject>("Visuals/Characters/DroidRig");
                var rig = Object.Instantiate(prefab, viewObject.transform);
                var animator = rig.GetComponent<Animator>();
                var view = viewObject.AddComponent<BattleUnitView>();
                view.SetPreserveSpriteColor(true);
                view.SetVisualRoot(rig.transform);
                Assert.IsNotNull(rig.transform.Find("Body/Screen Background").GetComponent<SpriteRenderer>().sprite);

                var state = new BattleUnitState("player.droid.01", BattleTeamId.Player, Vector2.zero, 100);
                var presenter = new BattleUnitPresenter(state, view);
                presenter.Initialize(Color.white);
                view.SetHealthLabelPositionY(2f);
                Assert.IsFalse(animator.GetBool("IsMoving"));

                var healthLabel = viewObject.transform.Find("Health Label")?.GetComponent<TextMesh>();
                Assert.IsNotNull(healthLabel);
                Assert.AreEqual("100", healthLabel.text);
                Assert.AreEqual(new Color(0.06f, 1f, 0.10f, 1f), healthLabel.color);
                Assert.AreEqual(healthLabel.transform.localScale.x, healthLabel.transform.localScale.y);
                Assert.That(healthLabel.transform.localPosition.y, Is.EqualTo(2f).Within(0.0001f));
                Assert.That(healthLabel.transform.localScale.x, Is.EqualTo(0.15f).Within(0.0001f));

                weaponSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                    new Vector2(0.5f, 0.5f), 1f);
                presenter.SetEquippedWeapon(weaponSprite, false);
                presenter.SetWeaponAim(new Vector2(1f, 1f));
                var weaponMount = viewObject.transform.Find("Weapon Mount");
                var weaponRenderer = weaponMount?.GetComponent<SpriteRenderer>();
                Assert.IsNotNull(weaponRenderer);
                Assert.AreSame(weaponSprite, weaponRenderer.sprite);
                Assert.IsTrue(weaponRenderer.flipX);
                Assert.That(weaponMount.localPosition.x, Is.EqualTo(-0.68f).Within(0.0001f));
                Assert.That(weaponMount.localEulerAngles.z, Is.EqualTo(45f).Within(0.01f));
                var expectedMuzzle = weaponMount.TransformPoint(new Vector3(weaponSprite.bounds.extents.x, 0f, 0f));
                Assert.That(view.WeaponMuzzlePosition.x, Is.EqualTo(expectedMuzzle.x).Within(0.0001f));
                Assert.That(view.WeaponMuzzlePosition.y, Is.EqualTo(expectedMuzzle.y).Within(0.0001f));

                var initialRigScaleX = rig.transform.localScale.x;
                presenter.SetFacing(-1);
                Assert.AreEqual(-1, state.FacingDirectionX);
                Assert.That(rig.transform.localScale.x, Is.EqualTo(-initialRigScaleX).Within(0.0001f));
                Assert.That(weaponMount.localPosition.x, Is.EqualTo(0.68f).Within(0.0001f));

                state.MoveTo(new Vector2(0.25f, 0f));
                presenter.Refresh(true);
                Assert.IsTrue(animator.GetBool("IsMoving"));

                presenter.Refresh(true);
                Assert.IsTrue(animator.GetBool("IsMoving"));

                view.SendMessage("AdvanceMovementAnimation", 0f);
                Assert.IsTrue(animator.GetBool("IsMoving"));

                view.SendMessage("AdvanceMovementAnimation", 0.51f);

                Assert.IsFalse(animator.GetBool("IsMoving"));

                state.MoveTo(new Vector2(state.Position.x, state.Position.y + 0.25f));
                presenter.Refresh(true);
                Assert.IsFalse(animator.GetBool("IsMoving"));
            }
            finally
            {
                if (weaponSprite != null)
                {
                    Object.DestroyImmediate(weaponSprite);
                }

                Object.DestroyImmediate(viewObject);
            }
        }

        [Test]
        public void OpponentHealth_IsShownAsUnscaledRedNumberOnly()
        {
            var viewObject = new GameObject("Opponent Health Test");
            try
            {
                var view = viewObject.AddComponent<BattleUnitView>();
                var state = new BattleUnitState("opponent.test.01", BattleTeamId.Opponent, Vector2.zero, 100);
                var presenter = new BattleUnitPresenter(state, view);

                presenter.Initialize(Color.white);

                var healthLabel = viewObject.transform.Find("Health Label")?.GetComponent<TextMesh>();
                Assert.IsNotNull(healthLabel);
                Assert.AreEqual("100", healthLabel.text);
                Assert.AreEqual(new Color(1f, 0.05f, 0.03f, 1f), healthLabel.color);
                Assert.IsNull(viewObject.transform.Find("Health Bar"));
                Assert.AreEqual(healthLabel.transform.localScale.x, healthLabel.transform.localScale.y);
                Assert.That(healthLabel.transform.localPosition.y, Is.EqualTo(1.48f).Within(0.0001f));
                Assert.That(healthLabel.transform.localScale.x, Is.EqualTo(0.20f).Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(viewObject);
            }
        }

        [Test]
        public void DroidRig_MoveClipLoopsAllFourPlatformFrames()
        {
            var moveClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(
                "Assets/_Game/Art/Characters/Droid/DroidMove.anim");
            Assert.IsNotNull(moveClip);

            var bindings = AnimationUtility.GetObjectReferenceCurveBindings(moveClip);
            Assert.AreEqual(1, bindings.Length);
            Assert.AreEqual("Platform", bindings[0].path);

            var keys = AnimationUtility.GetObjectReferenceCurve(moveClip, bindings[0]);
            Assert.AreEqual(5, keys.Length);
            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual($"platform_only{i + 1}", keys[i].value.name);
            }

            Assert.AreSame(keys[0].value, keys[4].value);
            Assert.IsTrue(AnimationUtility.GetAnimationClipSettings(moveClip).loopTime);
        }

        [Test]
        public void BattleHud_UsesCompactAlignedTopBarAndBottomWeaponControls()
        {
            var root = new GameObject("Battle HUD Layout Test");
            GameObject createdEventSystem = null;
            try
            {
                root.AddComponent<BattleHudView>();
                createdEventSystem = GameObject.Find("Battle EventSystem");

                var canvas = root.transform.Find("Battle HUD Canvas");
                Assert.IsNotNull(canvas);

                var tutorialCard = RequireRect(canvas, "Tutorial Card");
                var timer = RequireRect(canvas, "Turn Timer");
                var settings = RequireRect(canvas, "Settings");
                var arsenal = RequireRect(canvas, "Arsenal");
                var moveLeft = RequireRect(canvas, "Move Left");
                var moveRight = RequireRect(canvas, "Move Right");
                var jumpLeft = RequireRect(canvas, "Jump Left");
                var jumpRight = RequireRect(canvas, "Jump Right");

                Assert.AreEqual(new Vector2(500f, 500f), tutorialCard.sizeDelta);
                Assert.AreEqual(new Vector2(246f, 180f), timer.sizeDelta);
                Assert.IsFalse(timer.GetComponent<Image>().preserveAspect);
                Assert.That(tutorialCard.anchoredPosition.y, Is.EqualTo(24f).Within(0.01f));
                Assert.That(timer.anchoredPosition.y, Is.EqualTo(0f).Within(0.01f));
                Assert.That(settings.anchoredPosition.y, Is.EqualTo(-20f).Within(0.01f));
                Assert.That(arsenal.anchoredPosition.y, Is.EqualTo(-30f).Within(0.01f));
                Assert.AreEqual(new Vector2(264f, 264f), arsenal.sizeDelta);
                Assert.AreEqual(new Vector2(202f, 202f), moveLeft.sizeDelta);
                Assert.AreEqual(new Vector2(202f, 202f), moveRight.sizeDelta);
                Assert.AreEqual(new Vector2(134f, 134f), jumpLeft.sizeDelta);
                Assert.AreEqual(new Vector2(134f, 134f), jumpRight.sizeDelta);

                var fire = RequireRect(canvas, "Fire Weapon");
                var reset = RequireRect(canvas, "Reset Weapon");
                Assert.That(fire.anchorMin.x, Is.EqualTo(0.75f).Within(0.001f));
                Assert.That(reset.anchorMin.x, Is.EqualTo(0.25f).Within(0.001f));
                Assert.That(fire.anchoredPosition.y, Is.EqualTo(135f).Within(0.01f));
                Assert.That(reset.anchoredPosition.y, Is.EqualTo(135f).Within(0.01f));
                Assert.AreEqual(new Vector2(180f, 180f), fire.sizeDelta);
                Assert.AreEqual(new Vector2(180f, 180f), reset.sizeDelta);
                var fireFrameSprite = fire.GetComponent<Image>().sprite;
                Assert.AreEqual("Battle Fire Ring", fireFrameSprite.name);
                Assert.That(fireFrameSprite.texture.GetPixel(128, 128).a, Is.EqualTo(0f).Within(0.001f));
                Assert.AreEqual("reset_btn", reset.GetComponent<Image>().sprite.texture.name);
                var fireIcon = RequireRect(fire, "Selected Weapon Icon");
                Assert.AreEqual(new Vector2(120f, 120f), fireIcon.sizeDelta);
                Assert.That(fireIcon.anchoredPosition.y, Is.EqualTo(-10f).Within(0.01f));
                Assert.AreEqual(new Vector3(0.8f, 0.8f, 1f), fireIcon.localScale);
                Assert.IsFalse(fireIcon.GetComponent<Image>().raycastTarget);

                Assert.IsNull(canvas.Find("Power Track"));
                Assert.IsNull(canvas.Find("Power Capsule"));
                Assert.IsNull(canvas.Find("Aim Guide"));
            }
            finally
            {
                Object.DestroyImmediate(root);
                if (createdEventSystem != null)
                {
                    Object.DestroyImmediate(createdEventSystem);
                }
            }
        }

        private static RectTransform RequireRect(Transform parent, string name)
        {
            var child = parent.Find(name) as RectTransform;
            Assert.IsNotNull(child, $"{name} must exist in the battle HUD.");
            return child;
        }

        private static void AssertMagentaBackgroundIsTransparent(Texture2D texture)
        {
            Assert.IsNotNull(texture);
            Assert.AreEqual(0f, texture.GetPixel(0, 0).a, $"Top-left magenta background should be transparent for {texture.name}.");
        }

        [Test]
        public void TrainingArenaTheme_UsesAnimatedBackgroundPrefabAndLargerForeground()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsNotNull(theme);
            Assert.IsFalse(theme.Background.HasSprite);
            Assert.IsTrue(theme.HasBackgroundPrefab);
            Assert.AreEqual("Visuals/Arenas/Backgrounds/Sky1/AnimatedSkyBackground", theme.BackgroundPrefabResourcePath);
            Assert.AreEqual("Visuals/Arenas/Training/ground", theme.Foreground.ResourcePath);
            Assert.AreEqual(3, theme.AdditionalDestructibleLayers.Count);
            Assert.AreEqual("Visuals/Arenas/Training/ship", theme.AdditionalDestructibleLayers[0].ResourcePath);
            Assert.AreEqual("Visuals/Arenas/Training/boxes", theme.AdditionalDestructibleLayers[1].ResourcePath);
            Assert.AreEqual("Visuals/Arenas/Training/rock", theme.AdditionalDestructibleLayers[2].ResourcePath);
            Assert.AreEqual(1, theme.OverlayLayers.Count);
            Assert.AreEqual("Visuals/Arenas/KillZones/LaserLine/laser_line", theme.OverlayLayers[0].ResourcePath);
            Assert.IsFalse(theme.OverlayLayers[0].Solid);
        }

        [Test]
        public void DesertArenaTheme_UsesTheSharedLaserKillZone()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[1];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsNotNull(theme);
            Assert.AreEqual(1, theme.OverlayLayers.Count);
            Assert.AreEqual("Visuals/Arenas/KillZones/LaserLine/laser_line", theme.OverlayLayers[0].ResourcePath);
            Assert.IsTrue(theme.KillZoneY.HasValue);
            Assert.That(theme.KillZoneY.Value, Is.EqualTo(-10.2f).Within(0.001f));
        }

        [Test]
        public void ZoomedOutCamera_UsesViewportEdgesWhenClampingToArena()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "ClampCameraPositionToBounds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            var result = (Vector3)method.Invoke(null, new object[]
            {
                new Vector3(-9.5f, -6f, -10f),
                Rect.MinMaxRect(-9.5f, -6f, 9.5f, 6.8f),
                7f,
                2.1f,
                new Vector2(0f, 1.2f)
            });

            Assert.That(result.x, Is.EqualTo(0f).Within(0.001f));
            Assert.That(result.y, Is.EqualTo(1.2f).Within(0.001f));
            Assert.That(result.z, Is.EqualTo(-10f).Within(0.001f));
        }

        [Test]
        public void CameraPanBounds_IncludeTheFullForegroundWidth()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "ExpandHorizontalCameraBounds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            var result = (Rect)method.Invoke(null, new object[]
            {
                Rect.MinMaxRect(-9.5f, -6f, 9.5f, 6.8f),
                new Bounds(Vector3.zero, new Vector3(30f, 12f, 0f))
            });

            Assert.That(result.xMin, Is.EqualTo(-15f).Within(0.001f));
            Assert.That(result.xMax, Is.EqualTo(15f).Within(0.001f));
            Assert.That(result.yMin, Is.EqualTo(-6f).Within(0.001f));
            Assert.That(result.yMax, Is.EqualTo(6.8f).Within(0.001f));
        }

        [Test]
        public void MovementControls_AreOwnedOnlyByThePlayerTurn()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "IsPlayerControlTurn",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            Assert.IsTrue((bool)method.Invoke(null, new object[] { BattleTeamId.Player }));
            Assert.IsFalse((bool)method.Invoke(null, new object[] { BattleTeamId.Opponent }));
        }

        [Test]
        public void FullTrajectory_AddsDotsInsteadOfIncreasingTheirSpacing()
        {
            var method = typeof(BattleSceneController).GetMethod(
                "CalculateFullTrajectoryDotCount",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method);

            const float arcLength = 44f;
            var dotCount = (int)method.Invoke(null, new object[] { arcLength });

            Assert.That(dotCount, Is.GreaterThan(20));
            Assert.That(arcLength / (dotCount - 1), Is.LessThanOrEqualTo(0.55f));
        }

    }
}
