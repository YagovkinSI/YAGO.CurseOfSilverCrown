using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue;
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
            HandlePrologue(episode.Slides, colony);

            if (activeEvent.Id == nameof(ColonyNameEvent))
            {
                colony.SetName(command.DilemmaResolving); 
                var colonyStats = colony.Stats;
                colonyStats.SetEpisodeParameters(episode.Slides[episode.Slides.Count - 1].Parameters, isCycleOver: false);
            }
            else
            {
                var slide = episode.Slides.Single(x => x.Id == command.DilemmaResolving);
                var colonyStats = colony.Stats;
                var (isAvailable, mesasge) = (slide as Choice).CheckAvailability(colonyStats);
                if (!isAvailable)
                    throw new YagoException(mesasge, 400);
                colonyStats.SetEpisodeParameters(slide.Parameters, isCycleOver: false);
            }

            cycle.SetStepNumber(cycle.StepNumber, activeEvent: null, isCycleEnded: false);

            var list = new List<IEntity> { colony, cycle };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);
            return new SetChoiceResult();
        }

        private static void HandlePrologue(IReadOnlyList<Slide> prologueSlides, Colony colony)
        {
            var colonyStats = colony.Stats;
            var parameters = prologueSlides
                .Where(x => !x.Buttons.Any(y => y.Action != null))
                .SelectMany(x => x.Parameters)
                .ToList();
            if (!parameters.Any())
                return;
            colonyStats.SetEpisodeParameters(parameters, isCycleOver: false, isProglogue: true);
        }

        public record SetChoiceCommand(long UserId, string DilemmaResolving) : IRequest<SetChoiceResult>;
        public record SetChoiceResult();
    }
}
