using YAGO.World.Domain.Users;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Users
{
    public static class UserResponsesMapping
    {
        public static ApiResponse<UserPrivate> ToMyDataResponse(this User? source)
        {
            if (source == null)
                return ApiResponse<UserPrivate>.Empty;

            var myUser = new UserPrivate(
                source.Id,
                source.UserName,
                source.RegisteredAtUtc,
                source.LastActivityAtUtc,
                source.IsTemporary);

            return ApiResponse<UserPrivate>.CreateSuccess(data: myUser);
        }
    }
}
