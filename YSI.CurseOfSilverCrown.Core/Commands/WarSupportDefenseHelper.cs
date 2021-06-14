using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class WarSupportDefenseHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            int initiatorId, Unit warSupportDefenseCommand)
        {
            var organization = await context.Domains
                .Include(o => o.Vassals)
                .Include(o => o.Units)
                .SingleAsync(o => o.Id == organizationId);

            var commands = organization.Units
                .Where(c => c.InitiatorPersonId == initiatorId);

            //получаем список соседей до которых можем дойти
            var targets = RouteHelper.GetAvailableRoutes(context, warSupportDefenseCommand.PositionDomainId.Value, 2);

            var blockedOrganizationsIds = new List<int>();

            var targetIds = targets.Select(t => t.Id);
            var targetOrganizations = context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .Where(o => targetIds.Contains(o.Id))
                .Where(o => !blockedOrganizationsIds.Contains(o.Id));

            return targetOrganizations;
        }
    }
}
