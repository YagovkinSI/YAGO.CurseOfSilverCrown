using YAGO.World.Domain.Colonies;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.MyUsers
{
    public static class ColonyResponseMapping
    {
        public static MyDataResponse<MyColony> ToMyDataResponse(this Colony? source)
        {
            if (source == null)
                return MyDataResponse<MyColony>.NotAuthorized;

            var result = new MyColony(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                source.SolarsIncome,
                source.Reputation,
                source.Population,
                source.ZonesOccupied,
                source.ZonesTotal);

            return new MyDataResponse<MyColony>(
                IsAuthorized: true, 
                result);
        }
    }
}
