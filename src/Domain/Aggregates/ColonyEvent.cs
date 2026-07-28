using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Aggregates
{
    public class ColonyEvent
    {
        public GameEvent GameEvent { get; }
        public ColonyState ColonyStats { get; }

        public ColonyEvent(
            ColonyState colonyStats,
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
