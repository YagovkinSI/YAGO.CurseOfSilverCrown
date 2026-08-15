using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Turns.Queries.GetMyTurn
{
    public class GetTurnQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMyTurnQuery, GetMyTurnResult>
    {
        public async Task<GetMyTurnResult> Handle(GetMyTurnQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                throw new YagoNotValidException("Пользователь не имеет колонии.");

            var nextTurnStartAtUtc = colony.State.TurnReserve.GetNextTurnStartAtUtc(DateTime.UtcNow);

            return new GetMyTurnResult(nextTurnStartAtUtc);
        }
    }

    public record GetMyTurnQuery(long UserId) : IRequest<GetMyTurnResult>;
    public record GetMyTurnResult(DateTime NextTurnStartAtUtc);
}
