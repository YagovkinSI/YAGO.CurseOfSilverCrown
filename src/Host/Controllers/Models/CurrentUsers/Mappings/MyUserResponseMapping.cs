using YAGO.World.Application.Common.Models;
using YAGO.World.Domain.Users;

namespace YAGO.World.Host.Controllers.Models.CurrentUsers.Mappings
{
    public static class MyUserResponseMapping
    {
        public static MyDataResponse<MyUser> ToMyDataResponse(this User? source)
        {
            if (source == null)
                return MyDataResponse<MyUser>.NotAuthorized;

            var myUser = new MyUser(
                source.Id,
                source.UserName,
                source.Email,
                source.RegisteredAtUtc,
                source.LastActivityAtUtc,
                source.IsTemporary);

            return new MyDataResponse<MyUser>(
                IsAuthorized: true, 
                myUser);
        }
    }
}
