using System;

namespace YAGO.World.Domain.CurrentUsers
{
    public class CurrentUser
    {
        public string Id { get; }
        public string UserName { get; }
        public string? Email { get; }
        public DateTime LastActivity { get; }

        public CurrentUser(
            string id,
            string userName,
            string? email,
            DateTime lastActivity)
        {
            Id = id;
            UserName = userName;
            Email = email;
            LastActivity = lastActivity;
        }
    }
}