using YAGO.World.Domain.ColonyStats.Parameters;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Ships;

namespace YAGO.World.Application.Colonies
{
    public record ColonyWithDetails(
        Colony Colony,
        Ship Ship,
        ColonyCompanies Companies)
    { }
}
