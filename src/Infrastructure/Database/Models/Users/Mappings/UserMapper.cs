using System;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Infrastructure.Database.Models.Users.Mappings
{
    internal static class UserMapper
    {
        public static User ToDomain(this UserEntity source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(source.UserName))
                throw new YagoException("Имя пользователя не может быть NULL или пустым.");

            return new User(
                source.Id,
                source.UserName!,
                source.Email,
                source.Register,
                source.LastActivityTime);
        }

        public static UserEntity ToEntity(this User source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new UserEntity(
                source.Id,
                source.Name,
                source.Email,
                source.RegistrationDate,
                source.LastActivity);
        }
    }
}
