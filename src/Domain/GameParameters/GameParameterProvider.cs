using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterProvider
    {
        public static double GetValue(this Colony colony, GameParameterType parameterType)
        {
            var colonyState = colony.State;
            return parameterType switch
            {
                GameParameterType.SolarsCurrent => colonyState.Resources.Solars.Value,
                GameParameterType.SolarsDelta => colony.GetSolarDelta().Value,

                GameParameterType.ActionPointsCurrent => colonyState.Resources.ActionPoints.Value,
                GameParameterType.ActionPointsDelta => colonyState.Resources.ActionPoints.GetDeltaPerTurn(colonyState),

                GameParameterType.MoodCurrent => colonyState.Resources.Mood.Value,
                GameParameterType.MoodDelta => colonyState.Resources.Mood.GetDeltaPerTurn(colonyState),

                GameParameterType.TurnsCurrent => colonyState.Resources.TurnNumber.Value,

                GameParameterType.ModulesTotal => colonyState.Slots[ColonySlotType.Modules].GetTotal(colonyState),
                GameParameterType.ModulesUsed => colonyState.Slots[ColonySlotType.Modules].GetUsed(colonyState),
                GameParameterType.ModulesFree => colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState),

                GameParameterType.MiningSlotsTotal => colonyState.Slots[ColonySlotType.Mining].GetTotal(colonyState),
                GameParameterType.MiningSlotsUsed => colonyState.Slots[ColonySlotType.Mining].GetUsed(colonyState),
                GameParameterType.MiningSlotsFree => colonyState.Slots[ColonySlotType.Mining].GetFree(colonyState),

                GameParameterType.ReformsTaxLevel => colonyState.Reforms[ColonyReformType.TaxLevel].Value,
                GameParameterType.ReformsSocialGuaranteesLevel => colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value,
                GameParameterType.PublicDebt => colonyState.Reforms[ColonyReformType.PublicDebt].Value,

                GameParameterType.BuildingsAdministrativePrivate => colonyState.Industries[ColonyIndustryType.Administrative].PrivateCount,
                GameParameterType.BuildingsAdministrativeState => colonyState.Industries[ColonyIndustryType.Administrative].StateCount,
                GameParameterType.BuildingsAdministrativeTotal => colonyState.Industries[ColonyIndustryType.Administrative].Total,

                GameParameterType.BuildingsMiningPrivate => colonyState.Industries[ColonyIndustryType.Mining].PrivateCount,
                GameParameterType.BuildingsMiningState => colonyState.Industries[ColonyIndustryType.Mining].StateCount,
                GameParameterType.BuildingsMiningTotal => colonyState.Industries[ColonyIndustryType.Mining].Total,

                GameParameterType.BuildingsProductionPrivate => colonyState.Industries[ColonyIndustryType.Production].PrivateCount,
                GameParameterType.BuildingsProductionState => colonyState.Industries[ColonyIndustryType.Production].StateCount,
                GameParameterType.BuildingsProductionTotal => colonyState.Industries[ColonyIndustryType.Production].Total,

                GameParameterType.BuildingsServicePrivate => colonyState.Industries[ColonyIndustryType.Service].PrivateCount,
                GameParameterType.BuildingsServiceState => colonyState.Industries[ColonyIndustryType.Service].StateCount,
                GameParameterType.BuildingsServiceTotal => colonyState.Industries[ColonyIndustryType.Service].Total,

                GameParameterType.Population => colonyState.GetPopulation(),
                GameParameterType.Attractiveness => colonyState.GetAttractiveness(),
                GameParameterType.ServiceNeed => colonyState.GetServiceNeed(),

                _ => throw new YagoUnknownTypeException(parameterType.ToString())
            };
        }
    }
}
