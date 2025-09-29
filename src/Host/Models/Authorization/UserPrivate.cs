using System;

namespace YAGO.World.Host.Models.Authorization
{
    public class UserPrivate
    {
        public long Id { get; }
        public string UserName { get; }
        public string? Email { get; }
        public DateTime Registered { get; }
        public DateTime LastActivity { get; }
        public bool IsTemporary { get; }

        public UserPrivate(
            long id,
            string userName,
            string? email,
            DateTime registered,
            DateTime lastActivity,
            bool isTemporary)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Registered = registered;
            LastActivity = lastActivity;
            IsTemporary = isTemporary;
        }
    }
}
