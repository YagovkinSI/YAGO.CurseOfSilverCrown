using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.Users
{
    public class UserEntity : IdentityUser<long>
    {
        public DateTime RegisteredAtUtc { get; private set; }
        public DateTime LastActivityAtUtc { get; private set; }
        public bool IsTemporary { get; private set; }

        public virtual List<ColonyEntity>? Colonies { get; set; }

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

        public void UpdateFromDomain(User source)
        {
            if (Id != source.Id)
                throw new YagoException("не совпадение идентиифкаторов при обновлении сущности БД.");

            UserName = source.UserName;
            Email = source.Email;
            LastActivityAtUtc = source.LastActivityAtUtc;
            IsTemporary = source.IsTemporary;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<UserEntity>();
            model.HasKey(m => m.Id);
        }
    }
}
