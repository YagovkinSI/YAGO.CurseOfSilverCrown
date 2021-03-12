using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    internal static class UserExtensions
    {
        public static Domain.CurrentUsers.User ToDomainCurrentUser(this User source)
        {
            return new Domain.CurrentUsers.User(
                source.Id,
                source.UserName!,
                source.Email,
                source.Register,
                source.LastActivityTime);
        }
        public static User ToDatabase(this Domain.CurrentUsers.User source)
        {
            return new User
            {
                Id = source.Id,
                UserName = source.UserName,
                Email = source.Email,
                Register = source.Registered,
                LastActivityTime = source.LastActivity
            };
        }

        public static Domain.YagoEntities.YagoEntity ToYagoEntity(this User source)
        {
            return new Domain.YagoEntities.YagoEntity
            (
                source.Id,
                Domain.YagoEntities.Enums.YagoEntityType.User,
                source.UserName
            );
        }
    }
}
