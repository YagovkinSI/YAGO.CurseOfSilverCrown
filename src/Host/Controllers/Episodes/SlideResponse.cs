using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Common
{
    public record SlideResponse(
        string Title,
        string Illustration,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters);
}
