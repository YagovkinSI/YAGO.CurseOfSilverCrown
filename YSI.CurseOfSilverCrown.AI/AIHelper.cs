using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;

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

            var characters = context.Characters.ToList();
            foreach (var character in characters)
            {
                if (character.User != null && character.User.LastActivityTime > DateTime.Now - TimeSpan.FromDays(5))
                    continue;

                var isSameInitiator = character.Domains
                    .Single()
                    .Commands
                    .Any(u => u.InitiatorPersonId == character.Id);
                if (!isSameInitiator)
                    continue;

                var userAi = new UserAI(context, character.Id, currentTurn);
                userAi.SetCommands();
            }
        }
    }
}
