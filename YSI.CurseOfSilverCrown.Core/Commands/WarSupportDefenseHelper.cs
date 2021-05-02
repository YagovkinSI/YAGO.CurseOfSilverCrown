using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class WarSupportDefenseHelper
    {
        public static async Task<IEnumerable<Organization>> GetAvailableTargets(ApplicationDbContext context, string organizationId,
            Command warSupportDefenseCommand)
        {
            var organization = await context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Vassals)
                .Include(o => o.Commands)
                .SingleAsync(o => o.Id == organizationId);

            //получаем список соседей до которых можем дойти
            var targets = await RouteHelper.GetAvailableRoutes(context, organization);

            var blockedOrganizationsIds = new List<string>();

            //не защищаем тех на кого нападаем
            blockedOrganizationsIds.AddRange(organization.Commands
                        .Where(c => c.Type == enCommandType.War)
                        .Select(c => c.TargetOrganizationId));

            //не защищаем тех на кого уже есть приказ защиты
            blockedOrganizationsIds.AddRange(organization.Commands
                                .Where(c => c.Type == enCommandType.WarSupportDefense && c.Id != warSupportDefenseCommand?.Id)
                                .Select(c => c.TargetOrganizationId));

            var targetIds = targets.Select(t => t.Id);
            var targetOrganizations = await context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Vassals)
                .Include(o => o.Commands)
                .Where(o => targetIds.Contains(o.Id))
                .Where(o => o.OrganizationType == enOrganizationType.Lord &&
                    !blockedOrganizationsIds.Contains(o.Id))
                .ToListAsync();

            return targetOrganizations;
        }
    }
}
