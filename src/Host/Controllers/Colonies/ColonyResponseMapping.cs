using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static MyDataResponse<MyColony> ToMyDataResponse(
            this ColonyWithShipAndContracts? source)
        {
            if (source == null)
                return new MyDataResponse<MyColony>(IsAuthorized: true, Data: null);

            var result = source.ToMyColony();

            return new MyDataResponse<MyColony>(
                IsAuthorized: true,
                result);
        }

        public static MyColony ToMyColony(
            this ColonyWithShipAndContracts source)
        {
            return new MyColony(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                source.Colony.Solars,
                source.SolarIncome,
                source.Challenges,
                source.Population,
                source.ZonesOccupied,
                source.Ship.Zones);
        }

        public static PaginatedResponse<ColonyDetails> ToPaginatedResponse(
            this PaginatedData<ColonyWithShipAndContracts> source)
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

        public static ColonyDetails ToDetails(this ColonyWithShipAndContracts source)
        {
            return new ColonyDetails(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                source.SolarIncome,
                source.Challenges,
                source.Population,
                source.ZonesOccupied);
        }
    }
}
