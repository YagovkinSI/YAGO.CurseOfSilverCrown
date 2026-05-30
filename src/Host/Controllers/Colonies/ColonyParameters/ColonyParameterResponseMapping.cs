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
                ColonyParameterResponse.ColonyName(colony.Name),
                ColonyParameterResponse.ActionPoints(colonyResources.ActionPoints.Value, colonyResources.ActionPoints.MaxValue, colonyStats.ActionPointsTrend),
                ColonyParameterResponse.Finance(colonyResources.Solars, colonyStats.BudgetBalance),
                ColonyParameterResponse.Other());

            if (colony.Stats.PopulationTotal > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyStats.GdpCalc(), colonyStats.GdpTrendCalc()),
                    ColonyParameterResponse.Trust(colonyStats.MoodTotal.Value, colonyStats.MoodTotalBalanceCacl()),
                    ColonyParameterResponse.Area(colonyStats.ZonesOccupied, colonyResources.ZonesTotal));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.Stats;
            var episodeCount = colonyStats.EpisodeCount;
            var colonySettings = colonyStats.Settings;

            if (episodeCount > 0)
            {
                additionalPatameters.AddRange(
                    ColonyParameterResponse.Station(colonySettings.GetShipName(), colonySettings.ShipId),
                    ColonyParameterResponse.EpisodeCount(episodeCount));
            }
            if (episodeCount > 1)
            {
                additionalPatameters.AddRange(
                    ColonyParameterResponse.Attractiveness(colonyStats.AttractivenessTotalCalc()),
                    ColonyParameterResponse.Population(colonyStats.PopulationTotal),
                    ColonyParameterResponse.CodeOfLaws(colonySettings.GetCodeOfLaws()));
            }

            return additionalPatameters;
        }
    }
}
