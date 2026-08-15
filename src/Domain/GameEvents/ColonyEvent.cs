using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;

namespace YAGO.World.Domain.GameEvents
{
    public class ColonyEvent : IEntity<long>
    {
        public long Id { get; private set; }
        public long ColonyId { get; private set; }
        public string EventCode { get; }
        public DateTime CreatedAtUtc { get; }
        public int TurnNumber { get; }
        public bool IsRead { get; private set; }
        public bool IsCompleted { get; private set; }

        public ColonyEvent(
            long id,
            long colonyId,
            string eventCode,
            DateTime createdAtUtc,
            int turnNumber,
            bool isRead,
            bool isCompleted)
        {
            Id = id;
            ColonyId = colonyId;
            EventCode = eventCode;
            CreatedAtUtc = createdAtUtc;
            TurnNumber = turnNumber;
            IsRead = isRead;
            IsCompleted = isCompleted;
        }

        public static ColonyEvent CreateNew(long colonyId, string eventCode, int turnNumber)
        {
            return new ColonyEvent(
                id: default,
                colonyId: colonyId,
                eventCode,
                DateTime.UtcNow,
                turnNumber,
                isRead: false,
                isCompleted: false);
        }

        public static ColonyEvent CreateFirstColonyEvent()
        {
            return CreateNew(default, nameof(ColonyNameEvent), turnNumber: 1);
        }

        public void SetId(long id)
        {
            if (Id == id)
                return;
            if (Id != default)
                throw new YagoException("Событие уже имеет идентификатор.");
            Id = id;
        }

        public void SetColonyId(long colonyId)
        {
            if (ColonyId == colonyId)
                return;
            if (ColonyId != default)
                throw new YagoException("Событие уже имеет колонию.");
            ColonyId = colonyId;
        }

        public void SetRead()
        {
            IsRead = true;
        }

        public void SetComplited()
        {
            IsCompleted = true;
        }
    }
}
