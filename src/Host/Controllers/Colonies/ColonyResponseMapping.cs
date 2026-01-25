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

        public static IReadOnlyDictionary<ColonyParameterResponseType, double> ToColonyPatameters(this ColonyWithShipAndContracts source)
        {
            return new Dictionary<ColonyParameterResponseType, double>
            ([
                new (ColonyParameterResponseType.Solars, source.Colony.Solars),
                new(ColonyParameterResponseType.SolarsIncome, source.SolarIncome),
                new(ColonyParameterResponseType.GavernorType, source.GavernorType),
                new(ColonyParameterResponseType.Population, source.Population),
                new(ColonyParameterResponseType.ZonesOccupied, source.ZonesOccupied),
                new(ColonyParameterResponseType.ZonesTotal, source.Ship.Zones),
                new(ColonyParameterResponseType.CodeOfLaws, (int)source.Colony.CodeOfLaws),
                new(ColonyParameterResponseType.Ship, source.Colony.ShipId)
            ]);
        }
    }
}
