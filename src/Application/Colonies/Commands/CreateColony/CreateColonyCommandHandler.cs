using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Colonies.Commands.CreateColony
{
    public class CreateColonyCommandHandler(
        IColonyRepository colonyRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<CreateColonyCommand, CreateColonyResult>
    {
        public async Task<CreateColonyResult> Handle(CreateColonyCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            colony ??= await CreateColony(command.UserId, cancellationToken);

            var list = new List<ColonyEventDto>(colony.Events.Count);
            foreach (var colonyEvent in colony.Events)
            {
                var gameEvent = GameEventsDataset.Get(colonyEvent.EventId);
                var aggregate = new ColonyEventDto(colonyEvent, gameEvent, colony.State);
                list.Add(aggregate);
            }

            return new CreateColonyResult(colony, list);
        }

        private async Task<Colony> CreateColony(long userId, CancellationToken cancellationToken)
        {
            var colony = Colony.CreateNew(userId);
            try
            {
                await unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
                _ = await unitOfWorkRepository.Add(colony, cancellationToken);
                await unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            return colony;
        }
    }

    public record CreateColonyCommand(long UserId) : IRequest<CreateColonyResult>;
    public record CreateColonyResult(Colony? Colony, IReadOnlyList<ColonyEventDto> ColonyEvents);
}
