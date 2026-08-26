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
                    colony.State.Resources.ActionPoints.Value,
                    colony.State.Resources.ActionPoints.MaxValue,
                    colony.State.Resources.ActionPoints.GetDeltaPerTurn(colony.State)),
                ColonyParameterResponse.Finance(
                    colony.State.Resources.Solars.Value,
                    colony.GetSolarDelta()),
                ColonyParameterResponse.Other());

            if (colony.State.GetPopulation() > 0)
            {
                mainPatameters.AddRange(
                    ColonyParameterResponse.Gdp(colonyState.GetGdp()),
                    ColonyParameterResponse.Trust(
                        colony.State.Resources.Mood.Value,
                        colony.State.GetMoodDelta()),
                    ColonyParameterResponse.Area(
                        colony.State.Slots[Domain.Colonies.Slots.ColonySlotType.Modules].GetUsed(colony.State),
                        colony.State.Slots[Domain.Colonies.Slots.ColonySlotType.Modules].GetTotal(colony.State)));
            }

            return mainPatameters;
        }

        private static List<ColonyParameterResponse> AddAdditionalParameters(Colony colony)
        {
            var additionalPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.State;
            var currentWeek = colony.State.Resources.TurnNumber.Value;

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
            var humanism = colony.State.Reforms[ColonyReformType.SocialGuaranteesLevel].Value -
                colony.State.Reforms[ColonyReformType.TaxLevel].Value;
            return humanism switch
            {
                > 1 => CodeOfLaws.Humanist,
                < -1 => CodeOfLaws.Capitalist,
                _ => CodeOfLaws.Centrist
            };
        }
    }
}
