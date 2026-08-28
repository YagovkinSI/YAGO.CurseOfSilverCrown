using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Statistics.Queries.Models;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetSolarDeltaStatisticsQueryHandler
        : IRequestHandler<GetSolarDeltaStatisticsQuery, StatisticsResult>
    {
        public Task<StatisticsResult> Handle(
            GetSolarDeltaStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var statistics = new StatisticsDto(StatisticCode.SolarDelta, "Постройки", []);
            return Task.FromResult(new StatisticsResult(statistics));
        }
    }

    public record GetSolarDeltaStatisticsQuery(long UserId) : IRequest<StatisticsResult>;
}
