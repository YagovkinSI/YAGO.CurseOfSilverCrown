using System;

namespace YAGO.World.Host.Controllers.MyUsers
{
    public record UserSummary(
        long Id,
        string UserName,
        DateTime LastActivity);
}
