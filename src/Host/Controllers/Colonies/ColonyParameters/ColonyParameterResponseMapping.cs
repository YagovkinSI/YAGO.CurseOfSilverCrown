using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameParameters;
using YAGO.World.Host.Controllers.Colonies.Models;

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

            var colonyState = colony.State;

            mainPatameters.AddRange(
                ColonyParameterResponse.ActionPoints(
                    (int)colony.GetValue(GameParameterType.ActionPointsCurrent),
                    colonyState.Resources.ActionPoints.MaxValue,
                    (int)colony.GetValue(GameParameterType.ActionPointsDelta)),
                ColonyParameterResponse.Finance(
                    colony.GetValue(GameParameterType.SolarsCurrent),
                    colony.GetValue(GameParameterType.SolarsDelta)),
                ColonyParameterResponse.Other());

            if (colony.State.GetPopulation() > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyState.GetGdp()),
                    ColonyParameterResponse.Trust(
                        colony.GetValue(GameParameterType.MoodCurrent),
                        colony.GetValue(GameParameterType.MoodDelta)),
                    ColonyParameterResponse.Area(
                        (int)colony.GetValue(GameParameterType.ModulesUsed),
                        (int)colony.GetValue(GameParameterType.ModulesTotal)));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.State;
            var currentWeek = (int)colony.GetValue(GameParameterType.TurnsCurrent);

            additionalPatameters.AddRange(
                    ColonyParameterResponse.Station("Рассвет-342", 1),
                    ColonyParameterResponse.CurrentWeek(currentWeek));

            var population = colonyStats.GetPopulation();
            if (population > 0)
            {
                additionalPatameters.AddRange(
                    ColonyParameterResponse.Attractiveness(colonyStats.GetAttractiveness()),
                    ColonyParameterResponse.Population(population),
                    ColonyParameterResponse.CodeOfLaws(GetCodeOfLaws(colony)));
            }

            return additionalPatameters;
        }

        private static CodeOfLaws GetCodeOfLaws(Colony colony)
        {
            var humanism = colony.GetValue(GameParameterType.ReformsSocialGuaranteesLevel) -
                colony.GetValue(GameParameterType.ReformsTaxLevel);
            return humanism switch
            {
                > 1 => CodeOfLaws.Humanist,
                < -1 => CodeOfLaws.Capitalist,
                _ => CodeOfLaws.Centrist
            };
        }
    }
}
