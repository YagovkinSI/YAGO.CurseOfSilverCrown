using System;
using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record ChoiceResponse(
        string Id,
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<ColonyParameterResponse> Parameters,
        bool IsAvailable,
        string ButtonName)
        : SlideResponse(Id, Title, ImageName, Text, Parameters, [], ButtonName);
}
