using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates;
using YAGO.World.Domain.Entities.Colonies;
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

            var list = new List<ColonyEventAggregate>(colony?.Events.Count ?? 0);
            foreach (var colonyEvent in (colony?.Events ?? []))
            {
                var gameEvent = GameEventsDataset.Find(colonyEvent.EventId).Single();
                var aggregate = new ColonyEventAggregate(colonyEvent, gameEvent, colony!.State);
                list.Add(aggregate);
            }

            return new GetMyColonyResult(colony, list);
        }
    }

    public record GetMyColonyQuery(long UserId) : IRequest<GetMyColonyResult>;
    public record GetMyColonyResult(Colony? Colony, IReadOnlyList<ColonyEventAggregate> ColonyEvents);
}
