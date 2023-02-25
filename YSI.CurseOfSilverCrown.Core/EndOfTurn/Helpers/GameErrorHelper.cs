using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Helpers
{
    public class GameErrorHelper
    {
        public static void CheckAndFix(ApplicationDbContext context, int domainId, int personId)
        {
            if (!context.Commands.Any(c => c.DomainId == domainId &&
                   c.InitiatorPersonId == personId))
            {
                var domain = context.Domains.Find(domainId);
                CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(context, personId, domain);
            }
        }
    }
}
