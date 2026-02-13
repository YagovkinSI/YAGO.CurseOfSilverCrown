using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Host.Controllers.Common
{
    public record NotificationResponse(
        string Title,
        string Illustration,
        string[] Text,
        IReadOnlyList<KeyValueParameter> Parameters);
}
