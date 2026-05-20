using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using static YAGO.World.Application.Cycles.Commands.SetChoice.SetChoiceCommandHandler;

namespace YAGO.World.Application.Cycles.Commands.SetChoice
{
    public class SetChoiceCommandHandler(
        IColonyRepository colonyRepository,
        ICycleRepository cycleRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<SetChoiceCommand, SetChoiceResult>
    {
        public async Task<SetChoiceResult> Handle(SetChoiceCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var cycle = await cycleRepository.FindLastColonyCycle(colony.Id, cancellationToken);
            if (cycle?.ActiveEventId == null)
                throw new YagoException("Не найдена дилемма для установки выбора.");

            var activeEvent = GameEventsDataset.Get(cycle.ActiveEventId);
            var episode = activeEvent.Episode;
            var dilemma = episode.Dilemma;
            HandlePrologue(episode.PrologueSlides, colony);
            if (dilemma is DilemmaSelect dilemmaSelect)
                HandleDilemmaSelect(dilemmaSelect, command.DilemmaResolving, colony);

            cycle.SetStepNumber(cycle.StepNumber, activeEvent: null, isCycleEnded: false);

            var list = new List<IEntity> { colony, cycle };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);
            return new SetChoiceResult();
        }

        private static void HandlePrologue(IReadOnlyList<PrologueSlide> prologueSlides, Colony colony)
        {
            var colonyStats = colony.Stats;
            var parameters = prologueSlides.SelectMany(x => x.Parameters).ToList();
            if (!parameters.Any())
                return;
            colonyStats.SetEpisodeParameters(parameters, isCycleOver: false, isProglogue: true);
        }

        private static void HandleDilemmaSelect(
            DilemmaSelect dilemmaSelect,
            string dilemmaResolving,
            Colony colony)
        {
            var choice = dilemmaSelect.GetChoice(Guid.Parse(dilemmaResolving));
            var colonyStats = colony.Stats;
            var (isAvailable, mesasge) = choice.CheckAvailability(colonyStats);
            if (!isAvailable)
                throw new YagoException(mesasge, 400);

            colonyStats.SetEpisodeParameters(choice.Parameters, isCycleOver: false);
        }        

        public record SetChoiceCommand(long UserId, string DilemmaResolving) : IRequest<SetChoiceResult>;
        public record SetChoiceResult();
    }
}
