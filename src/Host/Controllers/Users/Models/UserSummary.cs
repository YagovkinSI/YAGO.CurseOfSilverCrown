using System;

namespace YAGO.World.Host.Controllers.Users.Models
{
    public record UserSummary(
        long Id,
        string UserName,
        DateTime LastActivity);
}
