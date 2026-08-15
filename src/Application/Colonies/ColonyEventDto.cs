using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Colonies
{
    public class ColonyEventDto
    {
        public ColonyEvent ColonyEvent { get; }
        public GameEvent GameEvent { get; }
        public ColonyState ColonyState { get; }

        public ColonyEventDto(
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
}
