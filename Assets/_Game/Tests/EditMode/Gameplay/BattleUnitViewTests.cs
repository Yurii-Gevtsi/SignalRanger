using System.Reflection;
using Game.Gameplay.Battle.Runtime;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleUnitViewTests
    {
        [Test]
        public void SelfDestructMarker_AppearsAboveTheDroidAndTracksTheState()
        {
            var viewObject = new GameObject("Self Destruct View Test");
            try
            {
                var view = viewObject.AddComponent<BattleUnitView>();
                var setStatus = typeof(BattleUnitView).GetMethod("SetSelfDestructActive", BindingFlags.Public | BindingFlags.Instance);
                Assert.IsNotNull(setStatus);
                setStatus.Invoke(view, new object[] { true });
                var icon = viewObject.transform.Find("Self Destruct Status Icon")?.GetComponent<SpriteRenderer>();
                Assert.IsNotNull(icon);
                Assert.IsTrue(icon.enabled);
                Assert.AreEqual("self_destruct", icon.sprite.texture.name);
                Assert.That(icon.sprite.bounds.size.x * icon.transform.localScale.x, Is.EqualTo(0.6f).Within(0.01f));
                setStatus.Invoke(view, new object[] { false });
                Assert.IsFalse(icon.enabled);
            }
            finally
            {
                Object.DestroyImmediate(viewObject);
            }
        }

        [Test]
        public void VirusStatusIcon_SitsBesideTheHealthLabelOnTheOtherSideFromTheJammerIconAndTracksTheState()
        {
            var viewObject = new GameObject("Virus Status View Test");
            try
            {
                var view = viewObject.AddComponent<BattleUnitView>();
                var setStatus = typeof(BattleUnitView).GetMethod("SetVirusActive", BindingFlags.Public | BindingFlags.Instance);
                Assert.IsNotNull(setStatus, "BattleUnitView must expose the virus presentation state.");
                setStatus.Invoke(view, new object[] { true });

                var icon = viewObject.transform.Find("Virus Status Icon")?.GetComponent<SpriteRenderer>();
                var healthLabel = viewObject.transform.Find("Health Label");
                Assert.IsNotNull(icon);
                Assert.IsTrue(icon.enabled);
                Assert.AreEqual("virus", icon.sprite.texture.name);
                Assert.IsNotNull(healthLabel);
                Assert.That(icon.transform.localPosition.y, Is.EqualTo(healthLabel.localPosition.y).Within(0.0001f));
                Assert.That(icon.transform.localPosition.x, Is.LessThan(healthLabel.localPosition.x));
                var width = icon.sprite.bounds.size.x * icon.transform.localScale.x;
                Assert.That(width, Is.InRange(0.4f, 0.7f), "readable, not larger than the droid");

                setStatus.Invoke(view, new object[] { false });
                Assert.IsFalse(icon.enabled);
            }
            finally
            {
                Object.DestroyImmediate(viewObject);
            }
        }

        [Test]
        public void RepairJammerStatusIcon_TracksActiveState()
        {
            var viewObject = new GameObject("Repair Jammer Status View Test");
            try
            {
                var view = viewObject.AddComponent<BattleUnitView>();
                var setStatus = typeof(BattleUnitView).GetMethod(
                    "SetRepairJammerActive",
                    BindingFlags.Public | BindingFlags.Instance);

                Assert.IsNotNull(setStatus, "BattleUnitView must expose the repair-jammer presentation state.");

                setStatus.Invoke(view, new object[] { true });

                var icon = viewObject.transform.Find("Repair Jammer Status Icon")?.GetComponent<SpriteRenderer>();
                var healthLabel = viewObject.transform.Find("Health Label");
                Assert.IsNotNull(icon);
                Assert.IsTrue(icon.enabled);
                Assert.IsNotNull(icon.sprite);
                Assert.AreEqual("no-repair-2", icon.sprite.texture.name);
                Assert.IsNotNull(healthLabel);
                Assert.That(icon.transform.localPosition.y, Is.EqualTo(healthLabel.localPosition.y).Within(0.0001f));
                Assert.That(icon.transform.localPosition.x, Is.GreaterThan(healthLabel.localPosition.x));

                setStatus.Invoke(view, new object[] { false });

                Assert.IsFalse(icon.enabled);
            }
            finally
            {
                Object.DestroyImmediate(viewObject);
            }
        }
    }
}
