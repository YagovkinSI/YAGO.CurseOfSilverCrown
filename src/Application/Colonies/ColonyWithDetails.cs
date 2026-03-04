using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Colonies.Ships;

namespace YAGO.World.Application.Colonies
{
    public record ColonyWithDetails(
        Colony Colony,
        Ship Ship,
        ColonyCompanies Companies)
    { }
}
