using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record MyColony(
        long Id,
        long UserId,
        string Name,
        IReadOnlyDictionary<ColonyParameterResponseType, double> ColonyParameters)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            ColonyParameters);
}

