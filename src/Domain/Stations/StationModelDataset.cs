using System.Collections.Generic;

namespace YAGO.World.Domain.Stations
{
    public static class StationModelDataset
    {
        private static readonly Dictionary<StationModelId, StationModel> _data = new()
        {
            { StationModelId.Dawn_342, CreateDawn342() },
            { StationModelId.Resolute_120, CreateResolute120() }
        };

        public static IReadOnlyDictionary<StationModelId, StationModel> Data => _data;

        private static StationModel CreateDawn342()
        {
            return new(
                StationModelId.Dawn_342,
                modulesTotal: 140);
        }

        private static StationModel CreateResolute120()
        {
            return new(
                StationModelId.Resolute_120,
                modulesTotal: 420);
        }
    }
}
