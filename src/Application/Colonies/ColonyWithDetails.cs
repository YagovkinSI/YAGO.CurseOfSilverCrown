using YAGO.World.Domain.ColonyStats.Parameters;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies
{
    public record ColonyWithDetails(
        Colony Colony,
        ColonyCompanies Companies)
    { }
}
