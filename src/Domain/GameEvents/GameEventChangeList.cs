using System.Collections.Generic;

namespace YAGO.World.Domain.GameEvents
{
    public class GameEventChangeList
    {
        public IReadOnlyList<RequirementsParameter> Requirements { get; }
        public IReadOnlyList<KeyValueParameter> ColonyStats { get; }
        public IReadOnlyList<string> NewQuests { get; }
        public StringKey? StringChange { get; }

        public GameEventChangeList(
            IReadOnlyList<KeyValueParameter> colonyStats,
            IReadOnlyList<string> newQuests,
            IReadOnlyList<RequirementsParameter>? requirements = null,
            StringKey? stringChange = null)
        {
            ColonyStats = colonyStats;
            NewQuests = newQuests;
            Requirements = requirements ?? [];
            StringChange = stringChange;
        }
    }
}
