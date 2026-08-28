using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetMainStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMainStatisticsQuery, StatisticsResult>
    {
        public async Task<StatisticsResult> Handle(
            GetMainStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var solarDelta = colony.GetSolarDelta();

            var fields = new List<StatFieldDto>
            {
                new(
                    StatisticCategory.Info,
                    "Колония",
                    colony.DisplayInfo.DisplayName,
                    ParameterStatus.Neutral,
                    [],
                    null),
                new(
                    StatisticCategory.Solars,
                    "Солары",
                    $"{colony.State.Resources.Solars.Value.ToBeautifulString()} ({solarDelta.ToBeautifulString()})",
                    solarDelta > 0 ? ParameterStatus.Good : ParameterStatus.Bad,
                    ["Солары - денежная валюта в Поясе."],
                    StatisticCode.SolarDelta)
            };

            var statistics = new StatisticsDto(StatisticCode.Main, "Колония", fields);
            return new StatisticsResult(statistics);
        }
    }

    public record GetMainStatisticsQuery(long UserId) : IRequest<StatisticsResult>;
}
