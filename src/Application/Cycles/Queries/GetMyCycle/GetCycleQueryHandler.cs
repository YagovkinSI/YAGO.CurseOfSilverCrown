using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Application.Cycles.Queries.GetMyCycle
{
    public class GetCycleQueryHandler(
        IColonyRepository colonyRepository,
        ICycleRepository cycleRepository)
        : IRequestHandler<GetMyCycleQuery, GetMyCycleResult>
    {
        public async Task<GetMyCycleResult> Handle(GetMyCycleQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetMyCycleResult(Cycle: null, ColonyEpisodes: []);

            var cycle = await cycleRepository.FindLastColonyCycle(colony.Id, cancellationToken);

            var colonyEpisodes = cycle == null ? [] : GameEventsDataset
                .GetAll()
                .Where(x => cycle.GameEventsIds.Contains(x.Id))
                .Select(x => new ColonyEpisode(x.Episode, colony.Stats))
                .ToList();

            return new GetMyCycleResult(cycle, colonyEpisodes);
        }
    }

    public record GetMyCycleQuery(long UserId) : IRequest<GetMyCycleResult>;
    public record GetMyCycleResult(Cycle? Cycle, IReadOnlyList<ColonyEpisode> ColonyEpisodes);
}
