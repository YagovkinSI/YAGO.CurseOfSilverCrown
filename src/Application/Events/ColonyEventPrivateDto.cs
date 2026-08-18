using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Events
{
    public class ColonyEventPrivateDto
    {
        public ColonyEvent ColonyEvent { get; }
        public GameEvent GameEvent { get; }
        public ColonyState ColonyState { get; }

        public ColonyEventPrivateDto(
            ColonyEvent colonyEvent,
            GameEvent gameEvent,
            ColonyState colonyState)
        {
            if (colonyEvent.EventCode != gameEvent.Code)
                throw new YagoException("Не совпадают идентификаторы событий.");

            ColonyEvent = colonyEvent;
            GameEvent = gameEvent;
            ColonyState = colonyState;
        }
    }

    public class ColonyEventSummaryDto
    {
        public ColonyEvent ColonyEvent { get; }
        public GameEvent GameEvent { get; }

        public ColonyEventSummaryDto(
            ColonyEvent colonyEvent,
            GameEvent gameEvent)
        {
            if (colonyEvent.EventCode != gameEvent.Code)
                throw new YagoException("Не совпадают идентификаторы событий.");

            ColonyEvent = colonyEvent;
            GameEvent = gameEvent;
        }
    }
}
