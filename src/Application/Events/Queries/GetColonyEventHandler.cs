using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Events.Queries
{
    public class GetColonyEventHandler(
        IColonyRepository colonyRepository,
        IColonyEventRepository colonyEventRepository,
        IGameEventRepository gameEventRepository)
        : IRequestHandler<GetColonyEventQuery, GetGetColonyEventResult>
    {
        public async Task<GetGetColonyEventResult> Handle(GetColonyEventQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var colonyEvent = await colonyEventRepository.Find(command.ColonyEventId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyEvent), command.ColonyEventId.ToString());
            if (colonyEvent.ColonyId != colony.Id)
                throw new YagoNotVerifyOwnershipException(nameof(ColonyEvent), command.ColonyEventId.ToString());

            var gameEvent = await gameEventRepository.Get(colonyEvent.EventCode, cancellationToken);
            var aggregate = new ColonyEventPrivateDto(colonyEvent, gameEvent, colony.State);
            return new GetGetColonyEventResult(aggregate);
        }
    }

    public record GetColonyEventQuery(long UserId, long ColonyEventId) : IRequest<GetGetColonyEventResult>;
    public record GetGetColonyEventResult(ColonyEventPrivateDto? ColonyEvent);
}
