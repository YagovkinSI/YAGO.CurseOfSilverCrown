using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Commands.DomainCommands
{
    public static class VassalTransferHelper
    {
        public static async Task<IEnumerable<Organization>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            Command command = null)
        {
            var organization = await context.Domains.FindAsync(organizationId);

            var commands = organization.Commands
                .ToList();

            var result = new List<Organization>();
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


        public static async Task<IEnumerable<Organization>> GetAvailableTargets2(ApplicationDbContext context, int organizationId, Command command = null)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
