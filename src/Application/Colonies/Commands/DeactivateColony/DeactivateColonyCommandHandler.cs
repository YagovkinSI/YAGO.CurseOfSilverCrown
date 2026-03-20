using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Repository;

namespace YAGO.World.Application.Colonies.Commands.DeactivateColony
{
    public class DeactivateColonyCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<DeactivateColonyCommand, ProcessorResultEmpty>
    {
        public async Task<ProcessorResultEmpty> Handle(DeactivateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony == null)
                return new ProcessorResultEmpty();

            userColony.Deactivate();

            await colonyRepository.Update(userColony, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }
    public record DeactivateColonyCommand(long UserId) : IRequest<ProcessorResultEmpty>;
}
