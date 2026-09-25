using System.Collections.Generic;

namespace YAGO.World.Domain.Stations
{
    public static class StationModelDataset
    {
        private static readonly Dictionary<StationModelId, Station> _data = new()
        {
            { StationModelId.Dawn_342, CreateDawn342() },
            { StationModelId.Resolute_120, CreateResolute120() }
        };

        public static IReadOnlyDictionary<StationModelId, Station> Data => _data;

        private static Station CreateDawn342()
        {
            return new(
                StationModelId.Dawn_342,
                name: "Рассвет-342",
                modulesTotal: 140);
        }

        private static Station CreateResolute120()
        {
            return new(
                StationModelId.Resolute_120,
                name: "Решимость-120",
                modulesTotal: 420);
        }
    }
}
