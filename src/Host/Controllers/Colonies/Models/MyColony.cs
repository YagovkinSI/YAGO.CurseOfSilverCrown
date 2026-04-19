using System;
using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record MyColony(
        Guid Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters,
        bool AutoRunCycle,
        bool NewColonyAvailable,
        double Solars,
        double ZonesAvailable)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            ColonyParameters);
}

