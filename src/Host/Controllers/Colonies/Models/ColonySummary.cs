using System;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record ColonySummary(
        Guid Id,
        long UserId,
        string Name);
}
