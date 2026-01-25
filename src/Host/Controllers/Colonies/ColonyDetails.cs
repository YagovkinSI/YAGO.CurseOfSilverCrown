using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        IReadOnlyDictionary<ColonyParameterResponseType, double> ColonyParameters)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
