using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class GameSessionHelper
    {
        public static string GetName(ApplicationDbContext context, Turn turn)
        {
            var session = context.GameSessions.Single(s => s.StartSeesionTurnId <= turn.Id && s.EndSeesionTurnId >= turn.Id);


            var number = turn.Id - session.StartSeesionTurnId + 1;
            var year = 587 + session.NumberOfGame * 10 + (turn.Id - 1) / 4;
            switch (turn.Id % 4)
            {
                case 1:
                    return $"{year} год, зима (ход {number})";
                case 2:
                    return $"{year} год, весна (ход {number})";
                case 3:
                    return $"{year} год, лето (ход {number})";
                case 4:
                default:
                    return $"{year} год, осень (ход {number})";
            }
        }
    }
}
