using System.Collections.Generic;
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
            var colonyPatameters = source.ToColonyPatameters();

            return new MyColony(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                colonyPatameters);
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
            var colonyPatameters = source.ToColonyPatameters();

            return new ColonyDetails(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                colonyPatameters);
        }

        public static IReadOnlyList<ColonyParameter> ToColonyPatameters(this ColonyWithShipAndContracts source)
        {
            return new List<ColonyParameter>
            ([
                new ColonyParameter(ColonyParameterType.Solars, source.Colony.Solars),
                new ColonyParameter(ColonyParameterType.SolarIncome, source.SolarIncome),
                new ColonyParameter(ColonyParameterType.GavernorType, source.GavernorType),
                new ColonyParameter(ColonyParameterType.Population, source.Population),
                new ColonyParameter(ColonyParameterType.ZonesOccupied, source.ZonesOccupied),
                new ColonyParameter(ColonyParameterType.ZonesTotal, source.Ship.Zones),
                new ColonyParameter(ColonyParameterType.CodeOfLaws, (int)source.Colony.CodeOfLaws),
                new ColonyParameter(ColonyParameterType.Ship, source.Colony.ShipId)
            ]);
        }
    }
}
