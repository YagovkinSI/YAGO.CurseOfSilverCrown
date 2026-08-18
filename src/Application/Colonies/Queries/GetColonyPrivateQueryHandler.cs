using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Events;
using YAGO.World.Application.Interfaces.Repository;

namespace YAGO.World.Application.Colonies.Queries
{
    public class GetColonyPrivateQueryHandler(
        IColonyRepository colonyRepository,
        IColonyEventRepository colonyEventRepository,
        IGameEventRepository gameEventRepository)
        : IRequestHandler<GetColonyPrivateQuery, GetColonyPrivateResult>
    {
        public async Task<GetColonyPrivateResult> Handle(GetColonyPrivateQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetColonyPrivateResult(ColonyPrivate: null);
            var colonyEvents = await colonyEventRepository.FindByColonyId(colony.Id, onlyNotComplited: true, cancellationToken);

            var list = new List<ColonyEventSummaryDto>(colonyEvents.Count);
            foreach (var colonyEvent in colonyEvents)
            {
                var gameEvent = await gameEventRepository.Get(colonyEvent.EventCode, cancellationToken);
                var aggregate = new ColonyEventSummaryDto(colonyEvent, gameEvent);
                list.Add(aggregate);
            }

            var colonyPrivate = new ColonyPrivateDto(colony, list);
            return new GetColonyPrivateResult(colonyPrivate);
        }
    }

    public record GetColonyPrivateQuery(long UserId) : IRequest<GetColonyPrivateResult>;
    public record GetColonyPrivateResult(ColonyPrivateDto? ColonyPrivate);
}
