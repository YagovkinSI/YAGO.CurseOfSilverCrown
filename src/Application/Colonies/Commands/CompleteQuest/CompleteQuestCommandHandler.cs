using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.Quests;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.Commands.CompleteQuest
{
    public class CompleteQuestCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<CompleteQuestCommand, ColonyEpisode>
    {
        public async Task<ColonyEpisode> Handle(CompleteQuestCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var quest = QuestDataset.Get(command.QuestId);
            var colonyQuest = new ColonyQuest(colony.Stats, quest);

            //TODO

            var episode = new Episode(
                quest.Id.ToString(),
                quest.Name,
                [new PrologueSlide(quest.Name, ImageSet.Feature, ["Молодец"], [], "Всё")],
                dilemma: null);
            return new ColonyEpisode(episode, colony.Stats);
        }
    }

    public record CompleteQuestCommand(long UserId, Guid QuestId, string? DilemmaResolving) : IRequest<ColonyEpisode>;
}
