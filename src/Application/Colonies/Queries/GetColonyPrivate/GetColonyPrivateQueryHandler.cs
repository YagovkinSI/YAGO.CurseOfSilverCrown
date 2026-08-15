using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Colonies.Queries.GetColonyPrivate
{
    public class GetColonyPrivateQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetColonyPrivateQuery, GetColonyPrivateResult>
    {
        public async Task<GetColonyPrivateResult> Handle(GetColonyPrivateQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetColonyPrivateResult(Colony: null, ColonyEvents: []);

            var list = new List<ColonyEventDto>(colony.Events.Count);
            foreach (var colonyEvent in colony.Events)
            {
                var gameEvent = GameEventsDataset.Get(colonyEvent.Key);
                var aggregate = new ColonyEventDto(colonyEvent.Value, gameEvent, colony.State);
                list.Add(aggregate);
            }

            return new GetColonyPrivateResult(colony, list);
        }
    }

    public record GetColonyPrivateQuery(long UserId) : IRequest<GetColonyPrivateResult>;
    public record GetColonyPrivateResult(Colony? Colony, IReadOnlyList<ColonyEventDto> ColonyEvents);
}
