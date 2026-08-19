using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Reforms
{
    public record ReformDetails(
        string Code,
        string Name,
        string Image,
        IReadOnlyList<ColonyParameterResponse> Parameters,
        IReadOnlyList<ColonyParameterResponse> Requirements,
        string[] Description,
        SlideButtonResponse Button);
}
