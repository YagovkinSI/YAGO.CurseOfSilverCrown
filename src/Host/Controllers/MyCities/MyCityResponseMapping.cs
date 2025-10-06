using YAGO.World.Domain.Cities;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.MyUsers
{
    public static class MyCityResponseMapping
    {
        public static MyDataResponse<MyCity> ToMyDataResponse(this City? source)
        {
            if (source == null)
                return MyDataResponse<MyCity>.NotAuthorized;

            var myCity = new MyCity(
                source.Id,
                source.UserId,
                source.Name,
                source.Descripion);

            return new MyDataResponse<MyCity>(
                IsAuthorized: true,
                myCity);
        }
    }
}
