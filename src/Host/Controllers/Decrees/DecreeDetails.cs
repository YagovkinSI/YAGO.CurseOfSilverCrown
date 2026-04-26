using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

namespace YAGO.World.Host.Controllers.Decrees
{
    public record DecreeDetails(
        long Id,
        string Name,
        string Image,
        string[] Text,
        IReadOnlyList<ColonyParameterResponse> Parameters,
        string[] Description);
}
