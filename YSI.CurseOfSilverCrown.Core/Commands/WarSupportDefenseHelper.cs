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
            string initiatorId, Command warSupportDefenseCommand)
        {
            var organization = await context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Vassals)
                .Include(o => o.Commands)
                .SingleAsync(o => o.Id == organizationId);

            var commands = organization.Commands
                .Where(c => c.InitiatorOrganizationId == initiatorId);

            //получаем список соседей до которых можем дойти
            var targets = await RouteHelper.GetAvailableRoutes(context, organization);

            var blockedOrganizationsIds = new List<string>();

            //не защищаем тех на кого нападаем
            blockedOrganizationsIds.AddRange(commands
                        .Where(c => c.Type == enCommandType.War || (c.Type == enCommandType.Rebellion && c.Warriors > 0))
                        .Select(c => c.TargetOrganizationId));

            //не защищаем тех на кого уже есть приказ защиты
            blockedOrganizationsIds.AddRange(commands
                                .Where(c => c.Type == enCommandType.WarSupportDefense && c.Id != warSupportDefenseCommand?.Id)
                                .Select(c => c.TargetOrganizationId));

            //не нападаем на тех на кого уже есть приказ помощь в нападении
            blockedOrganizationsIds.AddRange(commands
                                .Where(c => c.Type == enCommandType.WarSupportAttack)
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
