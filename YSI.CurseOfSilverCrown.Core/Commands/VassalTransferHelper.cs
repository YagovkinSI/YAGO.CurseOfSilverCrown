using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class VassalTransferHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            int initiatorId, Command command = null)
        {
            var organization = await context.Domains.FindAsync(organizationId);

            var commands = organization.Commands
                .Where(c => c.InitiatorPersonId == initiatorId);

            var result = new List<Domain>();
            if (organization.SuzerainId == null)
                result.Add(organization);
            result.AddRange(organization.Vassals);

            var blockedOrganizationsIds = new List<int>();

            //не передаём тех на кого уже есть приказ передачи
            blockedOrganizationsIds.AddRange(commands
                                .Where(c => c.Type == enCommandType.VassalTransfer && c.Id != command?.Id)
                                .Select(c => c.TargetDomainId.Value));

            result = result.Where(o => !blockedOrganizationsIds.Contains(o.Id)).ToList();

            return result;
        }


        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context, int organizationId, Command command = null)
        {
            var organizations = context.Domains
                .Where(d => d.Id <= Constants.MaxPlayerCount);

            return await organizations.ToListAsync();
        }
    }
}
