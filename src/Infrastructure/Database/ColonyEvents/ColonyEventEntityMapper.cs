using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Infrastructure.Database.ColonyEvents
{
    internal static class ColonyEventEntityMapper
    {
        public static ColonyEventEntity ToEntity(this ColonyEvent colonyEvent)
        {
            return new ColonyEventEntity(
                colonyEvent.Id,
                colonyEvent.ColonyId,
                colonyEvent.EventCode,
                colonyEvent.CreatedAtUtc,
                colonyEvent.TurnNumber,
                colonyEvent.IsRead,
                colonyEvent.IsCompleted);
        }

        public static ColonyEvent ToDomain(this ColonyEventEntity colonyEvent)
        {
            return new ColonyEvent(
                colonyEvent.Id,
                colonyEvent.ColonyId,
                colonyEvent.EventCode,
                colonyEvent.CreatedAtUtc,
                colonyEvent.TurnNumber,
                colonyEvent.IsRead,
                colonyEvent.IsCompleted);
        }
    }
}
