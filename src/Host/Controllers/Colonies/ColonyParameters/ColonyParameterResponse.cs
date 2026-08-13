using YAGO.World.Domain.GameEvents.Dataset;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public record ColonyParameterResponse(
        string Type,
        string[] StatMenus,
        int Weight,
        string Name,
        string Value,
        string? Url = null)
    {
        public string Status { get; set; } = ParameterStatusConstants.Neutral;

        public static ColonyParameterResponse ActionPoints(int resources, int limit, int trend)
        {
            return new(
                ColonyParameterNames.ActionPoints,
                StatMenus: [StatMenuConstants.Header, StatMenuConstants.Stats],
                Weight: 0,
                "Очки действий",
                $"{resources.ToBeautifulString()}/{limit.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)})",
                Url: null);
        }

        public static ColonyParameterResponse ActionPoints_Resourses(int resources, bool isChange)
        {
            return new(
                ColonyParameterNames.ActionPoints_Resourses,
                StatMenus: [],
                Weight: 0,
                "Очки действий",
                resources.ToBeautifulString(isChange),
                Url: null);
        }

        public static ColonyParameterResponse ActionPoints_Trend(int trend, bool isChange)
        {
            return new(
                ColonyParameterNames.ActionPoints_Trend,
                StatMenus: [],
                Weight: 0,
                "Прирост ОД",
                $"{trend.ToBeautifulString(isChange)}",
                Url: null);
        }

        public static ColonyParameterResponse Gdp(double resources)
        {
            return new(
                ColonyParameterNames.Gdp,
                StatMenus: [StatMenuConstants.Stats],
                Weight: 1,
                "ВВП",
                resources.ToBeautifulString(),
                Url: null);
        }

        public static ColonyParameterResponse Attractiveness(double value)
        {
            return new(
                ColonyParameterNames.Attractiveness_Total,
                StatMenus: [StatMenuConstants.Other],
                Weight: 14,
                "Привлекательность",
                value.ToBeautifulString(),
                Url: null);
        }

        public static ColonyParameterResponse Finance(double resources, double trend)
        {
            return new(
                ColonyParameterNames.Economic,
                StatMenus: [StatMenuConstants.Header, StatMenuConstants.Stats],
                Weight: 2,
                "Финансы",
                $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)})",
                Url: null);
        }

        public static ColonyParameterResponse FinanceReserves(double resources, bool isChange)
        {
            return new(
                ColonyParameterNames.Economic_Reserves,
                StatMenus: [],
                Weight: 21,
                "Солары",
                resources.ToBeautifulString(isChange),
                Url: null);
        }

        public static ColonyParameterResponse FinanceTrend(double trend, bool isChange)
        {
            return new(
                ColonyParameterNames.Economic_Budget_Balance,
                StatMenus: [],
                Weight: 29,
                isChange ? "Солары за ход" : "Солары",
                $"{trend.ToBeautifulString(isChange)}",
                Url: null);
        }

        public static ColonyParameterResponse Trust(double resources, double trend)
        {
            return new(
                ColonyParameterNames.Mood_Total,
                StatMenus: [StatMenuConstants.Header, StatMenuConstants.Stats],
                Weight: 3,
                "Доверие",
                $"{(resources < GameEventsConstants.TrustWithRevolt ? "🔥 " : "")}" +
                $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)})",
                Url: null);
        }

        public static ColonyParameterResponse TrustResourse(double value, bool isChange)
        {
            return new(
                ColonyParameterNames.Mood_Total,
                StatMenus: [],
                Weight: 31,
                "Доверие",
                $"{value.ToBeautifulString(isChange)}",
                Url: null);
        }

        public static ColonyParameterResponse Area(int occupied, int total)
        {
            return new(
                ColonyParameterNames.Area,
                StatMenus: [StatMenuConstants.Header, StatMenuConstants.Stats],
                Weight: 4,
                "Пространство",
                $"{occupied.ToBeautifulString()}/{total.ToBeautifulString()}");
        }

        public static ColonyParameterResponse AreaOccupied(int total)
        {
            return new(
                ColonyParameterNames.Area_Total,
                StatMenus: [],
                Weight: 41,
                "Занято зон",
                total.ToBeautifulString());
        }

        public static ColonyParameterResponse Other()
        {
            return new(
                ColonyParameterNames.Other,
                StatMenus: [StatMenuConstants.Stats],
                Weight: 9,
                "Дополнительно",
                "...");
        }

        public static ColonyParameterResponse Station(string shipName, long shipId)
        {
            return new(
                ColonyParameterNames.Ship_Id,
                StatMenus: [StatMenuConstants.Other],
                Weight: 91,
                "Станция",
                shipName,
                Url: shipId.ToString());
        }

        public static ColonyParameterResponse Population(int value, bool isChange = false)
        {
            return new(
                ColonyParameterNames.Population_Total,
                StatMenus: [StatMenuConstants.Header, StatMenuConstants.Stats],
                Weight: 92,
                "Население",
                value.ToBeautifulString(isChange));
        }

        public static ColonyParameterResponse CodeOfLaws(CodeOfLaws codeOfLaws)
        {
            var value = codeOfLaws switch
            {
                Models.CodeOfLaws.Capitalist => "Корпоративные",
                Models.CodeOfLaws.Centrist => "Стандартные",
                Models.CodeOfLaws.Humanist => "Гуманные",
                _ => "Не определены",
            };
            return new(
                ColonyParameterNames.Laws_CodeOfLaws,
                StatMenus: [StatMenuConstants.Other],
                Weight: 93,
                "Законы",
                value);
        }

        public static ColonyParameterResponse CurrentWeek(int currentWeek)
        {
            return new(
                ColonyParameterNames.CurrentWeek,
                StatMenus: [StatMenuConstants.Other],
                Weight: 99,
                "Ход",
                currentWeek.ToString());
        }
    }
}
