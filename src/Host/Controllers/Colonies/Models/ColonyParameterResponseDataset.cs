using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.ValueTypes;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public static class ColonyParameterResponseDataset
    {
        //Colony
        public static ColonyParameterResponse GetColonyName(string colonyName)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Colony_Name,
                ParrentType: null,
                Weight: 0,
                "Колония",
                colonyName);
        }

        //Economic
        public static ColonyParameterResponse Economic(ColonyStats colonyStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Economic,
                ParrentType: null,
                Weight: 20,
                "Резервы",
                $"{colonyStats.Resources.Solars.ToBeautifulString()} ({colonyStats.BudgetBalance.ToBeautifulString()}/н)");
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
                isChange && value > 0 ? $"+{value.ToBeautifulString(isChange)}/н" : $"{value.ToBeautifulString(isChange)}/н");
        }

        //Mood
        public static ColonyParameterResponse MoodTotal(LimitedDouble moodTotal, bool isChange = false)
        {
            var value = moodTotal.Value.ToBeautifulString(isChange);
            if (!isChange && moodTotal.Value < 50)
                value += " (риск бунта)";
            return new ColonyParameterResponse(
                ColonyParameterNames.Mood_Total,
                ParrentType: null,
                Weight: 30,
                "Настроение",
                value);
        }

        //AreaCapacity
        public static ColonyParameterResponse AreaCapacity(ColonyStats sourceStats)
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
        public static ColonyParameterResponse AttractivenessTotal(ColonyStats colonyStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Attractiveness_Total,
                ParrentType: null,
                Weight: 60,
                "Привлекательность",
                colonyStats.AttractivenessTotalCalc().ToBeautifulString());
        }

        public static ColonyParameterResponse GetStation(string shipName, long shipId, bool inOther)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Ship_Id,
                ParrentType: inOther ? ColonyParameterNames.Other : null,
                Weight: 200,
                "Станция",
                shipName,
                Url: shipId.ToString());
        }

        public static ColonyParameterResponse GetEpisodeCount(int episodeCount)
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

        public static ColonyParameterResponse GetLaws(CodeOfLaws codeOfLaws)
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
