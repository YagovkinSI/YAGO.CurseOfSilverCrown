using System;

namespace YAGO.World.Domain.CurrentUsers
{
    public class User
    {
        public long Id { get; }
        public string UserName { get; }
        public string? Email { get; }
        public DateTime Registered { get; }
        public DateTime LastActivity { get; }

        public User(
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
