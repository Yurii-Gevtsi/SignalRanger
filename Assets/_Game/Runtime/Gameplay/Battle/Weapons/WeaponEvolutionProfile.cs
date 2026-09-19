using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Battle-only result of the player's saved workshop choice. It never mutates
    /// the shared base weapon definition, so opponents keep their own balance.
    /// </summary>
    public readonly struct WeaponEvolutionProfile
    {
        public WeaponEvolutionProfile(
            int salvoCount,
            float salvoIntervalSeconds,
            float knockbackMultiplier,
            int blasterBurstShotCount = 3,
            int terrainBounceLimit = 0,
            float drillRange = 0f,
            TerrainMaterial drillPenetration = TerrainMaterial.Soil,
            int mortarFragmentCount = 0,
            int mortarCascadeCount = 0,
            float rocketDrillDurationSeconds = 0f,
            TerrainMaterial rocketDrillPenetration = TerrainMaterial.Soil,
            int missileSplitChargeCount = 0,
            int missileObstacleBypassCount = 0,
            float c4DamageMultiplier = 1f,
            float c4ImpactRadiusMultiplier = 1f,
            bool grenadeSticks = false,
            float grenadeMagneticSeekRadius = 0f,
            float grenadeGravityPullRadius = 0f,
            float grenadePullAcceleration = 0f,
            int mineDeployLimitPerTurn = 1,
            bool mineTriggersEnemiesOnly = false,
            bool mineAlliesImmuneToExplosion = false,
            int repairProtectionTurns = 0,
            int repairRegenerationTurns = 0,
            bool teleportExchangesEnemy = false,
            bool teleportExchangesAlliesAndObjects = false,
            int teleportDisorientationTurns = 0,
            float teleportImpulseRadius = 0f,
            float teleportImpulseStrength = 0f,
            int toxicArmorWeakenPercent = 0,
            int toxicArmorWeakenTurns = 0,
            float toxicMovementDamagePerSecond = 0f,
            int toxicDurationBonusTurns = 0,
            float fireGunStreamDurationMultiplier = 1f,
            float fireGunBurnDurationMultiplier = 1f,
            int fireGunIgniteTurns = 0,
            int fireGunIgniteDamagePerTurn = 0,
            int fireGunChainDamagePercent = 0,
            int empStunTurns = 0,
            bool empStunBlocksMovement = false,
            int empWeakenDamagePercent = 0,
            int empWeakenTurns = 0,
            float cryoFreezeTurns = 0f,
            int cryoChainDamagePercent = 0,
            float fpvControlBonusSeconds = 0f,
            float fpvDamageMultiplier = 1f,
            float fpvImpactRadiusMultiplier = 1f)
        {
            SalvoCount = Mathf.Max(1, salvoCount);
            SalvoIntervalSeconds = Mathf.Max(0f, salvoIntervalSeconds);
            KnockbackMultiplier = Mathf.Max(0f, knockbackMultiplier);
            BlasterBurstShotCount = Mathf.Max(1, blasterBurstShotCount);
            TerrainBounceLimit = Mathf.Max(0, terrainBounceLimit);
            DrillRange = Mathf.Max(0f, drillRange);
            DrillPenetration = drillPenetration;
            MortarFragmentCount = Mathf.Max(0, mortarFragmentCount);
            MortarCascadeCount = Mathf.Max(0, mortarCascadeCount);
            RocketDrillDurationSeconds = Mathf.Max(0f, rocketDrillDurationSeconds);
            RocketDrillPenetration = rocketDrillPenetration;
            MissileSplitChargeCount = Mathf.Max(0, missileSplitChargeCount);
            MissileObstacleBypassCount = Mathf.Max(0, missileObstacleBypassCount);
            C4DamageMultiplier = Mathf.Max(0f, c4DamageMultiplier);
            C4ImpactRadiusMultiplier = Mathf.Max(0.1f, c4ImpactRadiusMultiplier);
            GrenadeSticks = grenadeSticks;
            GrenadeMagneticSeekRadius = Mathf.Max(0f, grenadeMagneticSeekRadius);
            GrenadeGravityPullRadius = Mathf.Max(0f, grenadeGravityPullRadius);
            GrenadePullAcceleration = Mathf.Max(0f, grenadePullAcceleration);
            MineDeployLimitPerTurn = Mathf.Max(1, mineDeployLimitPerTurn);
            MineTriggersEnemiesOnly = mineTriggersEnemiesOnly;
            MineAlliesImmuneToExplosion = mineAlliesImmuneToExplosion;
            RepairProtectionTurns = Mathf.Max(0, repairProtectionTurns);
            RepairRegenerationTurns = Mathf.Max(0, repairRegenerationTurns);
            TeleportExchangesEnemy = teleportExchangesEnemy;
            TeleportExchangesAlliesAndObjects = teleportExchangesAlliesAndObjects;
            TeleportDisorientationTurns = Mathf.Max(0, teleportDisorientationTurns);
            TeleportImpulseRadius = Mathf.Max(0f, teleportImpulseRadius);
            TeleportImpulseStrength = Mathf.Max(0f, teleportImpulseStrength);
            ToxicArmorWeakenPercent = Mathf.Max(0, toxicArmorWeakenPercent);
            ToxicArmorWeakenTurns = Mathf.Max(0, toxicArmorWeakenTurns);
            ToxicMovementDamagePerSecond = Mathf.Max(0f, toxicMovementDamagePerSecond);
            ToxicDurationBonusTurns = Mathf.Max(0, toxicDurationBonusTurns);
            FireGunStreamDurationMultiplier = Mathf.Max(0.1f, fireGunStreamDurationMultiplier);
            FireGunBurnDurationMultiplier = Mathf.Max(0.1f, fireGunBurnDurationMultiplier);
            FireGunIgniteTurns = Mathf.Max(0, fireGunIgniteTurns);
            FireGunIgniteDamagePerTurn = Mathf.Max(0, fireGunIgniteDamagePerTurn);
            FireGunChainDamagePercent = Mathf.Max(0, fireGunChainDamagePercent);
            EmpStunTurns = Mathf.Max(0, empStunTurns);
            EmpStunBlocksMovement = empStunBlocksMovement;
            EmpWeakenDamagePercent = Mathf.Clamp(empWeakenDamagePercent, 0, 100);
            EmpWeakenTurns = Mathf.Max(0, empWeakenTurns);
            CryoFreezeTurns = Mathf.Max(0f, cryoFreezeTurns);
            CryoChainDamagePercent = Mathf.Max(0, cryoChainDamagePercent);
            FpvControlBonusSeconds = Mathf.Max(0f, fpvControlBonusSeconds);
            FpvDamageMultiplier = Mathf.Max(0f, fpvDamageMultiplier);
            FpvImpactRadiusMultiplier = Mathf.Max(0.1f, fpvImpactRadiusMultiplier);
        }

        public int SalvoCount { get; }
        public float SalvoIntervalSeconds { get; }
        public float KnockbackMultiplier { get; }
        public int BlasterBurstShotCount { get; }
        public int TerrainBounceLimit { get; }
        public float DrillRange { get; }
        public TerrainMaterial DrillPenetration { get; }
        public int MortarFragmentCount { get; }
        public int MortarCascadeCount { get; }
        public float RocketDrillDurationSeconds { get; }
        public TerrainMaterial RocketDrillPenetration { get; }
        public int MissileSplitChargeCount { get; }
        public int MissileObstacleBypassCount { get; }
        public float C4DamageMultiplier { get; }
        public float C4ImpactRadiusMultiplier { get; }
        public bool GrenadeSticks { get; }
        public float GrenadeMagneticSeekRadius { get; }
        public float GrenadeGravityPullRadius { get; }
        public float GrenadePullAcceleration { get; }
        public int MineDeployLimitPerTurn { get; }
        public bool MineTriggersEnemiesOnly { get; }
        public bool MineAlliesImmuneToExplosion { get; }
        public int RepairProtectionTurns { get; }
        public int RepairRegenerationTurns { get; }
        public bool TeleportExchangesEnemy { get; }
        public bool TeleportExchangesAlliesAndObjects { get; }
        public int TeleportDisorientationTurns { get; }
        public float TeleportImpulseRadius { get; }
        public float TeleportImpulseStrength { get; }
        public int ToxicArmorWeakenPercent { get; }
        public int ToxicArmorWeakenTurns { get; }
        public float ToxicMovementDamagePerSecond { get; }
        public int ToxicDurationBonusTurns { get; }
        public float FireGunStreamDurationMultiplier { get; }
        public float FireGunBurnDurationMultiplier { get; }
        public int FireGunIgniteTurns { get; }
        public int FireGunIgniteDamagePerTurn { get; }
        public int FireGunChainDamagePercent { get; }
        public int EmpStunTurns { get; }
        public bool EmpStunBlocksMovement { get; }
        public int EmpWeakenDamagePercent { get; }
        public int EmpWeakenTurns { get; }
        public float CryoFreezeTurns { get; }
        public int CryoChainDamagePercent { get; }
        public float FpvControlBonusSeconds { get; }
        public float FpvDamageMultiplier { get; }
        public float FpvImpactRadiusMultiplier { get; }

        public static WeaponEvolutionProfile Base => new(1, 0f, 1f, 3, 0);
    }

    public static class WeaponEvolutionRuntimeResolver
    {
        private const string ImpulseWeaponId = "weapon.impulse";
        private const string BlasterWeaponId = "weapon.blaster-caset";
        private const string BurWeaponId = "weapon.bur";
        private const string BurRocketWeaponId = "weapon.bur-rocket";
        private const string C4WeaponId = "weapon.c4";
        private const string GrenadeWeaponId = "weapon.granata";
        private const string MineWeaponId = "weapon.mine";
        private const string HealWeaponId = "weapon.heal";
        private const string TeleportWeaponId = "weapon.teleport";
        private const string MortarWeaponId = "weapon.mortira";
        private const string MissileWeaponId = "weapon.missle";
        private const string ToxicWeaponId = "weapon.toxic";
        private const string FireGunWeaponId = "weapon.fire-gun";
        private const string EmpWeaponId = "weapon.emp";
        private const string FreezeWeaponId = "weapon.freeze";
        private const string FpvWeaponId = "weapon.fpv-drone";
        private const float SalvoIntervalSeconds = 0.18f;
        private const float ImpactKnockbackMultiplier = 1.55f;
        private const float AssaultKnockbackMultiplier = 2.15f;

        public static WeaponEvolutionProfile Resolve(WeaponDefinition weapon, SaveWeaponProgressData progress)
        {
            if (weapon == null
                || progress == null
                || progress.rank < 5)
            {
                return WeaponEvolutionProfile.Base;
            }

            var branch = (WeaponUpgradeBranch)progress.branch;
            var selectedEvolutionRank = progress.rank;
            if (WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression)
                && WeaponEvolutionSelectionResolver.TryGetSelected(progression, progress, out var selected))
            {
                branch = selected.LegacyBranch;
                selectedEvolutionRank = selected.Rank;
            }
            if (weapon.WeaponId == BlasterWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        selectedEvolutionRank >= 10 ? 5 : 4);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        3,
                        selectedEvolutionRank >= 10 ? 2 : 1);
                }
            }

            if (weapon.WeaponId == BurWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        drillRange: selectedEvolutionRank >= 10 ? 5.65f : 4.45f);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        drillRange: selectedEvolutionRank >= 10 ? 4.1f : 3.6f,
                        drillPenetration: selectedEvolutionRank >= 10
                            ? TerrainMaterial.Titanium
                            : TerrainMaterial.LightMetal);
                }
            }

            if (weapon.WeaponId == BurRocketWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        rocketDrillDurationSeconds: selectedEvolutionRank >= 10 ? 2.25f : 1.5f,
                        rocketDrillPenetration: TerrainMaterial.LightMetal);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        rocketDrillDurationSeconds: 1f,
                        rocketDrillPenetration: selectedEvolutionRank >= 10
                            ? TerrainMaterial.Titanium
                            : TerrainMaterial.LightMetal);
                }
            }

            if (weapon.WeaponId == C4WeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        c4DamageMultiplier: selectedEvolutionRank >= 10 ? 1.3f : 1.15f,
                        c4ImpactRadiusMultiplier: selectedEvolutionRank >= 10 ? 1.55f : 1.3f);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        selectedEvolutionRank >= 10 ? 3f : 2.15f,
                        c4ImpactRadiusMultiplier: selectedEvolutionRank >= 10 ? 0.85f : 0.93f);
                }
            }

            if (weapon.WeaponId == GrenadeWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        grenadeSticks: true,
                        grenadeMagneticSeekRadius: selectedEvolutionRank >= 10 ? 2.4f : 0f);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        grenadeGravityPullRadius: selectedEvolutionRank >= 10 ? 5f : 3.5f,
                        grenadePullAcceleration: 8f);
                }
            }

            if (weapon.WeaponId == MineWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        mineDeployLimitPerTurn: selectedEvolutionRank >= 10 ? 5 : 4);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        mineTriggersEnemiesOnly: true,
                        mineAlliesImmuneToExplosion: selectedEvolutionRank >= 10);
                }
            }

            if (weapon.WeaponId == HealWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        repairProtectionTurns: selectedEvolutionRank >= 10 ? 2 : 1);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        repairRegenerationTurns: selectedEvolutionRank >= 10 ? 2 : 1);
                }
            }

            if (weapon.WeaponId == TeleportWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        teleportExchangesEnemy: true,
                        teleportExchangesAlliesAndObjects: selectedEvolutionRank >= 10,
                        teleportDisorientationTurns: 1);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        teleportImpulseRadius: selectedEvolutionRank >= 10 ? 4.2f : 2.7f,
                        teleportImpulseStrength: selectedEvolutionRank >= 10 ? 7.5f : 4.5f);
                }
            }

            if (weapon.WeaponId == MortarWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        mortarFragmentCount: selectedEvolutionRank >= 10 ? 5 : 4);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        mortarCascadeCount: selectedEvolutionRank >= 10 ? 2 : 1);
                }
            }

            if (weapon.WeaponId == MissileWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        missileSplitChargeCount: selectedEvolutionRank >= 10 ? 3 : 2);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        missileObstacleBypassCount: selectedEvolutionRank >= 10 ? 2 : 1);
                }
            }

            if (weapon.WeaponId == ToxicWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        toxicArmorWeakenPercent: selectedEvolutionRank >= 10 ? 35 : 20,
                        toxicArmorWeakenTurns: selectedEvolutionRank >= 10 ? 3 : 2);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(
                        1,
                        0f,
                        1f,
                        toxicMovementDamagePerSecond: selectedEvolutionRank >= 10 ? 10f : 6f,
                        toxicDurationBonusTurns: selectedEvolutionRank >= 10 ? 1 : 0);
                }
            }

            if (weapon.WeaponId == FireGunWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        fireGunStreamDurationMultiplier: 1.4f,
                        fireGunBurnDurationMultiplier: selectedEvolutionRank >= 10 ? 1.5f : 1f);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        fireGunIgniteTurns: selectedEvolutionRank >= 10 ? 3 : 2,
                        fireGunIgniteDamagePerTurn: selectedEvolutionRank >= 10 ? 9 : 6,
                        fireGunChainDamagePercent: selectedEvolutionRank >= 10 ? 60 : 35);
                }
            }

            if (weapon.WeaponId == EmpWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        empStunTurns: 1,
                        empStunBlocksMovement: selectedEvolutionRank >= 10);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        empWeakenDamagePercent: selectedEvolutionRank >= 10 ? 35 : 20,
                        empWeakenTurns: selectedEvolutionRank >= 10 ? 3 : 2);
                }
            }

            if (weapon.WeaponId == FreezeWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        cryoFreezeTurns: selectedEvolutionRank >= 10 ? 3f : 2f);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        cryoFreezeTurns: 1f,
                        cryoChainDamagePercent: selectedEvolutionRank >= 10 ? 60 : 35);
                }
            }

            if (weapon.WeaponId == FpvWeaponId)
            {
                if (branch == WeaponUpgradeBranch.A)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        fpvControlBonusSeconds: selectedEvolutionRank >= 10 ? 4f : 2f);
                }

                if (branch == WeaponUpgradeBranch.B)
                {
                    return new WeaponEvolutionProfile(1, 0f, 1f,
                        fpvDamageMultiplier: selectedEvolutionRank >= 10 ? 1.35f : 1.2f,
                        fpvImpactRadiusMultiplier: selectedEvolutionRank >= 10 ? 1.3f : 1.15f);
                }
            }

            if (weapon.WeaponId != ImpulseWeaponId)
            {
                return WeaponEvolutionProfile.Base;
            }

            if (branch == WeaponUpgradeBranch.A)
            {
                return new WeaponEvolutionProfile(selectedEvolutionRank >= 10 ? 3 : 2, SalvoIntervalSeconds, 1f);
            }

            if (branch == WeaponUpgradeBranch.B)
            {
                return new WeaponEvolutionProfile(1, 0f,
                    selectedEvolutionRank >= 10 ? AssaultKnockbackMultiplier : ImpactKnockbackMultiplier);
            }

            return WeaponEvolutionProfile.Base;
        }
    }
}
