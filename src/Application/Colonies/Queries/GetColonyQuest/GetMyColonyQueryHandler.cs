using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyQuests;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Application.Colonies.Queries.GetColonyQuest
{
    public class GetColonyQuestHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetColonyQuestQuery, GetGetColonyQuestResult>
    {
        public async Task<GetGetColonyQuestResult> Handle(GetColonyQuestQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return new GetGetColonyQuestResult(ColonyQuest: null);

            var quest = GameEventsDataset.Get(command.QuestId);
            var colonyQuest = new ColonyQuest(colony.Stats, quest);
            return new GetGetColonyQuestResult(colonyQuest);
        }
    }

    public record GetColonyQuestQuery(long UserId, string QuestId) : IRequest<GetGetColonyQuestResult>;
    public record GetGetColonyQuestResult(ColonyQuest? ColonyQuest);
}
