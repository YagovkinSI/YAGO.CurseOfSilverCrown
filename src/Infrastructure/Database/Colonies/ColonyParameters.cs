using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyParameters(
        bool Named,
        StationEntity Station,
        ColonyStatsEntity States,
        IReadOnlyList<ColonyEventEntity> Events);
}
