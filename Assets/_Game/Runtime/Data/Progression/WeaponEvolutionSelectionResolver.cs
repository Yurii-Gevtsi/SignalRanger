using Game.Data.Save;

namespace Game.Data.Progression
{
    /// <summary>
    /// Resolves the current weapon identity from independent rank-5/rank-10
    /// evolution purchases. The latest purchased milestone is authoritative.
    /// </summary>
    public static class WeaponEvolutionSelectionResolver
    {
        public static bool TryGetSelected(
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress,
            out WeaponEvolutionOptionDefinition option)
        {
            if (definition == null || progress == null)
            {
                option = default;
                return false;
            }

            if (progress.rank >= 10 && TryGetChoice(definition, progress, 10, out option))
            {
                return true;
            }

            if (progress.rank >= 5 && TryGetChoice(definition, progress, 5, out option))
            {
                return true;
            }

            option = default;
            return false;
        }

        public static bool TryGetChoice(
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress,
            int rank,
            out WeaponEvolutionOptionDefinition option)
        {
            if (definition != null && progress?.evolutionChoices != null)
            {
                for (var index = 0; index < progress.evolutionChoices.Count; index++)
                {
                    var choice = progress.evolutionChoices[index];
                    if (choice?.rank == rank && definition.TryGetEvolutionOption(rank, choice.optionId, out option))
                    {
                        return true;
                    }
                }
            }

            // Older profiles stored one branch for both milestones. Retain that
            // representation until the player makes a new independent choice.
            var legacyBranch = progress == null ? WeaponUpgradeBranch.None : (WeaponUpgradeBranch)progress.branch;
            if (definition != null && legacyBranch != WeaponUpgradeBranch.None
                && definition.TryGetNode(rank, legacyBranch, out var legacy)
                && legacy.IsEvolution && !string.IsNullOrWhiteSpace(legacy.MechanicId))
            {
                option = new WeaponEvolutionOptionDefinition(legacy);
                return true;
            }

            option = default;
            return false;
        }
    }
}
