using System;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyEventEntity(
        string EventId,
        bool IsRead,
        DateTime CreatedAtUtc);
}
