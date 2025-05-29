using YAGO.World.Infrastructure.Database.Models.Turns;

namespace YAGO.World.Infrastructure.Helpers
{
    public static class TurnHelper
    {
        public static string GetName(int turnId)
        {
            var month = 3574 + turnId - 1;
            return $"{(month / 12) + 1} год, {(month % 12) + 1} месяц (ход {turnId})";
        }
    }
}
