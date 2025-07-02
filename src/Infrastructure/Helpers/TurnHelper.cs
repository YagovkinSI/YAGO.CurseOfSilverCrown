using System;

namespace YAGO.World.Infrastructure.Helpers
{
    public static class TurnHelper
    {
        private static readonly DateTime _eraStart = new(2024, 07, 14, 0, 0, 0, DateTimeKind.Utc);
        private static readonly string[] seasons = new[] { "Зима", "Весна", "Лето", "Осень" };

        public static string GetName(DateTime turnDate, int turnId)
        {
            var diffDays = (int)(turnDate - _eraStart).TotalDays;

            var year = diffDays / 4 + 1;

            var seasonIndex = diffDays % 4;
            var season = seasons[seasonIndex];

            return $"{year} год, {season} (ход {turnId})";
        }
    }
}
