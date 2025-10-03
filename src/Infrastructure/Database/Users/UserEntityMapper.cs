using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Infrastructure.Database.Users
{
    internal static class UserEntityMapper
    {
        public static User ToDomain(this UserEntity source)
        {
            return string.IsNullOrWhiteSpace(source.UserName)
                ? throw new YagoException("Имя пользователя не может быть NULL или пустым.")
                : new User(
                source.Id,
                source.UserName,
                source.Email,
                source.RegisteredAtUtc,
                source.LastActivityAtUtc,
                source.IsTemporary);
        }

        public static UserEntity ToEntity(this User source)
        {
            return new UserEntity(
                source.Id,
                source.UserName,
                source.Email,
                source.RegisteredAtUtc,
                source.LastActivityAtUtc,
                source.IsTemporary);
        }
    }
}
