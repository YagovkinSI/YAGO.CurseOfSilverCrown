using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    public class UserEntity : IdentityUser<long>
    {
        public DateTime RegisteredAtUtc { get; private set; }
        public DateTime LastActivityAtUtc { get; private set; }

        protected UserEntity() { }

        public UserEntity(
            long id,
            string userName,
            string? email,
            DateTime registeredAtUtc,
            DateTime lastActivityAtUtc)
        {
            Id = id;
            UserName = userName;
            Email = email;
            RegisteredAtUtc = registeredAtUtc;
            LastActivityAtUtc = lastActivityAtUtc;
        }

        public void UpdateLastActivity() { LastActivityAtUtc = DateTime.UtcNow; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<UserEntity>();
            model.HasKey(m => m.Id);
        }
    }
}
