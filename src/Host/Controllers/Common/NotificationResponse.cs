using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;

namespace YAGO.World.Host.Controllers.Common
{
    public record NotificationResponse(
        string Title,
        IllustrationType Illustration,
        string Text,
        IReadOnlyList<ColonyParameter> Parameters);
}
