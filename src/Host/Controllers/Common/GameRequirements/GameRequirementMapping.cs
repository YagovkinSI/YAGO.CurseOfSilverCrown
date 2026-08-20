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
            var status = requirement.Check(colonyStats);
            return new GameRequirementResponse(
                iconType,
                label,
                value,
                status,
                Url: null,
                InfoUrl: null);
        }

        private static string GetLabel(GameRequirement requirement)
        {
            return requirement.Type switch
            {
                GameRequirementType.SolarsCanSpend => "Солары (будут потрачены):",
                GameRequirementType.SolarsLessThan => "Солары:",
                GameRequirementType.ActionPointsCanSpend => "Очки действий (будут потрачены):",
                GameRequirementType.MoodLessThan => "Доверие:",
                GameRequirementType.ModulesFreeCanSpend => "Свободные модули (будут заняты):",
                GameRequirementType.ModulesUsedMoreThan => "Занято модулей:",
                GameRequirementType.CreditCanTake => "Кредит:",
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
                GameRequirementType.CreditCanTake => "доступен",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}