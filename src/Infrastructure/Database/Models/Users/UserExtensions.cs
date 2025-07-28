using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    internal static class UserExtensions
    {
        public static CurrentUser ToDomainCurrentUser(this User source)
        {
            return new CurrentUser(
                source.Id,
                source.UserName!,
                source.Email,
                source.Register,
                source.LastActivityTime);
        }
        public static User ToDatabase(this CurrentUser source)
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
