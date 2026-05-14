using System;

namespace YAGO.World.Domain.Entities.Quests
{
    public class Quest
    {
        public Guid Id { get; }
        public string Name { get; }
        public QuestType Type { get; }

        public Quest(Guid id, string name, QuestType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }
    }
}
