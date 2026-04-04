using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record ChioceResponse(
        Guid Id,
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters)
        : SlideResponse(Title, ImageName, Text, Parameters);
}
