using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.Commands.DeactivateColony
{
    public class DeactivateColonyCommandHandler(
        IColonyRepository colonyRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<DeactivateColonyCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(DeactivateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony == null)
                return new HandlerResultEmpty();

            userColony.Deactivate();
            var newColony = Colony.CreateNew(command.UserId);

            await unitOfWorkRepository.SaveInTransactionAsync([userColony, newColony], cancellationToken);

            return new HandlerResultEmpty();
        }
    }
    public record DeactivateColonyCommand(long UserId) : IRequest<HandlerResultEmpty>;
}
