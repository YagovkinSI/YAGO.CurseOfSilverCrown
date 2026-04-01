using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Repository;

namespace YAGO.World.Application.Colonies.Commands.DeactivateColony
{
    public class DeactivateColonyCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<DeactivateColonyCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(DeactivateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony == null)
                return new HandlerResultEmpty();

            userColony.Deactivate();

            await colonyRepository.Update(userColony, cancellationToken);

            return new HandlerResultEmpty();
        }
    }
    public record DeactivateColonyCommand(long UserId) : IRequest<HandlerResultEmpty>;
}
