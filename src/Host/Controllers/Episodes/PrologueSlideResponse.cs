using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record PrologueSlideResponse(
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters,
        string ContinueButtonName)
        : SlideResponse(Title, ImageName, Text, Parameters);
}
