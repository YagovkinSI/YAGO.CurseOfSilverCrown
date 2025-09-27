using YAGO.World.Domain.Users;

namespace YAGO.World.Host.Models.Authorization.Mappings
{
    public static class AuthorizationDataMapping
    {
        public static AuthorizationData ToAuthorizationData(this User? source)
        {
            if (source == null)
                return AuthorizationData.NotAuthorized;

            var userPivate = new UserPrivate(
                source.Id,
                source.Name,
                source.Email,
                source.RegistrationDate,
                source.LastActivity
                );

            return new AuthorizationData(userPivate);
        }
    }
}
