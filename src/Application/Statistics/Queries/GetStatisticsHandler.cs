using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameParameters;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetStatisticsHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(GetStatisticsQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            GameParameterComposition? result = null;
            switch (command.Type)
            {
                case StatisticType.SolarsDelta:
                    result = GameParameterHelper.GetBudgetComposition(colony);
                    break;
            }

            return result == null
                ? throw new YagoException($"Не удалось получить данные по типу: {command.Type}.")
                : new GetStatisticsResult(result);
        }
    }

    public record GetStatisticsQuery(long UserId, StatisticType Type) : IRequest<GetStatisticsResult>;
    public record GetStatisticsResult(GameParameterComposition Composition);

    public enum StatisticType
    {
        SolarsDelta
    }
}
