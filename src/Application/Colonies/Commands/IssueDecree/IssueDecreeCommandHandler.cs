using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;

namespace YAGO.World.Application.Colonies.Commands.IssueDecree
{
    public class IssueDecreeCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<IssueDecreeCommand, CompleteEventResult>
    {
        public async Task<CompleteEventResult> Handle(IssueDecreeCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var decreeDataset = DecreeDataset.Get().ToList();
            var decree = decreeDataset.Find(x => x.Id == command.DecreeId)
                ?? throw new YagoNotFoundException(nameof(Decree), command.DecreeId.ToString());

            var colonyStats = colony.State;

            var eventResult =  new EventResult(
                decree.Name,
                decree.Image,
                text: [],
                [], [], [], showForce: true);
            eventResult.SetMainParametersBefore(colony);

            colonyStats.IssueDecree(decree);

            eventResult.SetMainParametersAfter(colony);

            await colonyRepository.Update(colony, cancellationToken);

            return new CompleteEventResult(eventResult);
        }
    }

    public record IssueDecreeCommand(long UserId, long DecreeId) : IRequest<CompleteEventResult>;
    public record CompleteEventResult(EventResult EventResult);
}
