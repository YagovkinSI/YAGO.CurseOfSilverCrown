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
        IColonyRepository colonyRepository,
        IColonyEventRepository colonyEventRepository)
        : IRequestHandler<GetColonyPrivateQuery, GetColonyPrivateResult>
    {
        public async Task<GetColonyPrivateResult> Handle(GetColonyPrivateQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetColonyPrivateResult(Colony: null, ColonyEvents: []);
            var colonyEvents = await colonyEventRepository.FindByColonyId(colony.Id, onlyNotComplited: true, cancellationToken);

            var list = new List<ColonyEventDto>(colonyEvents.Count);
            foreach (var colonyEvent in colonyEvents)
            {
                var gameEvent = GameEventsDataset.Get(colonyEvent.EventCode);
                var aggregate = new ColonyEventDto(colonyEvent, gameEvent, colony.State);
                list.Add(aggregate);
            }

            return new GetColonyPrivateResult(colony, list);
        }
    }

    public record GetColonyPrivateQuery(long UserId) : IRequest<GetColonyPrivateResult>;
    public record GetColonyPrivateResult(Colony? Colony, IReadOnlyList<ColonyEventDto> ColonyEvents);
}
