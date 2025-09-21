using YAGO.World.Domain.Users;

namespace YAGO.World.Infrastructure.Database.Models.Users.Mappings
{
    internal static class UserExtensions
    {
        public static User ToDomain(this UserEntity source)
        {
            return new User(
                source.Id,
                source.UserName!,
                source.Email,
                source.Register,
                source.LastActivityTime);
        }
        public static UserEntity ToDatabase(this User source)
        {
            return new UserEntity
            {
                Id = source.Id,
                UserName = source.Name,
                Email = source.Email,
                Register = source.Registered,
                LastActivityTime = source.LastActivity
            };
        }
    }
}
