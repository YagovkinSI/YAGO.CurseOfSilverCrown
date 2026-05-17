using System;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Quests
{
    public class Quest
    {
        public Guid Id { get; }
        public string Name { get; }
        public QuestType Type { get; }
        public PrologueSlide PrologueSlide { get; }
        public Episode CompleteEpisode { get; }

        public Quest(
            Guid id,
            string name,
            QuestType type,
            PrologueSlide prologueSlide,
            Episode completeEpisode)
        {
            Id = id;
            Name = name;
            Type = type;
            PrologueSlide = prologueSlide;
            CompleteEpisode = completeEpisode;
        }
    }
}
