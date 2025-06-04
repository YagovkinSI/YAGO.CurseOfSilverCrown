namespace YAGO.World.Infrastructure.Database.Models.Users
{
    public static class UserExtensions
    {
        internal static Domain.Users.User ToDomain(this User user)
        {
            if (user == null)
                return null;

            return new Domain.Users.User(
                user.Id,
                user.UserName);
        }
    }
}
