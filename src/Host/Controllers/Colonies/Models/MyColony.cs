using System;
using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Events.Models;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record MyColony(
        Guid Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters,
        IReadOnlyList<ColonyEventResponse> Quests,
        bool NewColonyAvailable,
        double Solars,
        double ZonesAvailable)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            ColonyParameters);
}

