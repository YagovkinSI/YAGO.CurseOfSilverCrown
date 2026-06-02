using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Cycles;

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
                return new GetMyCycleResult(Cycle: null);

            var cycle = await cycleRepository.FindLastColonyCycle(colony.Id, cancellationToken);

            return new GetMyCycleResult(cycle);
        }
    }

    public record GetMyCycleQuery(long UserId) : IRequest<GetMyCycleResult>;
    public record GetMyCycleResult(Cycle? Cycle);
}
