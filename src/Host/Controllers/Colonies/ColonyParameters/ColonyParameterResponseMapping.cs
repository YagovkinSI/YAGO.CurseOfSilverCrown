using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public static class ColonyParameterResponseMapping
    {
        public static IReadOnlyList<ColonyParameterResponse> ToColonyParameters(Colony colony)
        {
            var colonyPatameters = new List<ColonyParameterResponse>();

            var colonyStats = colony.Stats;
            var colonyResources = colonyStats.Resources;
            var episodeCount = colonyStats.EpisodeCount;
            var colonySettings = colonyStats.Settings;

            colonyPatameters.Add(
                ColonyParameterResponse.ColonyName(colony.HasName ? colony.Name : "-"));
            if (colony.Stats.PopulationTotal > 0)
                colonyPatameters.Add(
                    ColonyParameterResponse.Gdp(colonyStats.GdpCalc(), colonyStats.GdpTrendCalc()));
            colonyPatameters.Add(
                ColonyParameterResponse.Finance(colonyResources.Solars, colonyStats.BudgetBalance));
            if (colony.Stats.PopulationTotal > 0) 
                colonyPatameters.AddRange(
                    ColonyParameterResponse.Trust(colonyStats.MoodTotal.Value, colonyStats.MoodTotalBalanceCacl()));
            if (colony.Stats.PopulationTotal > 0)
                colonyPatameters.Add(
                    ColonyParameterResponse.AreaCapacity(colonyStats.ZonesOccupied, colonyResources.ZonesTotal));
            colonyPatameters.Add(
                ColonyParameterResponse.Other());

            SetByEpisode(colonyPatameters, colonyStats, episodeCount, colonySettings);
            return colonyPatameters
                .OrderBy(x => x.Weight)
                .ToList();

        }

        private static void SetByEpisode(
            List<ColonyParameterResponse> colonyPatameters, 
            ColonyStats colonyStats, 
            int episodeCount, 
            ColonySettings colonySettings)
        {
            if (episodeCount > 0)
            {
                colonyPatameters.Add(GetStation(colonySettings.GetShipName(), colonySettings.ShipId));
                colonyPatameters.Add(GetEpisodeCount(episodeCount));
            }
            if (episodeCount > 1)
            {
                colonyPatameters.Add(AttractivenessTotal(colonyStats));
                colonyPatameters.Add(GetPopulation(colonyStats.PopulationTotal));
                colonyPatameters.Add(GetLaws(colonySettings.GetCodeOfLaws()));
            }
        }

        public static ColonyParameterResponse EconomicReserves(double value, bool isChange = false)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Economic_Reserves,
                ParrentType: ColonyParameterNames.Economic_Reserves,
                Weight: 21,
                "Резервы",
                value.ToBeautifulString(isChange));
        }

        public static ColonyParameterResponse EconomicBudgetBalance(double value, bool isChange = false)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Economic_Budget_Balance,
                ParrentType: null,
                Weight: 22,
                "Доход",
                $"{value.ToBeautifulString(isChange)}/н");
        }        

        public static ColonyParameterResponse AreaCapacityOccupied(int value, bool isChange = false)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.AreaCapacity_Occupied,
                ParrentType: null,
                Weight: 52,
                "Площадь",
                isChange && value > 0 ? $"+{value}" : value.ToString());
        }

        //Attractiveness
        private static ColonyParameterResponse AttractivenessTotal(ColonyStats colonyStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Attractiveness_Total,
                ParrentType: ColonyParameterNames.Gdp,
                Weight: 60,
                "Привлекательность",
                colonyStats.AttractivenessTotalCalc().ToBeautifulString());
        }

        private static ColonyParameterResponse GetStation(string shipName, long shipId)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Ship_Id,
                ParrentType: ColonyParameterNames.Other,
                Weight: 200,
                "Станция",
                shipName,
                Url: shipId.ToString());
        }

        private static ColonyParameterResponse GetEpisodeCount(int episodeCount)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.EpisodeCount,
                ParrentType: ColonyParameterNames.Other,
                Weight: 900,
                "Ход",
                episodeCount.ToString());
        }

        public static ColonyParameterResponse GetPopulation(int value, bool isChange = false)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Population_Total,
                ParrentType: ColonyParameterNames.Other,
                Weight: 150,
                "Население",
                isChange && value > 0 ? $"+{value}" : value.ToString());
        }

        private static ColonyParameterResponse GetLaws(CodeOfLaws codeOfLaws)
        {
            var value = codeOfLaws switch
            {
                CodeOfLaws.Capitalist => "Корпоративные",
                CodeOfLaws.Centrist => "Стандартные",
                CodeOfLaws.Humanist => "Гуманные",
                _ => "Не определены",
            };
            return new ColonyParameterResponse(
                ColonyParameterNames.Laws_CodeOfLaws,
                ParrentType: ColonyParameterNames.Other,
                Weight: 300,
                "Законы",
                value);
        }
    }
}
