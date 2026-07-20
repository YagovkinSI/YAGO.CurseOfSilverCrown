using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public static class ColonyParameterResponseMapping
    {
        public static IReadOnlyList<ColonyParameterResponse> ToColonyParameters(Colony colony)
        {
            var colonyPatameters = new List<ColonyParameterResponse>();

            var mainPatameters = AddMainParameters(colony);
            colonyPatameters.AddRange(mainPatameters);

            var additionalPatameters = AddAdditionalParameters(colony);
            colonyPatameters.AddRange(additionalPatameters);

            return [.. colonyPatameters.OrderBy(x => x.Weight)];
        }

        private static List<ColonyParameterResponse> AddMainParameters(Colony colony)
        {
            var mainPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.Stats;
            var colonyResources = colonyStats.Resources;

            mainPatameters.AddRange(
                ColonyParameterResponse.ActionPoints(colonyResources.ActionPoints.Value, colonyResources.ActionPoints.MaxValue, colonyStats.ActionPointsTrend),
                ColonyParameterResponse.Finance(colonyResources.Solars, colonyStats.GetSolarsIncome()),
                ColonyParameterResponse.Other());

            if (colony.Stats.GetPopulation() > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyStats.GdpCalc(), colonyStats.GdpTrendCalc()),
                    ColonyParameterResponse.Trust(colonyStats.MoodTotal.Value, colonyStats.MoodTotalBalanceCacl()),
                    ColonyParameterResponse.Area(colonyStats.GetZonesOccupied(), colonyResources.ZonesTotal));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.Stats;
            var currentWeek = colonyStats.CurrentWeek;
            var colonySettings = colonyStats.Settings;

            additionalPatameters.AddRange(
                    ColonyParameterResponse.Station(colonySettings.GetShipName(), colonySettings.ShipId),
                    ColonyParameterResponse.CurrentWeek(currentWeek));

            var population = colonyStats.GetPopulation();
            if (population > 0)
            {
                additionalPatameters.AddRange(
                    ColonyParameterResponse.Attractiveness(colonyStats.AttractivenessTotalCalc()),
                    ColonyParameterResponse.Population(population),
                    ColonyParameterResponse.CodeOfLaws(colonySettings.GetCodeOfLaws()));
            }

            return additionalPatameters;
        }
    }
}
