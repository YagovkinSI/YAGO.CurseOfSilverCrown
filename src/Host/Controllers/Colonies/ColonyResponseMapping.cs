using YAGO.World.Application.Colonies;
using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.MyUsers
{
    public static class ColonyResponseMapping
    {
        public static MyDataResponse<MyColony> ToMyDataResponse(
            this ColonyWithShipAndBuildingsDto? source)
        {
            if (source == null)
                return new MyDataResponse<MyColony>(IsAuthorized: true, Data: null);

            var result = new MyColony(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                source.Colony.Solars,
                source.SolarIncome,
                source.Reputation,
                source.Population,
                source.ZonesOccupied,
                source.Ship.Zones);

            return new MyDataResponse<MyColony>(
                IsAuthorized: true,
                result);
        }
    }
}
