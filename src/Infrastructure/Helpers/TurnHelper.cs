using YAGO.World.Infrastructure.Database.Models.Turns;

namespace YAGO.World.Infrastructure.Helpers
{
    public static class TurnHelper
    {
        public static string GetName(this Turn turn)
        {
            var month = 3574 + turn.Id - 1;
            return $"{(month / 12) + 1} год, {(month % 12) + 1} месяц (ход {turn.Id})";
        }
    }
}
