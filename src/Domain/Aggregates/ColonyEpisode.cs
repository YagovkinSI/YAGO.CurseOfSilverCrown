using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Aggregates
{
    public class ColonyEpisode
    {
        public Episode Episode { get; }
        public ColonyState ColonyStats { get; }

        public ColonyEpisode(Episode episode, ColonyState colonyStats)
        {
            Episode = episode;
            ColonyStats = colonyStats;
        }
    }
}
