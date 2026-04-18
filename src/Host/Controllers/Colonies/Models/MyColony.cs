using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record MyColony(
        Guid Id,
        long UserId,
        string Name,
        IReadOnlyList<KeyValueParameter> ColonyParameters,
        bool AutoRunCycle)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            ColonyParameters);
}

