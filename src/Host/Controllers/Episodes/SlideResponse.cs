using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record SlideResponse(
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters);
}
