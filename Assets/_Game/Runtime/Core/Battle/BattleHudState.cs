namespace Game.Core.Battle
{
    using System;
    using System.Collections.Generic;
    using Game.Core.Localization;

    public readonly struct BattleWeaponHudItem
    {
        public BattleWeaponHudItem(
            int index,
            string label,
            string iconId,
            string ammoLabel,
            bool hasLimitedAmmo,
            int ammoRemaining,
            bool isUsable,
            int level = 1,
            string description = null,
            float damageValue = 0f,
            float accuracyValue = 0f,
            float ammoValue = -1f,
            float radiusValue = 0f,
            string coreStatLabelKey = "architect.detail.statDamage")
        {
            Index = index;
            Label = label ?? string.Empty;
            IconId = iconId ?? string.Empty;
            AmmoLabel = ammoLabel ?? string.Empty;
            HasLimitedAmmo = hasLimitedAmmo;
            AmmoRemaining = ammoRemaining;
            IsUsable = isUsable;
            Level = level;
            Description = description ?? string.Empty;
            DamageValue = damageValue;
            AccuracyValue = accuracyValue;
            AmmoValue = ammoValue;
            RadiusValue = radiusValue;
            CoreStatLabelKey = coreStatLabelKey;
        }

        public int Index { get; }
        public string Label { get; }
        public string IconId { get; }
        public string AmmoLabel { get; }
        public bool HasLimitedAmmo { get; }
        public int AmmoRemaining { get; }
        public bool IsUsable { get; }
        public int Level { get; }
        public string Description { get; }
        public float DamageValue { get; }
        public float AccuracyValue { get; }
        public float AmmoValue { get; }
        public float RadiusValue { get; }
        public string CoreStatLabelKey { get; }
    }

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
            float turnSecondsRemaining,
            bool showTurnTimer,
            int playerHealth,
            int playerMaxHealth,
            int opponentHealth,
            int opponentMaxHealth,
            string weaponLabel,
            string weaponIconId,
            string weaponRoleLabel,
            string ammoLabel,
            string weaponHint,
            string mobilityLabel,
            IReadOnlyList<BattleWeaponHudItem> arsenalWeapons,
            int selectedWeaponIndex,
            bool hasSelectedWeapon,
            bool showFireButton,
            bool canOpenArsenal,
            string tutorialTargetId,
            bool showTutorialCard,
            bool tutorialBlocksWeaponSwitch,
            bool tutorialBlocksCancel,
            bool tutorialBlocksMovement,
            bool showTutorialSkip,
            float power,
            float minPower,
            float maxPower,
            bool canFire,
            bool canUseMovement,
            bool showResult,
            string resultTitle,
            string resultSubtitle,
            string resultOutcome,
            string resultReward,
            string resultUnlock,
            bool canContinue,
            string retryLabel,
            string continueLabel,
            bool resultPlayerWon = false,
            string resultMissionTitle = null,
            bool resultMissionCleared = false,
            string opponentName = null,
            string opponentWeaponLabel = null,
            int opponentWeaponRank = 0,
            bool ropeControlsActive = false)
        {
            Status = status;
            TurnNumber = turnNumber;
            ActiveTeam = activeTeam;
            TurnSecondsRemaining = UnityEngine.Mathf.Max(0f, turnSecondsRemaining);
            ShowTurnTimer = showTurnTimer;
            PlayerHealth = playerHealth;
            PlayerMaxHealth = playerMaxHealth;
            OpponentHealth = opponentHealth;
            OpponentMaxHealth = opponentMaxHealth;
            WeaponLabel = weaponLabel;
            WeaponIconId = weaponIconId ?? string.Empty;
            WeaponRoleLabel = weaponRoleLabel ?? string.Empty;
            AmmoLabel = ammoLabel;
            WeaponHint = weaponHint ?? string.Empty;
            MobilityLabel = mobilityLabel ?? string.Empty;
            ArsenalWeapons = arsenalWeapons ?? Array.Empty<BattleWeaponHudItem>();
            SelectedWeaponIndex = selectedWeaponIndex;
            HasSelectedWeapon = hasSelectedWeapon;
            ShowFireButton = showFireButton;
            CanOpenArsenal = canOpenArsenal;
            TutorialTargetId = tutorialTargetId ?? string.Empty;
            ShowTutorialCard = showTutorialCard;
            TutorialBlocksWeaponSwitch = tutorialBlocksWeaponSwitch;
            TutorialBlocksCancel = tutorialBlocksCancel;
            TutorialBlocksMovement = tutorialBlocksMovement;
            ShowTutorialSkip = showTutorialSkip;
            Power = power;
            MinPower = minPower;
            MaxPower = maxPower;
            CanFire = canFire;
            CanUseMovement = canUseMovement;
            ShowResult = showResult;
            ResultTitle = resultTitle ?? string.Empty;
            ResultSubtitle = resultSubtitle ?? string.Empty;
            ResultOutcome = resultOutcome ?? string.Empty;
            ResultReward = resultReward ?? string.Empty;
            ResultUnlock = resultUnlock ?? string.Empty;
            CanContinue = canContinue;
            RetryLabel = retryLabel ?? GameText.Restart;
            ContinueLabel = continueLabel ?? GameText.Continue;
            ResultPlayerWon = resultPlayerWon;
            ResultMissionTitle = resultMissionTitle ?? string.Empty;
            ResultMissionCleared = resultMissionCleared;
            OpponentName = opponentName ?? string.Empty;
            OpponentWeaponLabel = opponentWeaponLabel ?? string.Empty;
            OpponentWeaponRank = opponentWeaponRank < 0 ? 0 : opponentWeaponRank;
            RopeControlsActive = ropeControlsActive;
        }

        /// <summary>
        /// Who the player is fighting: the antagonist in a raid, the matched
        /// commander's call sign in a duel, empty in story and training where the
        /// mission already says.
        /// </summary>
        public string OpponentName { get; }

        /// <summary>The weapon the opponent is holding this turn.</summary>
        public string OpponentWeaponLabel { get; }

        /// <summary>
        /// Rank that weapon fights at. Shown only above 1: a rank-1 badge on every
        /// opponent is noise, while a rank-6 one tells the player why that shot hurt.
        /// </summary>
        public int OpponentWeaponRank { get; }
        public bool RopeControlsActive { get; }

        public bool ShowOpponentBar => !string.IsNullOrEmpty(OpponentName) && !ShowResult;
        public bool ShowOpponentRank => OpponentWeaponRank > 1;

        /// <summary>Whether the finished battle was a win, which drives the result artwork.</summary>
        public bool ResultPlayerWon { get; }
        public string ResultMissionTitle { get; }

        /// <summary>
        /// Whether this battle actually banked the mission. Only a won battle counts - a
        /// loss leaves the mission outstanding even if its objective was met.
        /// </summary>
        public bool ResultMissionCleared { get; }

        public string Status { get; }
        public int TurnNumber { get; }
        public string ActiveTeam { get; }
        public float TurnSecondsRemaining { get; }
        public bool ShowTurnTimer { get; }
        public int PlayerHealth { get; }
        public int PlayerMaxHealth { get; }
        public int OpponentHealth { get; }
        public int OpponentMaxHealth { get; }
        public string WeaponLabel { get; }
        public string WeaponIconId { get; }
        public string WeaponRoleLabel { get; }
        public string AmmoLabel { get; }
        public string WeaponHint { get; }
        public string MobilityLabel { get; }
        public IReadOnlyList<BattleWeaponHudItem> ArsenalWeapons { get; }
        public int SelectedWeaponIndex { get; }
        public bool HasSelectedWeapon { get; }
        public bool ShowFireButton { get; }
        public bool CanOpenArsenal { get; }
        public string TutorialTargetId { get; }
        public bool ShowTutorialCard { get; }
        public bool TutorialBlocksWeaponSwitch { get; }
        public bool TutorialBlocksCancel { get; }
        public bool TutorialBlocksMovement { get; }
        public bool ShowTutorialSkip { get; }
        public float Power { get; }
        public float MinPower { get; }
        public float MaxPower { get; }
        public bool CanFire { get; }
        public bool CanUseMovement { get; }
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
