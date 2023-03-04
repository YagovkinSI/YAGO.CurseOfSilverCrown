using System.Linq;

namespace YSI.CurseOfSilverCrown.Core.Database.Commands
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
