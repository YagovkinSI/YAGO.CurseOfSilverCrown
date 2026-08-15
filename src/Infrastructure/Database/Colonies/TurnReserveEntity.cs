using System;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record TurnReserveEntity(
        int TurnsAvailableFixed,
        DateTime LastTurnTimeAtUtc);
}
