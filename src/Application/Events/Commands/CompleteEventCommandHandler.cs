using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.GameActions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Events.Commands
{
    public class CompleteEventCommandHandler(
        IColonyRepository colonyRepository,
        IColonyEventRepository colonyEventRepository,
        IGameEventRepository gameEventRepository,
        IUnitOfWorkRepository unitOfWorkRepository,
        IApplyGameActionService applyGameActionService)
        : IRequestHandler<CompleteEventCommand, CompleteEventResult>
    {
        private const string DefaultActionKey = "#default";

        public async Task<CompleteEventResult> Handle(CompleteEventCommand command, CancellationToken cancellationToken)
        {
            if (command.DilemmaResolving.Contains('#'))
                throw new YagoException("Команда содержит недопустимый символ.");
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var colonyEvent = await colonyEventRepository.Find(command.ColonyEventId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyEvent), command.ColonyEventId.ToString());
            if (colonyEvent.IsCompleted)
                throw new YagoException("Событие уже завершено.");

            var gameEvent = await gameEventRepository.Get(colonyEvent.EventCode, cancellationToken);
            var action = gameEvent.Actions
                .SingleOrDefault(x => x.Key == command.DilemmaResolving || x.Key == DefaultActionKey);
            var gameAction = action.Value;

            var eventResultDto = applyGameActionService.Apply(
                gameAction, colony, command.DilemmaResolving);
            colonyEvent.SetComplited();
            SavePlayerChoice(colonyEvent, action.Key);

            await SaveChanges(colony, colonyEvent, eventResultDto.NewColonyEvents ?? [], cancellationToken);

            return new CompleteEventResult(eventResultDto.GameActionResult);
        }

        /// <summary>
        /// Фиксирует любой выбор игрока без явного объявления в квесте.
        /// Действие по умолчанию и незаполненные поля ввода не являются выбором.
        /// </summary>
        private static void SavePlayerChoice(ColonyEvent colonyEvent, string? actionKey)
        {
            if (string.IsNullOrWhiteSpace(actionKey) || actionKey == DefaultActionKey)
                return;
            colonyEvent.SetChoice(actionKey);
        }

        private async Task SaveChanges(
            Colony colony,
            ColonyEvent colonyEvent,
            IEnumerable<ColonyEvent> newColonyEvents,
            CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
                await unitOfWorkRepository.Update(colony, cancellationToken);
                await unitOfWorkRepository.Update(colonyEvent, cancellationToken);
                foreach (var newColonyEvent in newColonyEvents)
                    await unitOfWorkRepository.Add(newColonyEvent, cancellationToken);
                await unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }

    public record CompleteEventCommand(long UserId, long ColonyEventId, string DilemmaResolving) : IRequest<CompleteEventResult>;
    public record CompleteEventResult(GameActionResult EventResult);
}
