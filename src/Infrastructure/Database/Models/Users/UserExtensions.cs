using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    internal static class UserExtensions
    {
        internal static Domain.Users.User ToDomain(this User source)
        {
            return new Domain.Users.User(
                source.Id,
                source.UserName!,
                source.LastActivityTime);
        }

        internal static Domain.CurrentUsers.CurrentUser ToDomainCurrentUser(this User source)
        {
            return new Domain.CurrentUsers.CurrentUser(
                source.Id,
                source.UserName!,
                source.Email,
                source.LastActivityTime);
        }

        public static User ToDatabase(this CurrentUser source)
        {
            return new User
            {
                Id = source.Id,
                UserName = source.UserName,
                Email = source.Email,
                LastActivityTime = source.LastActivity
            };
        }
    }
}
