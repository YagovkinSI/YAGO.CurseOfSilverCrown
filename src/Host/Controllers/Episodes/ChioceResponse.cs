using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record ChoiceResponse(
        Guid Id,
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters,
        bool IsAvailable,
        string ButtonName)
        : SlideResponse(Title, ImageName, Text, Parameters);
}
