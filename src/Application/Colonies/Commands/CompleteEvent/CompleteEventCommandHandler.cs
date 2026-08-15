using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;
using static YAGO.World.Application.Colonies.Commands.CompleteEvent.CompleteEventCommandHandler;

namespace YAGO.World.Application.Colonies.Commands.CompleteEvent
{
    public class CompleteEventCommandHandler(
        IColonyRepository colonyRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<CompleteEventCommand, CompleteEventResult>
    {
        public async Task<CompleteEventResult> Handle(CompleteEventCommand command, CancellationToken cancellationToken)
        {
            if (command.DilemmaResolving.Contains('#'))
                throw new YagoException("Команда содержит недопустимый символ.");
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            if (!colony.Events.Select(x => x.EventId).Contains(command.EventId))
                throw new YagoException("Не найдено событие для завершения.");

            var gameEvent = GameEventsDataset.Get(command.EventId);
            var eventResult = gameEvent.Results.FirstOrDefault(x => x.Key == command.DilemmaResolving).Value
                ?? gameEvent.Results.FirstOrDefault(x => x.Key == "#end").Value
                ?? EventResult.CreateNew();
            eventResult.SetMainParametersBefore(colony);
            SetChangeList(colony, gameEvent, command.DilemmaResolving);
            eventResult.SetMainParametersAfter(colony);

            colony.RemoveEvent(gameEvent.Id);

            await SaveChanges(colony, cancellationToken);

            return new CompleteEventResult(eventResult.Show ? eventResult : null);
        }

        private static void SetChangeList(
            Colony colony,
            GameEvent gameEvent,
            string dilemmaResolving)
        {
            var changeList = gameEvent.ChangeList;
            if (gameEvent.Id == nameof(ColonyNameEvent))
            {
                if (!string.IsNullOrEmpty(dilemmaResolving))
                    colony.SetName(dilemmaResolving);
            }
            else if (changeList.ContainsKey(dilemmaResolving))
            {
                var change = gameEvent.ChangeList[dilemmaResolving];
                var isAvailable = change.Requirements.All(x => x.Check(colony.State));
                if (!isAvailable)
                    throw new YagoException("Не выполнены условия.", 400);
                colony.SetChanges(change);
            }

            if (changeList.ContainsKey("#end"))
                colony.SetChanges(changeList["#end"]);
        }

        private async Task SaveChanges(
            Colony colony, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
                await unitOfWorkRepository.Update(colony, cancellationToken);
                await unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public record CompleteEventCommand(long UserId, string EventId, string DilemmaResolving) : IRequest<CompleteEventResult>;
        public record CompleteEventResult(EventResult? EventResult);
    }
}
