using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        IReadOnlyList<KeyValueParameter> ColonyParameters)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
