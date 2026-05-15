using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Quests;

namespace YAGO.World.Application.Colonies.Queries.GetMyColony
{
    public class GetGetColonyQuestHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetGetColonyQuestQuery, GetGetColonyQuestResult>
    {
        public async Task<GetGetColonyQuestResult> Handle(GetGetColonyQuestQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetGetColonyQuestResult(ColonyQuest: null);

            var quest = QuestDataset.Get(command.QuestId);
            var colonyQuest = new ColonyQuest(colony.Stats, quest);
            return new GetGetColonyQuestResult(colonyQuest);
        }
    }

    public record GetGetColonyQuestQuery(long UserId, Guid QuestId) : IRequest<GetGetColonyQuestResult>;
    public record GetGetColonyQuestResult(ColonyQuest? ColonyQuest);
}
