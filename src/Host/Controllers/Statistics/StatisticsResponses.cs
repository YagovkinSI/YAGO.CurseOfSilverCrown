using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Statistics
{
    public record StatisticsResponse(
        string Code,
        string Title,
        IReadOnlyList<StatisticFieldResponse> Fields);

    public record StatisticFieldResponse(
        string Category,
        string Label,
        string Value,
        string Status,
        IReadOnlyList<string> Description,
        string? ChildrenCode);

    public static class ParameterStatusConstants
    {
        public const string Critical = "critical";
        public const string Bad = "bad";
        public const string Neutral = "neutral";
        public const string Good = "good";
        public const string Excellent = "excellent";
    }

    public static class StatisticCodeConstants
    {
        public const string Main = "main";
        public const string SolarDelta = "solarDelta";
    }

    public static class StatisticCategoryConstants
    {
        public const string Info = "info";
        public const string Solars = "solars";
    }
}
