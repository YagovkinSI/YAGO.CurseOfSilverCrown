using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Entities.Colonies;
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
            var colonyPatameters = source.ToColonyPatameters();
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
            var colonyPatameters = source.ToColonyPatameters();

            return new ColonyDetails(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters);
        }

        public static IReadOnlyList<ColonyParameterResponse> ToColonyPatameters(
            this Colony source)
        {
            var colonyPatameters = new List<ColonyParameterResponse>();

            var colonyStats = source.Stats;
            var episodeCount = colonyStats.EpisodeCount;
            var colonySettings = colonyStats.Settings;

            if (episodeCount > 0)
            {
                colonyPatameters.Add(ColonyParameterResponseDataset.GetColonyName(source.Name));
                colonyPatameters.Add(ColonyParameterResponseDataset.Economic(colonyStats));
                colonyPatameters.Add(ColonyParameterResponseDataset.GetStation(
                    colonySettings.GetShipName(), colonySettings.ShipId, inOther: episodeCount > 1));
                colonyPatameters.Add(ColonyParameterResponseDataset.GetEpisodeCount(episodeCount));
            }
            if (episodeCount > 1)
            {
                colonyPatameters.Add(ColonyParameterResponseDataset.MoodTotal(colonyStats.MoodTotal.Value));
                colonyPatameters.Add(ColonyParameterResponseDataset.AttractivenessTotal(colonyStats));
                colonyPatameters.Add(ColonyParameterResponseDataset.AreaCapacity(colonyStats));
                colonyPatameters.Add(ColonyParameterResponseDataset.GetPopulation(colonyStats.PopulationTotal));
                colonyPatameters.Add(ColonyParameterResponseDataset.GetLaws(colonySettings.GetCodeOfLaws()));
            }

            return colonyPatameters
                .OrderBy(x => x.Weight)
                .ToList();
        }
    }
}
