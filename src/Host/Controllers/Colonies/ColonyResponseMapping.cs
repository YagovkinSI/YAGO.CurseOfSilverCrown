using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static MyDataResponse<MyColony> ToMyDataResponse(
            this ColonyWithShipAndBuildings? source)
        {
            if (source == null)
                return new MyDataResponse<MyColony>(IsAuthorized: true, Data: null);

            var result = source.ToMyColony();

            return new MyDataResponse<MyColony>(
                IsAuthorized: true,
                result);
        }

        public static MyColony ToMyColony(
            this ColonyWithShipAndBuildings source)
        {
            return new MyColony(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                source.Colony.Solars,
                source.SolarIncome,
                source.Reputation,
                source.Population,
                source.ZonesOccupied,
                source.Ship.Zones);
        }

        public static PaginatedResponse<ColonyDetails> ToPaginatedResponse(
            this PaginatedData<ColonyWithShipAndBuildings> source)
        {
            var data = source.Data
                .Select(x => x.ToDetails())
                .ToArray();

            return new PaginatedResponse<ColonyDetails>(
                data,
                source.Total,
                source.Page,
                source.Limit);
        }

        public static ColonyDetails ToDetails(this ColonyWithShipAndBuildings source)
        {
            return new ColonyDetails(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                source.SolarIncome,
                source.Reputation,
                source.Population,
                source.ZonesOccupied);
        }
    }
}
