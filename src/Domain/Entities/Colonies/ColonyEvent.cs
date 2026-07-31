using System;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyEvent
    {
        public string EventId { get; }
        public bool IsRead { get; private set; }
        public DateTime CreatedAtUtc { get; }

        public ColonyEvent(
            string eventId, 
            bool isRead, 
            DateTime createdAtUtc)
        {
            EventId = eventId;
            IsRead = isRead;
            CreatedAtUtc = createdAtUtc;
        }

        public static ColonyEvent CreateNew(string eventId)
        {
            return new ColonyEvent(
                eventId,
                isRead: false,
                DateTime.UtcNow);
        }

        public void SetRead()
        {
            IsRead = true;
        }
    }
}
