using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class GameEventChangeList
    {
        public IReadOnlyList<KeyValueParameter> ColonyStats { get; }
        public IReadOnlyList<string> NewQuests { get; }

        public GameEventChangeList(
            IReadOnlyList<KeyValueParameter> colonyStats, 
            IReadOnlyList<string> newQuests)
        {
            ColonyStats = colonyStats;
            NewQuests = newQuests;
        }
    }
}
