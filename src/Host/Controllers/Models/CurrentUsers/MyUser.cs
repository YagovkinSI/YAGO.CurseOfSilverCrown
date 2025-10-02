using System;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers
{
    public record MyUser(
        long Id,
        string UserName,
        string? Email,
        DateTime Registered,
        DateTime LastActivity,
        bool IsTemporary)
        : UserDetails(
            Id,
            UserName,
            Registered,
            LastActivity);
}
