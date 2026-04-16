using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Services;
using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Application.Cycles.Commands.GetMyCycle
{
    public class GetCycleQueryHandler(
        ICurrentColonyProvider currentColonyProvider,
        ICurrentCycleProvider currentCycleProvider)
        : IRequestHandler<GetMyCycleCommand, GetMyCycleResult>
    {
        public async Task<GetMyCycleResult> Handle(GetMyCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await currentColonyProvider.Get(command.UserId, cancellationToken);
            var cycle = await currentCycleProvider.Get(colony.Id, cancellationToken);
            return new GetMyCycleResult(cycle);
        }
    }

    public record GetMyCycleCommand(long UserId) : IRequest<GetMyCycleResult>;
    public record GetMyCycleResult(Cycle Cycle);
}
