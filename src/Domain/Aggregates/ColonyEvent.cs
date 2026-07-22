using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Aggregates
{
    public class ColonyEvent
    {
        public GameEvent GameEvent { get; }
        public ColonyStates ColonyStats { get; }

        public ColonyEvent(
            ColonyStates colonyStats,
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
