using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.Commands.IssueDecree
{
    public class IssueDecreeCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<IssueDecreeCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(IssueDecreeCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var decreeDataset = DecreeDataset.Get().ToList();
            var decree = decreeDataset.Find(x => x.Id == command.DecreeId)
                ?? throw new YagoNotFoundException(nameof(Decree), command.DecreeId.ToString());

            var colonyStats = colony.Stats;
            colonyStats.IssueDecree(decree);
            await colonyRepository.Update(colony, cancellationToken);

            return new HandlerResultEmpty();
        }
    }

    public record IssueDecreeCommand(long UserId, long DecreeId) : IRequest<HandlerResultEmpty>;
}
