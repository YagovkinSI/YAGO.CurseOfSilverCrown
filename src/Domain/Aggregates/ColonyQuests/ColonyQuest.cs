using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Aggregates.ColonyQuests
{
    public class ColonyQuest
    {
        public GameEvent GameEvent { get; }
        public ColonyStats ColonyStats { get; }

        public ColonyQuest(
            ColonyStats colonyStats,
            GameEvent gameEvent)
        {
            GameEvent = gameEvent;
            ColonyStats = colonyStats;
        }

        public ColonyEpisode GetPrologueColonyEpisode()
        {
            return new ColonyEpisode(GameEvent.Episode, ColonyStats);
        }
    }
}
