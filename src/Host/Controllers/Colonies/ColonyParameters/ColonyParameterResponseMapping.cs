using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.GameActions;
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

            var colonyStats = colony.State;

            mainPatameters.AddRange(
                ColonyParameterResponse.ActionPoints(
                    (int)colonyStats.GetValue(GameParameterType.ActionPointsCurrent),
                    (int)colonyStats.Resources.ActionPoints.MaxValue,
                    (int)colonyStats.GetValue(GameParameterType.ActionPointsDelta)),
                ColonyParameterResponse.Finance(
                    colonyStats.GetValue(GameParameterType.SolarsCurrent),
                    colonyStats.GetValue(GameParameterType.SolarsDelta)),
                ColonyParameterResponse.Other());

            if (colony.State.GetPopulation() > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyStats.GetGdp()),
                    ColonyParameterResponse.Trust(
                        colonyStats.GetValue(GameParameterType.MoodCurrent),
                        colonyStats.GetValue(GameParameterType.MoodDelta)),
                    ColonyParameterResponse.Area(
                        (int)colonyStats.GetValue(GameParameterType.ModulesUsed),
                        (int)colonyStats.GetValue(GameParameterType.ModulesTotal)));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.State;
            var currentWeek = (int)colonyStats.GetValue(GameParameterType.TurnsCurrent);

            additionalPatameters.AddRange(
                    ColonyParameterResponse.Station("Рассвет-342", 1),
                    ColonyParameterResponse.CurrentWeek(currentWeek));

            var population = colonyStats.GetPopulation();
            if (population > 0)
            {
                additionalPatameters.AddRange(
                    ColonyParameterResponse.Attractiveness(colonyStats.GetAttractiveness()),
                    ColonyParameterResponse.Population(population),
                    ColonyParameterResponse.CodeOfLaws(GetCodeOfLaws(colonyStats)));
            }

            return additionalPatameters;
        }

        private static CodeOfLaws GetCodeOfLaws(ColonyState colonyStats)
        {
            var humanism = colonyStats.GetValue(GameParameterType.ReformsSocialGuaranteesLevel) -
                colonyStats.GetValue(GameParameterType.ReformsTaxLevel);
            return humanism switch
            {
                > 1 => CodeOfLaws.Humanist,
                < -1 => CodeOfLaws.Capitalist,
                _ => CodeOfLaws.Centrist
            };
        }
    }
}
