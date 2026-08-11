using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Application.Colonies.Queries.GetMyColony
{
    public class GetMyColonyQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMyColonyQuery, GetMyColonyResult>
    {
        public async Task<GetMyColonyResult> Handle(GetMyColonyQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetMyColonyResult(Colony: null, ColonyEvents: []);

            var list = new List<ColonyEventDto>(colony.Events.Count);
            foreach (var colonyEvent in colony.Events)
            {
                var gameEvent = GameEventsDataset.Get(colonyEvent.EventId);
                var aggregate = new ColonyEventDto(colonyEvent, gameEvent, colony.State);
                list.Add(aggregate);
            }

            return new GetMyColonyResult(colony, list);
        }
    }

    public record GetMyColonyQuery(long UserId) : IRequest<GetMyColonyResult>;
    public record GetMyColonyResult(Colony? Colony, IReadOnlyList<ColonyEventDto> ColonyEvents);
}
