using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record MyColony(
        long Id,
        long UserId,
        string Name,
        IReadOnlyList<KeyValueParameter> ColonyParameters)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            ColonyParameters);
}

