using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue;
using YAGO.World.Domain.Entities.Quests;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.Commands.CompleteQuest
{
    public class CompleteQuestCommandHandler(
        IColonyRepository colonyRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<CompleteQuestCommand, ColonyEpisode>
    {
        public async Task<ColonyEpisode> Handle(CompleteQuestCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var quest = QuestDataset.Get(command.QuestId);
            var completeEpisode = quest.CompleteEpisode;
            HandleTextInput(quest, command.DilemmaResolving, colony);
            HandlePrologue(completeEpisode.PrologueSlides, colony);
            colony.RemoveQuest(quest.Id);

            var list = new List<IEntity> { colony };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);

            return new ColonyEpisode(completeEpisode, colony.Stats);
        }

        private void HandleTextInput(Quest quest, string? dilemmaResolving, Colony colony)
        {
            if (quest.PrologueSlide.TextInput == null)
                return;

            if (string.IsNullOrWhiteSpace(dilemmaResolving))
                throw new YagoException("Значение ввода не может быть пустым.");

            if (quest.Id == nameof(ColonyNameQuest))
                colony.SetName(dilemmaResolving);
        }

        private static void HandlePrologue(IReadOnlyList<PrologueSlide> prologueSlides, Colony colony)
        {
            var colonyStats = colony.Stats;
            var parameters = prologueSlides.SelectMany(x => x.Parameters).ToList();
            if (!parameters.Any())
                return;
            colonyStats.SetEpisodeParameters(parameters, isCycleOver: false, isProglogue: true);
        }
    }

    public record CompleteQuestCommand(long UserId, string QuestId, string? DilemmaResolving) : IRequest<ColonyEpisode>;
}
