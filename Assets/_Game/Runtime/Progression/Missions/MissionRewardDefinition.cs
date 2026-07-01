using System;

namespace Game.Progression.Missions
{
    [Serializable]
    public sealed class MissionRewardDefinition
    {
        public int softCurrency;
        public string unlockWeaponId;

        public MissionRewardDefinition(int softCurrency, string unlockWeaponId = "")
        {
            this.softCurrency = softCurrency < 0 ? 0 : softCurrency;
            this.unlockWeaponId = unlockWeaponId ?? string.Empty;
        }
    }
}
