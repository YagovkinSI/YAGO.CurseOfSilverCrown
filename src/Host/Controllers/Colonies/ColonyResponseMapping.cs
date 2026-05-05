using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static ApiResponse<MyColony> ToApiResponse(
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
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);
            var autoRunCycle = source.IsAutoRunCycle();
            var newColonyAvailable = source.IsNewColonyAvailable();
            var solars = source.Stats.Resources.Solars;
            var zoneAvailable = source.Stats.ZonesAvailable;

            return new MyColony(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters,
                autoRunCycle,
                newColonyAvailable,
                solars,
                zoneAvailable);
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
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);

            return new ColonyDetails(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters);
        }
    }
}
