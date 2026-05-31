using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities;
using YAGO.World.Domain.Entities.GameEvents;
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

            var colonyStats = colony.Stats;
            colonyStats.SetEpisodeParameters(quest.Changes, isProglogue: true);
            colony.RemoveQuest(quest.Id);

            var list = new List<IEntity> { colony };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);

            return new ColonyEpisode(quest.CompleteEpisode, colony.Stats);
        }
    }

    public record CompleteQuestCommand(long UserId, string QuestId, string? DilemmaResolving) : IRequest<ColonyEpisode>;
}
