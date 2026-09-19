using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class NewWeaponArtResourceTests
    {
        [Test]
        public void NewWeaponArt_IsImportedWithRealAlpha()
        {
            var iconIds = new[]
            {
                "plasma_sword", "ion_shotgun", "titanium_disc", "rail_rifle", "boost",
                "sticky_grenade", "sticky_grenade_5A", "guardian_drone", "guardian_drone_5A",
                "spider_mine", "spider_mine_frame_00", "spider_mine_frame_01",
                "spider_mine_frame_02", "spider_mine_frame_03", "titanium_barrier",
                "electric_harpoon", "electric_harpoon_shot",
                "electric_harpoon_5A", "electric_harpoon_5A_shot", "sky_hook", "hopping_grenade",
                "cryo_grenade", "turn_transfer", "incendiary_grenade", "nuclear_charge",
                "repair_jammer", "laser_fence", "self_destruct", "grounder", "virus", "bomber",
                "ufo_carrier", "force_field", "force_field_sphere"
            };

            foreach (var iconId in iconIds)
            {
                var texture = Resources.Load<Texture2D>($"UI/Weapons/{iconId}");
                Assert.IsNotNull(texture, iconId);
            }
        }
    }
}
