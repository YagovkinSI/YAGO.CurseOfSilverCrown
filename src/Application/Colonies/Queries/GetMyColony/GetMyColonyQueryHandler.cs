using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyQuests;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Entities.Quests;

namespace YAGO.World.Application.Colonies.Queries.GetMyColony
{
    public class GetMyColonyQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMyColonyQuery, GetMyColonyResult>
    {
        public async Task<GetMyColonyResult> Handle(GetMyColonyQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);

            var colonyQuests = colony == null ? [] : GameEventsDataset
                .GetAll()
                .Where(x => colony.QuestIds.Contains(x.Id))
                .Select(x => new ColonyQuest(colony.Stats, x))
                .ToList();

            return new GetMyColonyResult(colony, colonyQuests);
        }
    }

    public record GetMyColonyQuery(long UserId) : IRequest<GetMyColonyResult>;
    public record GetMyColonyResult(Colony? Colony, IReadOnlyList<ColonyQuest> ColonyQuests);
}
