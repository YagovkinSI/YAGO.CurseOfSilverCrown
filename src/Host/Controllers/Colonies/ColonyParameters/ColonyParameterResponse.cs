using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents.Dataset;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public record ColonyParameterResponse(
        string Type,
        string? ParrentType,
        int Weight,
        string Name,
        string Value,
        string? Url = null)
    {
        public static ColonyParameterResponse ActionPoints(int resources, int trend) =>
            new(ColonyParameterNames.ActionPoints, ParrentType: null, Weight: 0, "Очки действий",
                $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)}/н)",
                Url: null);

        public static ColonyParameterResponse ActionPoints_Resourses(int resources, bool isChange) =>
            new(ColonyParameterNames.ActionPoints_Resourses, ParrentType: null, Weight: 0, "Очки действий",
                resources.ToBeautifulString(isChange),
                Url: null);

        public static ColonyParameterResponse ActionPoints_Trend(int trend, bool isChange) =>
            new(ColonyParameterNames.ActionPoints_Trend, ParrentType: null, Weight: 0, "Прирост ОД",
                trend.ToBeautifulString(isChange),
                Url: null);

        public static ColonyParameterResponse ColonyName(string colonyName) =>
            new(ColonyParameterNames.Colony_Name, ParrentType: null, Weight: 0, "Колония", colonyName, Url: null);

        public static ColonyParameterResponse Gdp(double resources, double trend) =>
            new(ColonyParameterNames.Gdp, ParrentType: null, Weight: 1, "ВВП",
                  $"{resources.ToBeautifulString()} (~{trend.ToBeautifulString(setPlus: true)}%)",
                  Url: null);

        public static ColonyParameterResponse Attractiveness(double value) =>
            new(ColonyParameterNames.Attractiveness_Total, ParrentType: ColonyParameterNames.Gdp, Weight: 14, "Привлекательность",
                  value.ToBeautifulString(),
                  Url: null);

        public static ColonyParameterResponse Finance(double resources, double trend) =>
            new(ColonyParameterNames.Economic, ParrentType: null, Weight: 2, "Финансы",
                  $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)}/н)",
                  Url: null);

        public static ColonyParameterResponse FinanceReserves(double resources, bool isChange) =>
            new(ColonyParameterNames.Economic_Reserves, ParrentType: ColonyParameterNames.Economic, Weight: 21, "Резервы",
                  resources.ToBeautifulString(isChange),
                  Url: null);

        public static ColonyParameterResponse FinanceTrend(double trend, bool isChange) =>
            new(ColonyParameterNames.Economic_Budget_Balance, ParrentType: ColonyParameterNames.Economic, Weight: 29,
                  isChange ? "Доход" : "Итого",
                  $"{trend.ToBeautifulString(isChange)}/н",
                  Url: null);

        public static ColonyParameterResponse Trust(double resources, double trend) =>
            new(ColonyParameterNames.Mood_Total, ParrentType: null, Weight: 3, "Доверие",
                  $"{(resources < GameEventsConstants.TrustWithRevolt ? "🔥 " : "")}" +
                  $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)}/н)",
                  Url: null);

        public static ColonyParameterResponse TrustResourse(double value, bool isChange) =>
            new(ColonyParameterNames.Mood_Total, ParrentType: ColonyParameterNames.Mood_Total, Weight: 31, "Доверие",
                  $"{value.ToBeautifulString(isChange)}",
                  Url: null);

        public static ColonyParameterResponse Area(int occupied, int total) =>
            new(ColonyParameterNames.Area, ParrentType: null, Weight: 4, "Пространство",
                $"{occupied.ToBeautifulString()}/{total.ToBeautifulString()}");

        public static ColonyParameterResponse AreaResourse(int total, bool isChange) =>
            new(ColonyParameterNames.Area_Total, ParrentType: ColonyParameterNames.Area, Weight: 41, "Пространство",
                total.ToBeautifulString(isChange));

        public static ColonyParameterResponse Other() =>
            new(ColonyParameterNames.Other, ParrentType: null, Weight: 9, "Дополнительно", "...");

        public static ColonyParameterResponse Station(string shipName, long shipId) =>
            new(ColonyParameterNames.Ship_Id, ParrentType: ColonyParameterNames.Other, Weight: 91, "Станция",
                shipName,
                Url: shipId.ToString());

        public static ColonyParameterResponse Population(int value, bool isChange = false) =>
            new(ColonyParameterNames.Population_Total, ParrentType: ColonyParameterNames.Other, Weight: 92, "Население",
                value.ToBeautifulString(isChange));

        public static ColonyParameterResponse CodeOfLaws(CodeOfLaws codeOfLaws)
        {
            var value = codeOfLaws switch
            {
                Domain.Entities.Colonies.CodeOfLaws.Capitalist => "Корпоративные",
                Domain.Entities.Colonies.CodeOfLaws.Centrist => "Стандартные",
                Domain.Entities.Colonies.CodeOfLaws.Humanist => "Гуманные",
                _ => "Не определены",
            };
            return new(ColonyParameterNames.Laws_CodeOfLaws, ParrentType: ColonyParameterNames.Other, Weight: 93, "Законы",
                value);
        }

        public static ColonyParameterResponse EpisodeCount(int episodeCount) =>
            new(ColonyParameterNames.EpisodeCount, ParrentType: ColonyParameterNames.Other, Weight: 99, "Ход",
                episodeCount.ToString());
    }
}
