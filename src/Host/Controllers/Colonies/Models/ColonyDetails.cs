using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record ColonyDetails(
        Guid Id,
        long UserId,
        string Name,
        IReadOnlyList<KeyValueParameter> ColonyParameters)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
