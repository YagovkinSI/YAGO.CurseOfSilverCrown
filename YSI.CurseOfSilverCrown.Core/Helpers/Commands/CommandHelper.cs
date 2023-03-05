using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Commands
{
    public class CommandHelper
    {
        public static void CheckAndFix(ApplicationDbContext context, int domainId, int personId)
        {
            if (!context.Commands.Any(c => c.DomainId == domainId &&
                   c.InitiatorPersonId == personId))
            {
                var domain = context.Domains.Find(domainId);
                CommandCreateForNewTurnHelper.CreateNewCommandsForOrganizations(context, personId, domain);
            }
        }
    }
}
