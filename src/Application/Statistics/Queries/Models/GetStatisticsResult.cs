using System.Collections.Generic;
using YAGO.World.Domain.Common;

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
        DisplayInfo? Info = null,
        StatisticCode? ChildrenCode = null);

    public record GetStatisticsResult(StatisticsResult Statistics);
}
