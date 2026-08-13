using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Turns;

namespace YAGO.World.Application.Turns.Queries.GetMyTurn
{
    public class GetTurnQueryHandler(
        IColonyRepository colonyRepository,
        ITurnRepository turnRepository)
        : IRequestHandler<GetMyTurnQuery, GetMyTurnResult>
    {
        public async Task<GetMyTurnResult> Handle(GetMyTurnQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetMyTurnResult(Turn: null);

            var turn = await turnRepository.FindLastColonyTurn(colony.Id, cancellationToken);

            return new GetMyTurnResult(turn);
        }
    }

    public record GetMyTurnQuery(long UserId) : IRequest<GetMyTurnResult>;
    public record GetMyTurnResult(Turn? Turn);
}
