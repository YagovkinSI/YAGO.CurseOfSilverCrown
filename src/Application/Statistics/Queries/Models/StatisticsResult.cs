using System.Collections.Generic;

namespace YAGO.World.Application.Statistics.Queries.Models
{
    public record StatisticsDto(
        StatisticCode Code,
        string Title,
        IReadOnlyList<StatFieldDto> Fields);

    public record StatFieldDto(
        StatisticCategory Category,
        string Label,
        string Value,
        ParameterStatus Status,
        IReadOnlyList<string> Description,
        StatisticCode? ChildrenCode);

    public record StatisticsResult(StatisticsDto? Statistics);
}
