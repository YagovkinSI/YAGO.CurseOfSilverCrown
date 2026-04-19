using System.Collections.Generic;
using YAGO.World.Host.Controllers.Colonies.Models;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record PrologueSlideResponse(
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<ColonyParameterResponse> Parameters,
        string ContinueButtonName)
        : SlideResponse(Title, ImageName, Text, Parameters);
}
