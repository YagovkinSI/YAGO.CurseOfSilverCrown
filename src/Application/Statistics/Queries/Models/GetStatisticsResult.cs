using System.Collections.Generic;

namespace YAGO.World.Application.Statistics.Queries.Models
{
    public record StatisticsResult(
        StatisticCode Code,
        string Title,
        IReadOnlyList<StatisticFieldDto> Fields);

    public record StatisticFieldDto(
        ParameterCategory Category,
        string Label,
        string Value,
        ParameterStatus Status,
        IReadOnlyList<string> Description,
        StatisticCode? ChildrenCode);

    public record GetStatisticsResult(StatisticsResult Statistics);
}
