using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Services;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using static YAGO.World.Application.Cycles.Commands.SetChoice.SetChoiceCommandHandler;

namespace YAGO.World.Application.Cycles.Commands.SetChoice
{
    public class SetChoiceCommandHandler(
        IColonyRepository colonyRepository,
        ICurrentCycleProvider currentCycleProvider,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<SetChoiceCommand, SetChoiceResult>
    {
        public async Task<SetChoiceResult> Handle(SetChoiceCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var cycle = await currentCycleProvider.Get(colony.Id, cancellationToken);
            if (cycle.ActiveEventId == null)
                return new SetChoiceResult();

            var activeEvent = GameEventsDataset.Get(cycle.ActiveEventId);
            var choice = activeEvent.Episode.GetChoice(command.ChoiceId);
            var colonyStats = colony.Stats;
            var (isAvailable, mesasge) = choice.CheckAvailability(colonyStats);
            if (!isAvailable)
                throw new YagoException(mesasge, 400);

            colonyStats.SetEpisodeParameters(choice.Parameters, isCycleOver: false);
            cycle.SetStepNumber(cycle.StepNumber, activeEvent: null, isCycleEnded: false);

            var list = new List<IEntity> { colony, cycle };
            await unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);
            return new SetChoiceResult();
        }

        public record SetChoiceCommand(long UserId, Guid ChoiceId) : IRequest<SetChoiceResult>;
        public record SetChoiceResult();
    }
}
