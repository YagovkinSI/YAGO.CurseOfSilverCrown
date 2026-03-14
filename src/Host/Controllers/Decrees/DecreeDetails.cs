using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Decrees
{
    public record DecreeDetails(
        long Id,
        string Name,
        string Image,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters,
        string[] Description);
}
