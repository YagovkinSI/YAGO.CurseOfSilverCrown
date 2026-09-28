using System.Collections.Generic;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Stations
{
    public static class StationModelDataset
    {
        private static readonly Dictionary<string, Station> _data = new()
        {
            { StationModelConstants.Dawn_342, CreateDawn342() },
            { StationModelConstants.Resolute_120, CreateResolute120() }
        };

        public static IReadOnlyDictionary<string, Station> Data => _data;

        public static Station Get(StationModelId id) => _data[ToCode(id)];

        public static Station? Find(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _data.TryGetValue(code, out var station) ? station : null;
        }

        public static Station GetRequired(string? code)
        {
            return Find(code)
                ?? throw new YagoException($"Неизвестная модель станции: {code ?? string.Empty}");
        }

        public static string ToCode(StationModelId id)
        {
            return id switch
            {
                StationModelId.Dawn_342 => StationModelConstants.Dawn_342,
                StationModelId.Resolute_120 => StationModelConstants.Resolute_120,
                _ => throw new YagoException($"Неизвестная модель станции: {id}"),
            };
        }

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