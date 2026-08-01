using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Aggregates
{
    public class ColonyEventAggregate
    {
        public ColonyEvent ColonyEvent { get; }
        public GameEvent GameEvent { get; }
        public ColonyState ColonyState { get; }

        public ColonyEventAggregate(
            ColonyEvent colonyEvent,
            GameEvent gameEvent,
            ColonyState colonyState)
        {
            if (colonyEvent.EventId != gameEvent.Id)
                throw new YagoException("Не совпадают идентификаторы событий.");

            ColonyEvent = colonyEvent;
            GameEvent = gameEvent;
            ColonyState = colonyState;
        }
    }
}
