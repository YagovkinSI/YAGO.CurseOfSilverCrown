using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Cycles.Queries.GetMyCycle
{
    public class GetCycleQueryHandler(
        IColonyRepository colonyRepository,
        ICycleRepository cycleRepository)
        : IRequestHandler<GetCycleQuery, GetCycleResult>
    {
        public async Task<GetCycleResult> Handle(GetCycleQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var cycle = await cycleRepository.GetLast(colony.Id, cancellationToken);
            if (cycle == null || cycle.State == CycleState.Completed)
                cycle = Cycle.CreateNew(colony.Id, cycle);

            return new GetCycleResult(cycle);
        }
    }

    public record GetCycleQuery(long UserId) : IRequest<GetCycleResult>;
    public record GetCycleResult(Cycle Cycle);
}
