using System;

namespace YAGO.World.Domain.CurrentUsers
{
    public class CurrentUser
    {
        public long Id { get; }
        public string UserName { get; }
        public string? Email { get; }
        public DateTime Registered { get; }
        public DateTime LastActivity { get; }

        public CurrentUser(
            long id,
            string userName,
            string? email,
            DateTime registered,
            DateTime lastActivity)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Registered = registered;
            LastActivity = lastActivity;
        }
    }
}
