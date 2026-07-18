using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Aggregates
{
    public class ColonyEpisode
    {
        public Episode Episode { get; }
        public ColonyStats ColonyStats { get; }

        public ColonyEpisode(Episode episode, ColonyStats colonyStats)
        {
            Episode = episode;
            ColonyStats = colonyStats;
        }
    }
}
