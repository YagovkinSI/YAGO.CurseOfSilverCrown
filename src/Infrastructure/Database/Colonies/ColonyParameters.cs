using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal record ColonyParameters(
        bool Named,
        ColonyStatsEntity States,
        IReadOnlyList<string> EventIds);
}
