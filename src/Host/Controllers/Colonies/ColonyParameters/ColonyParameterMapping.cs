using System.Collections.Generic;
using YAGO.World.Domain.GameActions;
using YAGO.World.Host.Controllers.Common;

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
                    colonyStatChange.GetChangeString())
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
