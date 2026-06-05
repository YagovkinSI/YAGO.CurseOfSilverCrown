using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyQuests;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Application.Colonies.Queries.GetColonyQuest
{
    public class GetColonyEventHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetColonyEventQuery, GetGetColonyEventResult>
    {
        public async Task<GetGetColonyEventResult> Handle(GetColonyEventQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetGetColonyEventResult(ColonyEvent: null);

            var gameEvent = GameEventsDataset.Get(command.EventId);
            var colonyEvent = new ColonyEvent(colony.Stats, gameEvent);
            return new GetGetColonyEventResult(colonyEvent);
        }
    }

    public record GetColonyEventQuery(long UserId, string EventId) : IRequest<GetGetColonyEventResult>;
    public record GetGetColonyEventResult(ColonyEvent? ColonyEvent);
}
