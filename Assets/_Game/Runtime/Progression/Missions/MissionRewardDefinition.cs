using System;

namespace Game.Progression.Missions
{
    [Serializable]
    public sealed class MissionRewardDefinition
    {
        public int softCurrency;
        public int commanderExperience;
        public int blueprints;
        public string blueprintWeaponId;
        public string unlockWeaponId;

        public MissionRewardDefinition(
            int softCurrency,
            string unlockWeaponId = "",
            int commanderExperience = 0,
            int blueprints = 0,
            string blueprintWeaponId = "")
        {
            this.softCurrency = softCurrency < 0 ? 0 : softCurrency;
            this.commanderExperience = commanderExperience < 0 ? 0 : commanderExperience;
            this.blueprints = blueprints < 0 ? 0 : blueprints;
            this.blueprintWeaponId = blueprintWeaponId ?? string.Empty;
            this.unlockWeaponId = unlockWeaponId ?? string.Empty;
        }
    }
}
