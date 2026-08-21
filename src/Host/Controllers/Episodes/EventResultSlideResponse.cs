using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EventResultSlideResponse(
        bool Show,
        string Title,
        string? ImageName,
        string[] Text,
        IReadOnlyList<ColonyParameterResponse> Parameters);
}
