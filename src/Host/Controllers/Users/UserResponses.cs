using System;

namespace YAGO.World.Host.Controllers.Users
{
    public record UserPrivate(
        long Id,
        string UserName,
        DateTime Registered,
        DateTime LastActivity,
        bool IsTemporary);

    public record UserDetails(
        long Id,
        string UserName,
        DateTime Registered,
        DateTime LastActivity);

    public record UserSummary(
        long Id,
        string UserName,
        DateTime LastActivity);
}
