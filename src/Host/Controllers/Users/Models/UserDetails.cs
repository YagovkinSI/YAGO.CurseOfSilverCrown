using System;

namespace YAGO.World.Host.Controllers.Users.Models
{
    public record UserDetails(
        long Id,
        string UserName,
        DateTime Registered,
        DateTime LastActivity)
        : UserSummary(
            Id,
            UserName,
            LastActivity);
}
