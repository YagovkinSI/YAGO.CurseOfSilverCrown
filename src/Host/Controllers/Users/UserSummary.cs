using System;

namespace YAGO.World.Host.Controllers.Users
{
    public record UserSummary(
        long Id,
        string UserName,
        DateTime LastActivity);
}
