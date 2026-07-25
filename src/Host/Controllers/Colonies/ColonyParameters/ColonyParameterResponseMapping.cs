using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.ValueTypes.States;

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
                    (int)colonyStats.GetGameParameter(StateKey.ReformPointsCurrent),
                    (int)colonyStats.States[StateKey.ReformPointsCurrent].MaxValue,
                    (int)colonyStats.GetGameParameter(StateKey.ReformPointsDelta)),
                ColonyParameterResponse.Finance((colonyStats.States[StateKey.SolarsCurrent] as MutableState)!.Value, colonyStats.GetSolarsIncome()),
                ColonyParameterResponse.Other());

            if (colony.State.GetPopulation() > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyStats.GdpCalc(), colonyStats.GdpTrendCalc()),
                    ColonyParameterResponse.Trust(
                        colonyStats.GetGameParameter(StateKey.MoodReserve),
                        colonyStats.MoodTotalBalanceCacl()),
                    ColonyParameterResponse.Area(
                        colonyStats.GetZonesOccupied(),
                        (int)colonyStats.GetGameParameter(StateKey.ModulesTotal)));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.State;
            var currentWeek = (int)colonyStats.GetGameParameter(StateKey.TurnsCurrent);

            additionalPatameters.AddRange(
                    ColonyParameterResponse.Station("Рассвет-342", 1),
                    ColonyParameterResponse.CurrentWeek(currentWeek));

            var population = colonyStats.GetPopulation();
            if (population > 0)
            {
                additionalPatameters.AddRange(
                    ColonyParameterResponse.Attractiveness(colonyStats.AttractivenessTotalCalc()),
                    ColonyParameterResponse.Population(population),
                    ColonyParameterResponse.CodeOfLaws(GetCodeOfLaws(colonyStats)));
            }

            return additionalPatameters;
        }

        private static CodeOfLaws GetCodeOfLaws(ColonyState colonyStats)
        {
            var humanism = colonyStats.GetGameParameter(StateKey.ReformsSocialGuaranteesLevel) -
                colonyStats.GetGameParameter(StateKey.ReformsTaxLevel);
            return humanism switch
            {
                > 1 => CodeOfLaws.Humanist,
                < -1 => CodeOfLaws.Capitalist,
                _ => CodeOfLaws.Centrist
            };
        }
    }
}
