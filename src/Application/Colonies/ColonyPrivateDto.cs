using System.Collections.Generic;
using YAGO.World.Application.Events;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public record ColonyPrivateDto(
        Colony Colony,
        IReadOnlyList<ColonyEventSummaryDto> ColonyEvents);
}
