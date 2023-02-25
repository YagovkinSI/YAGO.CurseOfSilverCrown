using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.AI
{
    public class AIHelper
    {
        public static void AICommandsPrepare(ApplicationDbContext context)
        {
            var currentTurn = context.Turns.
                  SingleOrDefault(t => t.IsActive);
            if (currentTurn == null)
                return;

            var persons = context.Persons.ToList();
            foreach (var person in persons)
            {
                if (person.User != null && person.User.LastActivityTime > DateTime.Now - TimeSpan.FromDays(5))
                    continue;

                var isSameInitiator = person.Domains
                    .Single()
                    .Commands
                    .Any(u => u.InitiatorPersonId == person.Id);
                if (!isSameInitiator)
                    continue;

                var userAi = new UserAI(context, person.Id, currentTurn);
                userAi.SetCommands();
            }
        }
    }
}
