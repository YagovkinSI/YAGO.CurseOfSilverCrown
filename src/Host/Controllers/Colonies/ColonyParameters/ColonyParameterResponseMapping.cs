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

            colonyPatameters.AddRange(
                new ColonyParameterNameResponse(colony.HasName ? colony.Name : "-"),
                new ColonyParameterFinanceResponse(colonyResources.Solars, colonyStats.BudgetBalance)
            );

            //Finance
            //Mood
            //Places

            //Population

            //Other

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
                colonyPatameters.Add(GetStation(
                    colonySettings.GetShipName(), colonySettings.ShipId, inOther: episodeCount > 1));
                colonyPatameters.Add(GetEpisodeCount(episodeCount));
            }
            if (episodeCount > 1)
            {
                colonyPatameters.Add(MoodTotal(colonyStats.MoodTotal.Value));
                colonyPatameters.Add(AttractivenessTotal(colonyStats));
                colonyPatameters.Add(AreaCapacity(colonyStats));
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

        //Mood
        public static ColonyParameterResponse MoodTotal(double moodTotal, bool isChange = false)
        {
            var value = moodTotal.ToBeautifulString(isChange);
            if (!isChange && moodTotal < 50)
                value += " (риск бунта)";
            return new ColonyParameterResponse(
                ColonyParameterNames.Mood_Total,
                ParrentType: null,
                Weight: 30,
                "Настроение",
                value);
        }

        //AreaCapacity
        private static ColonyParameterResponse AreaCapacity(ColonyStats sourceStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.AreaCapacity,
                ParrentType: null,
                Weight: 50,
                "Площадь",
                $"{sourceStats.ZonesOccupied}/{sourceStats.Resources.ZonesTotal}");
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
                ParrentType: null,
                Weight: 60,
                "Привлекательность",
                colonyStats.AttractivenessTotalCalc().ToBeautifulString());
        }

        private static ColonyParameterResponse GetStation(string shipName, long shipId, bool inOther)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Ship_Id,
                ParrentType: inOther ? ColonyParameterNames.Other : null,
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
                ParrentType: null,
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
                ParrentType: ColonyParameterNames.Colony_Name,
                Weight: 300,
                "Законы",
                value);
        }
    }
}
