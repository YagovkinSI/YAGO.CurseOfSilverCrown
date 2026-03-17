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
        public static ApiResponse<MyColony> ToMyDataResponse(
            this Colony? source)
        {
            if (source == null)
                return ApiResponse<MyColony>.CreateSuccess(data: null);

            var result = source.ToMyColony();

            return ApiResponse<MyColony>.CreateSuccess(data: result);
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
            var colonyStats = source.Stats;

            return new List<KeyValueParameter>
            ([
                new KeyValueParameter(ColonyParameterNames.Economic_Reserves, colonyStats.Solars),
                new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, colonyStats.BudgetBalance),
                new KeyValueParameter(ColonyParameterNames.Mood_Total, colonyStats.MoodTotalCacl()),
                new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, colonyStats.AttractivenessTotalCalc()),
                new KeyValueParameter(ColonyParameterNames.Population_Total, colonyStats.PopulationTotal),
                new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, colonyStats.ZonesOccupied),
                new KeyValueParameter(ColonyParameterNames.AreaCapacity_Total, colonyStats.ZonesTotal),
                new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws, (int)colonyStats.CodeOfLaws),
                new KeyValueParameter(ColonyParameterNames.Ship_Id, source.ShipId),
                new KeyValueParameter(ColonyParameterNames.CurrentWeek, colonyStats.CurrentWeek),
            ]);
        }
    }
}
