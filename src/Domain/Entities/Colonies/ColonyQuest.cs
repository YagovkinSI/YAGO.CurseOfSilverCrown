using System;
using YAGO.World.Domain.Entities.Quests;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyQuest
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Progress { get; }
        public QuestType Type { get; }

        public ColonyQuest(
            ColonyStats colonyStats,
            Quest quest)
        {
            Id = quest.Id;
            Name = quest.Name;
            Progress = "0/1";
            Type = quest.Type;
        }
    }
}
