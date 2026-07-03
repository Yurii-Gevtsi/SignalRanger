namespace Game.Core.Battle
{
    using Game.Core.Localization;

    /// <summary>
    /// Immutable snapshot the battle scene pushes to the HUD each frame.
    /// Lives in Core so gameplay and UI both consume it without depending on each other.
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
            string weaponRoleLabel,
            string ammoLabel,
            string weaponHint,
            string mobilityLabel,
            string tutorialTargetId,
            bool tutorialBlocksWeaponSwitch,
            bool tutorialBlocksCancel,
            bool tutorialBlocksMovement,
            float power,
            float minPower,
            float maxPower,
            bool canFire,
            bool canMoveLeft,
            bool canMoveRight,
            bool canJumpForward,
            bool canJumpBackward,
            bool showResult,
            string resultTitle,
            string resultSubtitle,
            string resultOutcome,
            string resultReward,
            string resultUnlock,
            bool canContinue,
            string retryLabel,
            string continueLabel)
        {
            Status = status;
            TurnNumber = turnNumber;
            ActiveTeam = activeTeam;
            PlayerHealth = playerHealth;
            PlayerMaxHealth = playerMaxHealth;
            OpponentHealth = opponentHealth;
            OpponentMaxHealth = opponentMaxHealth;
            WeaponLabel = weaponLabel;
            WeaponRoleLabel = weaponRoleLabel ?? string.Empty;
            AmmoLabel = ammoLabel;
            WeaponHint = weaponHint ?? string.Empty;
            MobilityLabel = mobilityLabel ?? string.Empty;
            TutorialTargetId = tutorialTargetId ?? string.Empty;
            TutorialBlocksWeaponSwitch = tutorialBlocksWeaponSwitch;
            TutorialBlocksCancel = tutorialBlocksCancel;
            TutorialBlocksMovement = tutorialBlocksMovement;
            Power = power;
            MinPower = minPower;
            MaxPower = maxPower;
            CanFire = canFire;
            CanMoveLeft = canMoveLeft;
            CanMoveRight = canMoveRight;
            CanJumpForward = canJumpForward;
            CanJumpBackward = canJumpBackward;
            ShowResult = showResult;
            ResultTitle = resultTitle ?? string.Empty;
            ResultSubtitle = resultSubtitle ?? string.Empty;
            ResultOutcome = resultOutcome ?? string.Empty;
            ResultReward = resultReward ?? string.Empty;
            ResultUnlock = resultUnlock ?? string.Empty;
            CanContinue = canContinue;
            RetryLabel = retryLabel ?? GameText.Restart;
            ContinueLabel = continueLabel ?? GameText.Continue;
        }

        public string Status { get; }
        public int TurnNumber { get; }
        public string ActiveTeam { get; }
        public int PlayerHealth { get; }
        public int PlayerMaxHealth { get; }
        public int OpponentHealth { get; }
        public int OpponentMaxHealth { get; }
        public string WeaponLabel { get; }
        public string WeaponRoleLabel { get; }
        public string AmmoLabel { get; }
        public string WeaponHint { get; }
        public string MobilityLabel { get; }
        public string TutorialTargetId { get; }
        public bool TutorialBlocksWeaponSwitch { get; }
        public bool TutorialBlocksCancel { get; }
        public bool TutorialBlocksMovement { get; }
        public float Power { get; }
        public float MinPower { get; }
        public float MaxPower { get; }
        public bool CanFire { get; }
        public bool CanMoveLeft { get; }
        public bool CanMoveRight { get; }
        public bool CanJumpForward { get; }
        public bool CanJumpBackward { get; }
        public bool ShowResult { get; }
        public string ResultTitle { get; }
        public string ResultSubtitle { get; }
        public string ResultOutcome { get; }
        public string ResultReward { get; }
        public string ResultUnlock { get; }
        public bool CanContinue { get; }
        public string RetryLabel { get; }
        public string ContinueLabel { get; }

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
