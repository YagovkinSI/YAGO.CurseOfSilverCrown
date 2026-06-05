using System.Collections.Generic;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class GameEventChangeList
    {
        public IReadOnlyList<KeyValueParameter> ColonyStats { get; }
        public IReadOnlyList<string> NewQuests { get; }
        public IReadOnlyList<ActionAvailableRequirement> AvailableRequirements { get; }

        public GameEventChangeList(
            IReadOnlyList<KeyValueParameter> colonyStats,
            IReadOnlyList<string> newQuests,
            IReadOnlyList<ActionAvailableRequirement>? availableRequirements = null)
        {
            ColonyStats = colonyStats;
            NewQuests = newQuests;
            AvailableRequirements = availableRequirements ?? [];
        }
    }
}
