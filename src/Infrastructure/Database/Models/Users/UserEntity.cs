using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    public class UserEntity : IdentityUser<long>
    {
        public DateTime Register { get; private set; }
        public DateTime LastActivityTime { get; private set; }

        protected UserEntity() { }

        public UserEntity(
            long id,
            string userName,
            string? email,
            DateTime register,
            DateTime lastActivityTime)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Register = register;
            LastActivityTime = lastActivityTime;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<UserEntity>();
            model.HasKey(m => m.Id);
        }

        public void UpdateLastActivityTime() { LastActivityTime = DateTime.UtcNow; }
    }
}
