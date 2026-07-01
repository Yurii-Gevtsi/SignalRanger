namespace Game.Gameplay.Battle.UI
{
    /// <summary>
    /// Immutable snapshot the battle scene pushes to the HUD each frame.
    /// Keeps the view free of gameplay types so it only knows how to draw.
    /// </summary>
    public readonly struct BattleHudState
    {
        public BattleHudState(
            string status,
            int turnNumber,
            string activeTeam,
            int playerHealth,
            int playerMaxHealth,
            int opponentHealth,
            int opponentMaxHealth,
            string weaponLabel,
            string ammoLabel,
            float power,
            float minPower,
            float maxPower,
            bool canFire)
        {
            Status = status;
            TurnNumber = turnNumber;
            ActiveTeam = activeTeam;
            PlayerHealth = playerHealth;
            PlayerMaxHealth = playerMaxHealth;
            OpponentHealth = opponentHealth;
            OpponentMaxHealth = opponentMaxHealth;
            WeaponLabel = weaponLabel;
            AmmoLabel = ammoLabel;
            Power = power;
            MinPower = minPower;
            MaxPower = maxPower;
            CanFire = canFire;
        }

        public string Status { get; }
        public int TurnNumber { get; }
        public string ActiveTeam { get; }
        public int PlayerHealth { get; }
        public int PlayerMaxHealth { get; }
        public int OpponentHealth { get; }
        public int OpponentMaxHealth { get; }
        public string WeaponLabel { get; }
        public string AmmoLabel { get; }
        public float Power { get; }
        public float MinPower { get; }
        public float MaxPower { get; }
        public bool CanFire { get; }

        public float PlayerHealthFraction => Fraction(PlayerHealth, PlayerMaxHealth);
        public float OpponentHealthFraction => Fraction(OpponentHealth, OpponentMaxHealth);

        public float PowerFraction
        {
            get
            {
                var range = MaxPower - MinPower;
                return range <= 0f ? 0f : UnityEngine.Mathf.Clamp01((Power - MinPower) / range);
            }
        }

        private static float Fraction(int value, int max)
        {
            return max <= 0 ? 0f : UnityEngine.Mathf.Clamp01((float)value / max);
        }
    }
}
