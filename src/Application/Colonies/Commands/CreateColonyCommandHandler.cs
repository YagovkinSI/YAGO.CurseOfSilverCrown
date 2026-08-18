using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Events;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Colonies.Commands
{
    public class CreateColonyCommandHandler(
        IColonyRepository colonyRepository,
        IGameEventRepository gameEventRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<CreateColonyCommand, CreateColonyResult>
    {
        public async Task<CreateColonyResult> Handle(CreateColonyCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony != null)
                throw new YagoException("Пользователь уже имеет колонию.");

            colony = Colony.CreateNew(command.UserId);
            var firstColonyEvent = ColonyEvent.CreateNew(
                colonyId: default,
                GameEventConstants.StartColonyEvent,
                turnNumber: 1);

            await SaveChanges(colony, firstColonyEvent, cancellationToken);

            var gameEvent = await gameEventRepository.Get(GameEventConstants.StartColonyEvent, cancellationToken);
            var eventDto = new ColonyEventSummaryDto(firstColonyEvent, gameEvent);
            var colonyPrivate = new ColonyPrivateDto(colony, [eventDto]);
            return new CreateColonyResult(colonyPrivate);
        }

        private async Task SaveChanges(Colony colony, ColonyEvent firstColonyEvent, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                var colonyId = await unitOfWorkRepository.Add(colony, cancellationToken);
                colony.SetId(colonyId);

                firstColonyEvent.SetColonyId(colonyId);
                var colonyEvetnId = await unitOfWorkRepository.Add(firstColonyEvent, cancellationToken);
                firstColonyEvent.SetId(colonyEvetnId);

                await unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }

    public record CreateColonyCommand(long UserId) : IRequest<CreateColonyResult>;
    public record CreateColonyResult(ColonyPrivateDto? ColonyPrivate);
}
