using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Commands.DomainCommands
{
    public static class VassalTransferHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            int initiatorId, Command command = null)
        {
            var organization = await context.Domains.FindAsync(organizationId);

            var commands = organization.Commands
                .Where(c => c.InitiatorCharacterId == initiatorId);

            var result = new List<Domain>();
            if (organization.SuzerainId == null)
                result.Add(organization);
            result.AddRange(organization.Vassals);

            var blockedOrganizationsIds = new List<int>();

            //не передаём тех на кого уже есть приказ передачи
            blockedOrganizationsIds.AddRange(commands
                                .Where(c => c.Type == CommandType.VassalTransfer && c.Id != command?.Id)
                                .Select(c => c.TargetDomainId.Value));

            result = result.Where(o => !blockedOrganizationsIds.Contains(o.Id)).ToList();

            return result;
        }


        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context, int organizationId, Command command = null)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
