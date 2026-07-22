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

            var colonyEvents = colony == null ? [] : GameEventsDataset
                .Find([.. colony.EventIds])
                .Select(x => new ColonyEvent(colony.State, x))
                .ToList();

            return new GetMyColonyResult(colony, colonyEvents);
        }
    }

    public record GetMyColonyQuery(long UserId) : IRequest<GetMyColonyResult>;
    public record GetMyColonyResult(Colony? Colony, IReadOnlyList<ColonyEvent> ColonyEvents);
}
