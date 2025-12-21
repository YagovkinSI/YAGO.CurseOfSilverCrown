using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Host.Controllers.Common
{
    public record NotificationResponse(
        string Title,
        string Illustration,
        string Text,
        IReadOnlyList<ColonyParameter> Parameters);
}
