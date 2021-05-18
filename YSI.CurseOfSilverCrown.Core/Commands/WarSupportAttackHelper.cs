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
    public static class WarSupportAttackHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            int initiatorId, Unit warCommand)
        {
            var organization = await context.Domains
                .Include(o => o.Vassals)
                .Include(o => o.Units)
                .SingleAsync(o => o.Id == organizationId);

            var commands = organization.Units
                .Where(c => c.InitiatorDomainId == initiatorId);

            //получаем список соседей до которых можем дойти
            var targets = RouteHelper.GetAvailableRoutes(context, organization);

            var blockedOrganizationsIds = new List<int>();

            //не нападаем на тех на кого защищаем
            blockedOrganizationsIds.AddRange(commands
                        .Where(c => c.Type == enArmyCommandType.WarSupportDefense)
                        .Select(c => c.TargetDomainId.Value));

            //не нападаем на тех на кого уже есть приказ нападения
            blockedOrganizationsIds.AddRange(commands
                                .Where(c => c.Type == enArmyCommandType.War)
                                .Select(c => c.TargetDomainId.Value));

            //не нападаем на тех на кого уже есть приказ помощь в нападении
            blockedOrganizationsIds.AddRange(commands
                                .Where(c => c.Type == enArmyCommandType.WarSupportAttack && c.Id != warCommand?.Id)
                                .Select(c => c.TargetDomainId.Value));

            //не нападаем на своё королевство
            var kingdomIds = context.Domains
                    .GetAllDomainsIdInKingdoms(organization);
            blockedOrganizationsIds.AddRange(kingdomIds);

            var targetIds = targets.Select(t => t.Id);
            var targetOrganizations = await context.Domains
                .Include(o => o.Vassals)
                .Include(o => o.Units)
                .Where(o => targetIds.Contains(o.Id))
                .Where(o => !blockedOrganizationsIds.Contains(o.Id))
                .ToListAsync();

            return targetOrganizations;
        }

        public async static Task<List<Domain>> GetAvailableTargets2(ApplicationDbContext context)
        {
            var organizations = context.Domains;

            return await organizations
                .ToListAsync();
        }
    }
}
