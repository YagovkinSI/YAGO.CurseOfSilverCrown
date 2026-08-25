using System.Collections.Generic;
using YAGO.World.Domain.GameParameters;
using YAGO.World.Host.Controllers.Common.Extensions;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public static class ColonyParameterMapping
    {
        public static ColonyParameterResponse MapToColonyPatameters(this KeyValuePair<GameParameterType, double[]> colonyStatChange)
        {
            return colonyStatChange.Key switch
            {
                GameParameterType.ModulesUsed => new ColonyParameterResponse(
                    ColonyParameterNames.AreaCapacity_Occupied,
                    StatMenus: [], Weight: 0,
                    "Занято зон",
                    colonyStatChange.GetChangeString()),
                GameParameterType.SolarsCurrent => new ColonyParameterResponse(
                    ColonyParameterNames.Economic_Reserves,
                    StatMenus: [], Weight: 0,
                    "Солары",
                    colonyStatChange.GetChangeString()),
                GameParameterType.SolarsDelta => new ColonyParameterResponse(
                    ColonyParameterNames.AreaCapacity_Occupied,
                    StatMenus: [], Weight: 0,
                    "Солары за ход",
                    colonyStatChange.GetChangeString()),
                GameParameterType.MoodCurrent => new ColonyParameterResponse(
                    ColonyParameterNames.AreaCapacity_Occupied,
                    StatMenus: [], Weight: 0,
                    "Доверие",
                    colonyStatChange.GetChangeString()),
                GameParameterType.Population => new ColonyParameterResponse(
                    ColonyParameterNames.Population_Total,
                    StatMenus: [], Weight: 0,
                    "Население",
                    colonyStatChange.GetChangeString()),
                GameParameterType.ActionPointsCurrent => throw new System.NotImplementedException(),
                GameParameterType.ActionPointsDelta => throw new System.NotImplementedException(),
                GameParameterType.ModulesTotal => throw new System.NotImplementedException(),
                GameParameterType.ModulesFree => throw new System.NotImplementedException(),
                GameParameterType.MoodDelta => throw new System.NotImplementedException(),
                GameParameterType.MiningSlotsTotal => throw new System.NotImplementedException(),
                GameParameterType.MiningSlotsUsed => throw new System.NotImplementedException(),
                GameParameterType.MiningSlotsFree => throw new System.NotImplementedException(),
                GameParameterType.TurnsCurrent => throw new System.NotImplementedException(),
                GameParameterType.Attractiveness => throw new System.NotImplementedException(),
                GameParameterType.ServiceNeed => throw new System.NotImplementedException(),
                GameParameterType.ReformsTaxLevel => throw new System.NotImplementedException(),
                GameParameterType.ReformsSocialGuaranteesLevel => throw new System.NotImplementedException(),
                GameParameterType.PublicDebt => throw new System.NotImplementedException(),
                GameParameterType.BuildingsAdministrativeState => throw new System.NotImplementedException(),
                GameParameterType.BuildingsAdministrativePrivate => throw new System.NotImplementedException(),
                GameParameterType.BuildingsAdministrativeTotal => throw new System.NotImplementedException(),
                GameParameterType.BuildingsMiningState => throw new System.NotImplementedException(),
                GameParameterType.BuildingsMiningPrivate => throw new System.NotImplementedException(),
                GameParameterType.BuildingsMiningTotal => throw new System.NotImplementedException(),
                GameParameterType.BuildingsProductionState => throw new System.NotImplementedException(),
                GameParameterType.BuildingsProductionPrivate => throw new System.NotImplementedException(),
                GameParameterType.BuildingsProductionTotal => throw new System.NotImplementedException(),
                GameParameterType.BuildingsServiceState => throw new System.NotImplementedException(),
                GameParameterType.BuildingsServicePrivate => throw new System.NotImplementedException(),
                GameParameterType.BuildingsServiceTotal => throw new System.NotImplementedException()
            };
        }

        private static string GetChangeString(this KeyValuePair<GameParameterType, double[]> colonyStatChange)
        {
            if (colonyStatChange.Value.Length > 1)
            {
                var before = colonyStatChange.Value[0];
                var after = colonyStatChange.Value[1];
                var change = after - before;
                return $"{(change > 0 ? "+" : "")}{change.ToBeautifulString()} " +
                    $"({before.ToBeautifulString()} -> {after.ToBeautifulString()})";
            }
            else
            {
                var change = colonyStatChange.Value[0];
                return $"{(change > 0 ? "+" : "")}{change.ToBeautifulString()}";
            }
        }
    }
}
