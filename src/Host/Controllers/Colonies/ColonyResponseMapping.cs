using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static MyDataResponse<MyColony> ToMyDataResponse(
            this Colony? source)
        {
            if (source == null)
                return new MyDataResponse<MyColony>(IsAuthorized: true, Data: null);

            var result = source.ToMyColony();

            return new MyDataResponse<MyColony>(
                IsAuthorized: true,
                result);
        }

        public static MyColony ToMyColony(
            this Colony source)
        {
            var colonyPatameters = source.ToColonyPatameters();

            return new MyColony(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters);
        }

        public static PaginatedResponse<ColonyDetails> ToPaginatedResponse(
            this PaginatedData<Colony> source)
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

        public static ColonyDetails ToDetails(this Colony source)
        {
            var colonyPatameters = source.ToColonyPatameters();

            return new ColonyDetails(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters);
        }

        public static IReadOnlyList<KeyValueParameter> ToColonyPatameters(
            this Colony source)
        {
            return new List<KeyValueParameter>
            ([
                new KeyValueParameter(ColonyParameterNames.Economic_Reserves, source.Solars),
                new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, source.BudgetBalance),
                new KeyValueParameter(ColonyParameterNames.Mood_Total, source.MoodTotalCacl()),
                new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, source.AttractivenessTotalCalc()),
                new KeyValueParameter(ColonyParameterNames.Population_Total, source.PopulationTotal),
                new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, source.ZonesOccupied),
                new KeyValueParameter(ColonyParameterNames.AreaCapacity_Total, source.ZonesTotal),
                new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws, (int)source.CodeOfLaws),
                new KeyValueParameter(ColonyParameterNames.Ship_Id, source.ShipId),
                new KeyValueParameter(ColonyParameterNames.CurrentWeek, source.CurrentWeek),
            ]);
        }
    }
}
