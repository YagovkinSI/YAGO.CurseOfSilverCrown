using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Web.Database;

namespace YSI.CurseOfSilverCrown.Web.AI
{
    public class AIHelper
    {
        public static void AICommandsPrepare(ApplicationDbContext context)
        {
            var currentTurn = context.Turns.
                  SingleOrDefault(t => t.IsActive);
            if (currentTurn == null)
                return;

            var domains = context.Domains.ToList();
            foreach (var domain in domains)
            {
                if (domain.User != null && domain.User.LastActivityTime > DateTime.Now - TimeSpan.FromDays(5))
                    continue;

                var userAi = new UserAI(context, domain.Id, currentTurn);
                userAi.SetCommands();
            }
        }
    }
}
