using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static MyDataResponse<MyColony> ToMyDataResponse(
            this ColonyWithDetails? source)
        {
            if (source == null)
                return new MyDataResponse<MyColony>(IsAuthorized: true, Data: null);

            var result = source.ToMyColony();

            return new MyDataResponse<MyColony>(
                IsAuthorized: true,
                result);
        }

        public static MyColony ToMyColony(
            this ColonyWithDetails source)
        {
            var colonyPatameters = source.ToColonyPatameters();

            return new MyColony(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                colonyPatameters);
        }

        public static PaginatedResponse<ColonyDetails> ToPaginatedResponse(
            this PaginatedData<ColonyWithDetails> source)
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

        public static ColonyDetails ToDetails(this ColonyWithDetails source)
        {
            var colonyPatameters = source.ToColonyPatameters();

            return new ColonyDetails(
                source.Colony.Id,
                source.Colony.UserId,
                source.Colony.Name,
                colonyPatameters);
        }

        public static IReadOnlyList<KeyValueParameter> ToColonyPatameters(
            this ColonyWithDetails source)
        {
            var budget = new Budget(
                source.Colony,
                source.Companies,
                source.Ship);
            var loyality = new Loyalty(
                source.Colony,
                source.Companies);
            var population = new Population(
                source.Colony,
                source.Companies);
            var areaCapacity = new AreaCapacity(
                source.Colony,
                source.Companies,
                source.Ship);

            return new List<KeyValueParameter>
            ([
                new KeyValueParameter(ColonyParameterNames.Economic_Reserves, source.Colony.Solars),
                new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, budget.Balance),
                new KeyValueParameter(ColonyParameterNames.Loyalty_Total, loyality.Total),
                new KeyValueParameter(ColonyParameterNames.Population_Total, population.Total),
                new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, areaCapacity.Occupied),
                new KeyValueParameter(ColonyParameterNames.AreaCapacity_Total, areaCapacity.Total),
                new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws, (int)source.Colony.CodeOfLaws),
                new KeyValueParameter(ColonyParameterNames.Ship_Id, source.Colony.ShipId)
            ]);
        }
    }
}
