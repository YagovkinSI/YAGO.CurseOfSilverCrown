using System;
using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record ColonyDetails(
        Guid Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
