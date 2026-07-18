namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public record RequirementParametersResponse(
        string Type,
        string[] StatMenus,
        int Weight,
        string Name,
        string Value,
        string? Url = null)
    {
        public string Status { get; set; } = ParameterStatusConstants.Neutral;

        public static ColonyParameterResponse ActionPoints_Resourses(double threshold, bool isTopThreshold)
        {
            return new(
                ColonyParameterNames.ActionPoints_Resourses,
                StatMenus: [],
                Weight: 0,
                "Очки действий (будут потрачены):",
                threshold.ToString(),
                Url: null);
        }

        public static ColonyParameterResponse FinanceReserves(double threshold, bool isTopThreshold)
        {
            return new(
                ColonyParameterNames.Economic_Reserves,
                StatMenus: [],
                Weight: 21,
                "Солары (будут потрачены):",
                threshold.ToString(),
                Url: null);
        }

        public static ColonyParameterResponse FinanceTrend(double threshold, bool isTopThreshold)
        {
            return new(
                ColonyParameterNames.Economic_Budget_Balance,
                StatMenus: [],
                Weight: 29,
                "Соларов за ход:",
                $"{(isTopThreshold ? "не более" : "не менее")} {threshold}",
                Url: null);
        }

        public static ColonyParameterResponse TrustResourse(double threshold, bool isTopThreshold)
        {
            return new(
                ColonyParameterNames.Mood_Total,
                StatMenus: [],
                Weight: 31,
                "Доверие:",
                $"{(isTopThreshold ? "не более" : "не менее")} {threshold}",
                Url: null);
        }

        public static ColonyParameterResponse AreaOccupied(double threshold, bool isTopThreshold)
        {
            return new(
                ColonyParameterNames.Area_Total,
                StatMenus: [],
                Weight: 41,
                "Занято зон:",
                $"{(isTopThreshold ? "не более" : "не менее")} {threshold}",
                Url: null);
        }
    }
}
