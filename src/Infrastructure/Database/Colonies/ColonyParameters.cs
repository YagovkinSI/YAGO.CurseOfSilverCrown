using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyParameters(
        string DatabaseName,
        bool Named,
        TurnReserveEntity TurnReserve,
        StationEntity Station,
        ColonyStatsEntity States,
        IReadOnlyList<ColonyEventEntity> Events);
}
