using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameActions;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Icons;

namespace YAGO.World.Host.Controllers.Common.GameRequirements
{
    internal static class GameRequirementMapping
    {
        public static GameRequirementResponse ToResponse(
            this GameRequirement requirement,
            ColonyState colonyStats)
        {
            var iconType = requirement.Type.ToIcon();
            var label = GetLabel(requirement);
            var value = GetValue(requirement);
            var isMet = requirement.Check(colonyStats);
            return new GameRequirementResponse(
                iconType,
                label,
                value,
                isMet,
                Url: null,
                InfoUrl: null);
        }

        private static string GetLabel(GameRequirement requirement)
        {
            return requirement.Type switch
            {
                GameRequirementType.SolarsCanSpend or
                GameRequirementType.SolarsLessThan => "Солары",
                GameRequirementType.ActionPointsCanSpend => "Очки действий",
                GameRequirementType.MoodLessThan => "Доверие:",
                GameRequirementType.ModulesFreeCanSpend => "Модули",
                GameRequirementType.ModulesUsedMoreThan => "Занято модулей",
                GameRequirementType.CreditCanTake => "Одобрение кредита",
                _ => throw new System.NotImplementedException(),
            };
        }

        private static string GetValue(GameRequirement requirement)
        {
            return requirement.Type switch
            {
                GameRequirementType.SolarsCanSpend or
                GameRequirementType.ActionPointsCanSpend or
                GameRequirementType.ModulesFreeCanSpend => requirement.RequirementValue.ToBeautifulString(),
                GameRequirementType.SolarsLessThan or
                GameRequirementType.MoodLessThan => $"не более {requirement.RequirementValue.ToBeautifulString()}",
                GameRequirementType.ModulesUsedMoreThan => $"не менее {requirement.RequirementValue.ToBeautifulString()}",
                GameRequirementType.CreditCanTake => "имеется",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}