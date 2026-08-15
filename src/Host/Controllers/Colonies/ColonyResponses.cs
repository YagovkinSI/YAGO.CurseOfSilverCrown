using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Events.Models;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyPrivate(
        long Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters,
        IReadOnlyList<ColonyEventResponse> Quests,
        double Solars,
        double ZonesAvailable);

    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        IReadOnlyList<ColonyParameterResponse> ColonyParameters);

    public record ColonySummary(
        long Id,
        long UserId,
        string Name);
}

