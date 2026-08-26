using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;

namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterValueHelper
    {
        public static double GetValue(this Colony colony, GameParameterType parameterType)
        {
            var colonyState = colony.State;
            return parameterType switch
            {
                GameParameterType.SolarsCurrent => colonyState.Resources.Solars.Value,
                GameParameterType.SolarsDelta => colony.GetSolarDelta().Value,

                GameParameterType.SolarDeltaIndustriesPrivate => GameParameterHelper.GetSolarDeltaIndustries(colony, isPrivate: true).Value,
                GameParameterType.SolarDeltaIndustriesState => GameParameterHelper.GetSolarDeltaIndustries(colony, isPrivate: false).Value,
                GameParameterType.PublicDebtService => GameParameterHelper.GetPublicDebtService(colony).Value,
                GameParameterType.AdministrationSalary => GameParameterHelper.GetAdministrationSalary(colony).Value,
                GameParameterType.PopulationTaxSolars => GameParameterHelper.GetPopulationTaxSolars(colony).Value,

                GameParameterType.ActionPointsCurrent => colonyState.Resources.ActionPoints.Value,
                GameParameterType.ActionPointsDelta => colonyState.Resources.ActionPoints.GetDeltaPerTurn(colonyState),

                GameParameterType.MoodCurrent => colonyState.Resources.Mood.Value,
                GameParameterType.MoodDelta => colonyState.Resources.Mood.GetDeltaPerTurn(colonyState),

                GameParameterType.TurnsCurrent => colonyState.Resources.TurnNumber.Value,

                GameParameterType.ModulesTotal => colonyState.Slots[ColonySlotType.Modules].GetTotal(colonyState),
                GameParameterType.ModulesUsed => colonyState.Slots[ColonySlotType.Modules].GetUsed(colonyState),

                GameParameterType.MiningSlotsFree => colonyState.Slots[ColonySlotType.Mining].GetFree(colonyState),

                GameParameterType.ReformsTaxLevel => colonyState.Reforms[ColonyReformType.TaxLevel].Value,
                GameParameterType.ReformsSocialGuaranteesLevel => colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value,

                GameParameterType.Population => colonyState.GetPopulation(),
            };
        }
    }
}
