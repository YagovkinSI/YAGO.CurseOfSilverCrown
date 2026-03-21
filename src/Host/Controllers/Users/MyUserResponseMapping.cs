using YAGO.World.Domain.Entities.Users;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Users.Models;

namespace YAGO.World.Host.Controllers.Users
{
    public static class MyUserResponseMapping
    {
        public static ApiResponse<MyUser> ToMyDataResponse(this User? source)
        {
            if (source == null)
                return ApiResponse<MyUser>.Empty;

            var myUser = new MyUser(
                source.Id,
                source.UserName,
                source.RegisteredAtUtc,
                source.LastActivityAtUtc,
                source.IsTemporary);

            return ApiResponse<MyUser>.CreateSuccess(data: myUser);
        }
    }
}
