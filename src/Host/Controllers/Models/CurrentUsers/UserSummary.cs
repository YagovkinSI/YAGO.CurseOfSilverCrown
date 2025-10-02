using System;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers
{
    public record UserSummary(
        long Id,
        string UserName,
        DateTime LastActivity);
}
