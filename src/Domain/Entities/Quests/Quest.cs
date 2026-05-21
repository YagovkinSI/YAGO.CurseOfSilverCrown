using System;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Quests
{
    public class Quest
    {
        public string Id { get; }
        public string Title { get; }
        public QuestType Type { get; }
        public PrologueSlide PrologueSlide { get; }
        public Episode CompleteEpisode { get; }

        public Quest(
            string id,
            string name,
            QuestType type,
            PrologueSlide prologueSlide,
            Episode completeEpisode)
        {
            Id = id;
            Title = name;
            Type = type;
            PrologueSlide = prologueSlide;
            CompleteEpisode = completeEpisode;
        }
    }
}
