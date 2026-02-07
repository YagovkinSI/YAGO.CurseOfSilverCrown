using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameter> ColonyParameters)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
