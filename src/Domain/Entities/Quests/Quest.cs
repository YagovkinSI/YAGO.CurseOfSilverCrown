using System.Collections.Generic;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Quests
{
    public class Quest
    {
        public string Id { get; }
        public string Title { get; }
        public QuestType Type { get; }
        public Episode PrologueEpisode { get; }
        public Episode CompleteEpisode { get; }
        public IReadOnlyList<KeyValueParameter> Changes { get; }

        public Quest(
            string id,
            string name,
            QuestType type,
            Episode prologueEpisode,
            Episode completeEpisode,
            IReadOnlyList<KeyValueParameter> changes)
        {
            Id = id;
            Title = name;
            Type = type;
            PrologueEpisode = prologueEpisode;
            CompleteEpisode = completeEpisode;
            Changes = changes;
        }
    }
}
