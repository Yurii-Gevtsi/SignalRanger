using NUnit.Framework;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.Visuals;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleVisualResourceTests
    {
        [Test]
        public void TrainingArena_UsesReadableTransparentForegroundAndExpectedBackground()
        {
            var foreground = Resources.Load<Sprite>("Visuals/Arenas/train2");
            var background = Resources.Load<Sprite>("Visuals/Arenas/Background2");

            Assert.IsNotNull(foreground);
            Assert.IsNotNull(background);
            Assert.IsTrue(foreground.texture.isReadable);
            Assert.AreEqual(1672, foreground.texture.width);
            Assert.AreEqual(941, foreground.texture.height);
            Assert.AreEqual(0, foreground.texture.GetPixel(0, 0).a);
            Assert.AreEqual(1672, background.texture.width);
            Assert.AreEqual(941, background.texture.height);
        }

        [Test]
        public void CharacterResources_ContainOnlyCurrentBattleVisuals()
        {
            Assert.IsNotNull(Resources.Load<GameObject>("Visuals/Characters/RaccoonRig"));
            Assert.IsNotNull(Resources.Load<Sprite>("Visuals/Characters/racoon-full"));
            Assert.IsNotNull(Resources.Load<Sprite>("Visuals/Characters/oct_full"));

            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat-builder-soft"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat-builder"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/cat2-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/ent-guardian-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/octopus-enemy-clean"));
            Assert.IsNull(Resources.Load<Sprite>("Visuals/Characters/raccoon-agent-clean"));
        }

        [Test]
        public void TrainingArenaTheme_PlacesALargerForegroundBelowTheActionArea()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsNotNull(theme);
            Assert.AreEqual(new Vector2(0f, 1.45f), theme.Background.Position);
            Assert.AreEqual(new Vector2(0f, -2f), theme.Foreground.Position);
            Assert.AreEqual(new Vector2(1.8f, 1.8f), theme.Foreground.Scale);
        }

        [Test]
        public void TrainingArenaTheme_UsesOceanKillZoneAndWaterlineDeathHeight()
        {
            var arena = PrototypeArenaLibrary.CreateAll()[0];
            var theme = BattleVisualThemeLibrary.ForArena(arena);

            Assert.IsTrue(theme.HasKillZonePrefab);
            Assert.AreEqual("Visuals/Arenas/KillZones/Ocean/Prefabs/KillZone_Ocean", theme.KillZonePrefabResourcePath);
            Assert.AreEqual(-5.75f, theme.KillZoneDeathY);
        }

        [Test]
        public void OceanKillZone_ProvidesLoopingVisualsAndStableTriggerBounds()
        {
            var prefab = Resources.Load<GameObject>("Visuals/Arenas/KillZones/Ocean/Prefabs/KillZone_Ocean");
            var waveClip = Resources.Load<AnimationClip>("Visuals/Arenas/KillZones/Ocean/Animations/Ocean_Waves_Loop");

            Assert.IsNotNull(prefab);
            Assert.IsNotNull(waveClip);
            Assert.That(waveClip.length, Is.EqualTo(5f).Within(0.02f));

            var visuals = prefab.transform.Find("Visuals");
            Assert.IsNotNull(visuals);
            var waves = visuals.Find("SurfaceWaves");
            Assert.IsNotNull(waves.GetComponent<Animator>());
            Assert.IsNull(visuals.Find("OceanBase"));
            Assert.IsNull(visuals.Find("LightRays"));
            Assert.IsNull(prefab.transform.Find("DeathZone"));
        }
    }
}
