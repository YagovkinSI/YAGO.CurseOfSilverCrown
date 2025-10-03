using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace YAGO.World.Infrastructure.Database.Users
{
    public class UserEntity : IdentityUser<long>
    {
        public DateTime RegisteredAtUtc { get; private set; }
        public DateTime LastActivityAtUtc { get; private set; }
        public bool IsTemporary { get; private set; }

        protected UserEntity() { }

        public UserEntity(
            long id,
            string userName,
            string? email,
            DateTime registeredAtUtc,
            DateTime lastActivityAtUtc,
            bool isTemporary)
        {
            Id = id;
            UserName = userName;
            Email = email;
            RegisteredAtUtc = registeredAtUtc;
            LastActivityAtUtc = lastActivityAtUtc;
            IsTemporary = isTemporary;
        }

        internal static UserEntity CreateNew(
            string userName, 
            string? email)
        {
            return new UserEntity(
                id: default,
                userName: userName,
                email: email,
                registeredAtUtc: DateTime.UtcNow,
                lastActivityAtUtc: DateTime.UtcNow,
                isTemporary: false
            );
        }

        internal static UserEntity CreateTemporary()
        {
            return new UserEntity(
                id: default,
                userName: $"User_{new Random().Next(0, 99999999)}",
                email: null,
                registeredAtUtc: DateTime.UtcNow,
                lastActivityAtUtc: DateTime.UtcNow,
                isTemporary: true
            );
        }

        public void UpdateLastActivity() { LastActivityAtUtc = DateTime.UtcNow; }

        public void ConvertToPermanentAccount(string userName, string? email)
        {
            UserName = userName;
            Email = email;
            IsTemporary = false; 
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<UserEntity>();
            model.HasKey(m => m.Id);
        }
    }
}
