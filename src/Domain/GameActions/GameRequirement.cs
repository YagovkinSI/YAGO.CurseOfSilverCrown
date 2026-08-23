using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public class GameRequirement
    {
        public GameRequirementType Type { get; }
        public double RequirementValue { get; }
        public string Achievement { get; }

        public GameRequirement(
            GameRequirementType type,
            double? requirementValue = null,
            string? achievement = null)
        {
            Type = type;
            RequirementValue = requirementValue ?? 0;
            Achievement = achievement ?? string.Empty;
        }

        public bool Check(ColonyState colonyState)
        {
            return Type switch
            {
                GameRequirementType.SolarsCanSpend =>
                    colonyState.Resources.Solars.Value >= RequirementValue,
                GameRequirementType.SolarsLessThan =>
                    colonyState.Resources.Solars.Value <= RequirementValue,
                GameRequirementType.CreditCanTake =>
                    colonyState.GetPublicDebt().Check(RequirementValue),
                GameRequirementType.ActionPointsCanSpend =>
                    colonyState.Resources.ActionPoints.Value >= RequirementValue,
                GameRequirementType.MoodLessThan =>
                    colonyState.Resources.Mood.Value <= RequirementValue,
                GameRequirementType.ModulesFreeCanSpend =>
                    colonyState.Slots[Colonies.Slots.ColonySlotType.Modules].GetFree(colonyState) >= RequirementValue,
                GameRequirementType.ModulesUsedMoreThan =>
                    colonyState.Slots[Colonies.Slots.ColonySlotType.Modules].GetUsed(colonyState) >= RequirementValue,
                GameRequirementType.DoesntHaveAchievement =>
                    !colonyState.Achievements.HasAchievement(Achievement),
                _ => throw new System.NotImplementedException(),
            };
        }

        public static GameRequirement SolarsMoreThan(int solars)
        {
            return new GameRequirement(
                GameRequirementType.SolarsCanSpend, solars);
        }

        public static GameRequirement ActionPointsMoreThan(int actionPoints)
        {
            return new GameRequirement(
                GameRequirementType.ActionPointsCanSpend, actionPoints);
        }

        public static GameRequirement ModulesFreeMoreThan(int modules)
        {
            return new GameRequirement(
                GameRequirementType.ModulesFreeCanSpend, modules);
        }
    }
}
