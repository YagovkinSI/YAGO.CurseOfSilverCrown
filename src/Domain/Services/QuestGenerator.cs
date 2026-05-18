using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Quests;

namespace YAGO.World.Domain.Services
{
    public interface IQuestGenerator
    {
        QuestGeneratorResult Generate(IReadOnlyList<Quest> questDataset, Colony colony);
    }

    public class QuestGenerator : IQuestGenerator
    {
        public QuestGeneratorResult Generate(
            IReadOnlyList<Quest> questDataset,
            Colony colony)
        {
            var quests = new List<ColonyQuest>();
            foreach (var quest in questDataset)
            {
                if (!colony.Quests.Any(x => x.Id == quest.Id) && quest.Check(colony))
                {
                    var colonyQuest = new ColonyQuest(colony.Stats, quest);
                    quests.Add(colonyQuest);
                }
            }

            return new QuestGeneratorResult(quests);
        }
    }

    public record QuestGeneratorResult(IReadOnlyList<ColonyQuest> Quests);
}
