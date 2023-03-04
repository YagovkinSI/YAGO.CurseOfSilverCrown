using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Turns;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class GameSessionHelper
    {
        public static string GetName(ApplicationDbContext context, Turn turn)
        {
            var session = context.GameSessions.Single(s => s.StartSeesionTurnId <= turn.Id && s.EndSeesionTurnId >= turn.Id);
            var number = turn.Id - session.StartSeesionTurnId + 1;
            var month = 3574 + (session.NumberOfGame - 1) * 200 + number - 1;
            return $"{month / 12 + 1} год, {month % 12 + 1} месяц (ход {number})";
        }
    }
}
