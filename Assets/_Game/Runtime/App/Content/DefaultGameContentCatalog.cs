using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using Game.Progression.Missions;
using Game.Core.Localization;

namespace Game.App.Content
{
    public static class DefaultGameContentCatalog
    {
        public static RuntimeGameContentCatalog Create()
        {
            return new RuntimeGameContentCatalog(
                PrototypeWeaponLibrary.CreateAll(),
                PrototypeArenaLibrary.CreateAll(),
                MvpMissionLibrary.CreateAll(),
                CreateCampaignChapters(),
                CreateEnemyLoadouts(),
                CreateDifficultyProfiles(),
                CreateAiProfiles(),
                CreateUnitArchetypes());
        }

        private static CampaignChapterDefinition[] CreateCampaignChapters()
        {
            return new[]
            {
                CampaignChapterDefinition.CreateRuntime(
                    "chapter.basics",
                    GameText.Localized("campaignText.chapter.basics.title", "Campaign"),
                    GameText.Localized("campaignText.chapter.basics.summary", "Mission briefing"),
                    new[]
                    {
                        "mission.training.01",
                        "mission.training.02",
                        "mission.training.03",
                        "mission.training.04",
                        "mission.training.05"
                    },
                    new[]
                    {
                        "tutorial.aim",
                        "tutorial.fire",
                        "tutorial.arc",
                        "tutorial.power",
                        "tutorial.weapon-switch"
                    },
                    new[]
                    {
                        "weapon.impulse",
                        "weapon.blaster-caset",
                        "weapon.bur",
                        "weapon.granata",
                        "weapon.mine"
                    }),
                CampaignChapterDefinition.CreateRuntime(
                    "chapter.tactical-tools",
                    GameText.Localized("campaignText.chapter.tactical-tools.title", "Campaign"),
                    GameText.Localized("campaignText.chapter.tactical-tools.summary", "Mission briefing"),
                    new[]
                    {
                        "mission.training.06",
                        "mission.training.07",
                        "mission.training.08"
                    },
                    new[]
                    {
                        "tutorial.area-choice",
                        "tutorial.weapon-switch",
                        "tutorial.precision",
                        "tutorial.review"
                    },
                    new[]
                    {
                        "weapon.granata",
                        "weapon.mine",
                        "weapon.c4",
                        "weapon.heal",
                        "weapon.teleport"
                    }),
                CampaignChapterDefinition.CreateRuntime(
                    "chapter.city",
                    GameText.Localized("campaign.defaultTitle", "Campaign"),
                    GameText.Localized("campaign.defaultSummary", "Mission briefing"),
                    new[] { "mission.training.09", "mission.training.10" },
                    new[] { "tutorial.review" },
                    new[] { "weapon.blaster-caset", "weapon.granata", "weapon.bur", "weapon.c4" }),
                CampaignChapterDefinition.CreateRuntime(
                    "chapter.grib",
                    GameText.Localized("campaign.defaultTitle", "Campaign"),
                    GameText.Localized("campaign.defaultSummary", "Mission briefing"),
                    new[] { "mission.training.11", "mission.training.12" },
                    new[] { "tutorial.review" },
                    new[] { "weapon.granata", "weapon.bur", "weapon.mine", "weapon.heal", "weapon.teleport" }),
                CampaignChapterDefinition.CreateRuntime(
                    "chapter.laguna",
                    GameText.Localized("campaign.defaultTitle", "Campaign"),
                    GameText.Localized("campaign.defaultSummary", "Mission briefing"),
                    new[] { "mission.training.13", "mission.training.14" },
                    new[] { "tutorial.review" },
                    new[] { "weapon.granata", "weapon.bur", "weapon.c4", "weapon.heal", "weapon.teleport" })
            };
        }

        private static EnemyLoadoutDefinition[] CreateEnemyLoadouts()
        {
            return new[]
            {
                EnemyLoadoutDefinition.CreateRuntime(
                    "enemy.training.direct",
                    "Direct Basics",
                    new[] { "weapon.impulse", "weapon.blaster-caset" }),
                EnemyLoadoutDefinition.CreateRuntime(
                    "enemy.training.arc",
                    "Arc Pressure",
                    new[] { "weapon.impulse", "weapon.granata" }),
                EnemyLoadoutDefinition.CreateRuntime(
                    "enemy.training.precision",
                    "Precision Mix",
                    new[] { "weapon.impulse", "weapon.bur" }),
                EnemyLoadoutDefinition.CreateRuntime(
                    "enemy.training.utility",
                    "Utility Pressure",
                    new[] { "weapon.mine", "weapon.granata", "weapon.heal" })
            };
        }

        private static DifficultyProfileDefinition[] CreateDifficultyProfiles()
        {
            return new[]
            {
                DifficultyProfileDefinition.CreateRuntime("difficulty.easy", 0, "ai.easy", "archetype.skirmisher"),
                DifficultyProfileDefinition.CreateRuntime("difficulty.normal", 1, "ai.normal", "archetype.striker"),
                DifficultyProfileDefinition.CreateRuntime("difficulty.hard", 2, "ai.hard", "archetype.bulwark")
            };
        }

        private static AiProfileDefinition[] CreateAiProfiles()
        {
            return new[]
            {
                AiProfileDefinition.CreateRuntime("ai.easy", 6f, 10f, new[] { "starter", "direct" }, 0.35f, 0.1f, 0.25f, 0.05f, 0.05f, 0.75f),
                AiProfileDefinition.CreateRuntime("ai.normal", 2.5f, 4f, new[] { "starter", "arc", "precision" }, 0.55f, 0.18f, 0.4f, 0.12f, 0.1f, 0.6f),
                AiProfileDefinition.CreateRuntime("ai.hard", 0.5f, 1.5f, new[] { "precision", "area", "utility" }, 0.72f, 0.28f, 0.55f, 0.22f, 0.14f, 0.45f)
            };
        }

        private static UnitArchetypeDefinition[] CreateUnitArchetypes()
        {
            var striker = PrototypeArchetypeLibrary.Striker;
            var skirmisher = PrototypeArchetypeLibrary.Skirmisher;
            var bulwark = PrototypeArchetypeLibrary.Bulwark;
            return new[]
            {
                UnitArchetypeDefinition.CreateRuntime(
                    striker.Id,
                    striker.DisplayName,
                    striker.MaxHealth,
                    striker.BodyHeight,
                    striker.WalkBudget,
                    striker.WalkSpeed,
                    striker.WalkStepHeight,
                    striker.FrontJumpDistance,
                    striker.FrontJumpHeight,
                    striker.BackJumpDistance,
                    striker.BackJumpHeight,
                    striker.MaxWalkSlopeDegrees),
                UnitArchetypeDefinition.CreateRuntime(
                    skirmisher.Id,
                    skirmisher.DisplayName,
                    skirmisher.MaxHealth,
                    skirmisher.BodyHeight,
                    skirmisher.WalkBudget,
                    skirmisher.WalkSpeed,
                    skirmisher.WalkStepHeight,
                    skirmisher.FrontJumpDistance,
                    skirmisher.FrontJumpHeight,
                    skirmisher.BackJumpDistance,
                    skirmisher.BackJumpHeight,
                    skirmisher.MaxWalkSlopeDegrees),
                UnitArchetypeDefinition.CreateRuntime(
                    bulwark.Id,
                    bulwark.DisplayName,
                    bulwark.MaxHealth,
                    bulwark.BodyHeight,
                    bulwark.WalkBudget,
                    bulwark.WalkSpeed,
                    bulwark.WalkStepHeight,
                    bulwark.FrontJumpDistance,
                    bulwark.FrontJumpHeight,
                    bulwark.BackJumpDistance,
                    bulwark.BackJumpHeight,
                    bulwark.MaxWalkSlopeDegrees)
            };
        }
    }
}
