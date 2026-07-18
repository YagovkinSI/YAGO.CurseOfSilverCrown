using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Decrees
{
    public record DecreeDetails(
        long Id,
        string Name,
        string Image,
        string[] Text,
        IReadOnlyList<ColonyParameterResponse> Parameters,
        IReadOnlyList<ColonyParameterResponse> Requirements,
        string[] Description,
        SlideButtonResponse Button);
}
