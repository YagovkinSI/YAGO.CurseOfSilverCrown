using System;

namespace YAGO.World.Host.Controllers.MyUsers
{
    public record MyUser(
        long Id,
        string UserName,
        DateTime Registered,
        DateTime LastActivity,
        bool IsTemporary)
        : UserDetails(
            Id,
            UserName,
            Registered,
            LastActivity);
}
