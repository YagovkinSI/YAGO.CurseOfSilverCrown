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
                    (int)colonyStats.GetGameParameter(StateKeys.ReformPoints.Reserve),
                    (int)colonyStats.States[StateKeys.ReformPoints.Reserve].MaxValue,
                    (int)colonyStats.GetGameParameter(StateKeys.ReformPoints.Income)),
                ColonyParameterResponse.Finance((colonyStats.States[StateKeys.Solars.Reserve] as MutableState)!.Value, colonyStats.GetSolarsIncome()),
                ColonyParameterResponse.Other());

            if (colony.State.GetPopulation() > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyStats.GdpCalc(), colonyStats.GdpTrendCalc()),
                    ColonyParameterResponse.Trust(
                        colonyStats.GetGameParameter(StateKeys.Mood.Reserve),
                        colonyStats.MoodTotalBalanceCacl()),
                    ColonyParameterResponse.Area(
                        colonyStats.GetZonesOccupied(),
                        (int)colonyStats.GetGameParameter(StateKeys.Modules.Total)));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.State;
            var currentWeek = (int)colonyStats.GetGameParameter(StateKeys.Counters.Turns);

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
            var humanism = colonyStats.GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel) -
                colonyStats.GetGameParameter(StateKeys.Reforms.TaxLevel);
            return humanism switch
            {
                > 1 => CodeOfLaws.Humanist,
                < -1 => CodeOfLaws.Capitalist,
                _ => CodeOfLaws.Centrist
            };
        }
    }
}
