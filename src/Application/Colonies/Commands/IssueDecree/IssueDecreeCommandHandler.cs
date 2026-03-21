using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.Commands.IssueDecree
{
    public class IssueDecreeCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<IssueDecreeCommand, ProcessorResultEmpty>
    {
        public async Task<ProcessorResultEmpty> Handle(IssueDecreeCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var decreeDataset = DecreeDataset.Get().ToList();
            var decree = decreeDataset.Find(x => x.Id == command.DecreeId)
                ?? throw new YagoNotFoundException(nameof(Decree), command.DecreeId);

            colony.IssueDecree(decree);
            await colonyRepository.Update(colony, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }

    public record IssueDecreeCommand(long UserId, long DecreeId) : IRequest<ProcessorResultEmpty>;
}
