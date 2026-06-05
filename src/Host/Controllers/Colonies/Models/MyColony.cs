using System;
using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Colonies.MyQuests;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record MyColony(
        Guid Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters,
        IReadOnlyList<MyQuest> Quests,
        bool NewColonyAvailable,
        double Solars,
        double ZonesAvailable)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            ColonyParameters);
}

