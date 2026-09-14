using System.Collections.Generic;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Stations
{
    public static class AsteroidDataset
    {
        private static readonly Dictionary<string, Asteroid> _data = new()
        {
            { AsteroidConstants.Large, CreateLarge() },
            { AsteroidConstants.Medium, CreateMedium() },
            { AsteroidConstants.Small, CreateSmall() }
        };

        public static IReadOnlyDictionary<string, Asteroid> Data => _data;

        public static Asteroid Get(AsteroidId id) => _data[ToCode(id)];

        public static string ToCode(AsteroidId id)
        {
            return id switch
            {
                AsteroidId.Large => AsteroidConstants.Large,
                AsteroidId.Medium => AsteroidConstants.Medium,
                AsteroidId.Small => AsteroidConstants.Small,
                _ => throw new YagoException($"Неизвестный астероид: {id}"),
            };
        }

        public static Asteroid? Find(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _data.TryGetValue(code, out var asteroid) ? asteroid : null;
        }

        public static Asteroid GetRequired(string? code)
        {
            return Find(code)
                ?? throw new YagoException($"Неизвестный астероид: {code ?? string.Empty}");
        }

        private static Asteroid CreateLarge()
        {
            return new(
                AsteroidId.Large,
                name: "Крупный астероид",
                distanceToCeres: 0.6,
                distanceToAvalon: 0.1,
                miningModulesLimit: 12);
        }

        private static Asteroid CreateMedium()
        {
            return new(
                AsteroidId.Medium,
                name: "Средний астероид",
                distanceToCeres: 0.45,
                distanceToAvalon: 0.05,
                miningModulesLimit: 9);
        }

        private static Asteroid CreateSmall()
        {
            return new(
                AsteroidId.Small,
                name: "Малый астероид",
                distanceToCeres: 0.3,
                distanceToAvalon: 0.2,
                miningModulesLimit: 7);
        }
    }
}