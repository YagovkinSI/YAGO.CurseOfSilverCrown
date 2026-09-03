using System.Collections.Generic;
using YAGO.World.Host.Controllers.Common;

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
        DisplayInfoResponse? Info,
        string? ChildrenCode);

    public static class ParameterStatusConstants
    {
        public const string Critical = "Critical";
        public const string Bad = "Bad";
        public const string Neutral = "Neutral";
        public const string Good = "Good";
        public const string Excellent = "Excellent";
    }

    public static class StatisticCodeConstants
    {
        public const string Main = "Main";
        public const string MainMore = "MainMore";
        public const string Solars = "Solars";
        public const string SolarDelta = "SolarDelta";
        public const string PublicDebt = "PublicDebt";
        public const string AdministrationSalary = "AdministrationSalary";
    }

    public static class StatisticCategoryConstants
    {
        public const string Info = "Info";
        public const string ActionPoints = "ActionPoints";
        public const string Solars = "Solars";
        public const string SolarDelta = "SolarDelta";
        public const string Modules = "Modules";
        public const string Mood = "Mood";
        public const string Reforms = "Reforms";
        public const string Population = "Population";
        public const string PrivateCapital = "PrivateCapital";
    }
}
