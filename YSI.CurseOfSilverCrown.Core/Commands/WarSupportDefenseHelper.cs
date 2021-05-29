using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class WarSupportDefenseHelper
    {
        public static async Task<IEnumerable<DomainMin>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            int initiatorId, Unit warSupportDefenseCommand)
        {
            var organization = await context.Domains
                .Include(o => o.Vassals)
                .Include(o => o.Units)
                .SingleAsync(o => o.Id == organizationId);

            var commands = organization.Units
                .Where(c => c.InitiatorDomainId == initiatorId);

            //получаем список соседей до которых можем дойти
            var targets = RouteHelper.GetAvailableRoutes(context, organization, warSupportDefenseCommand.PositionDomainId.Value, 2);

            var availableOrganizationsIds = new List<int>();
            var kingdomIds = KingdomHelper.GetAllDomainsIdInKingdoms(context.Domains, organization);
            availableOrganizationsIds.AddRange(kingdomIds);

            var blockedOrganizationsIds = new List<int>();

            var targetIds = targets.Select(t => t.Id);
            var targetOrganizations = context.GetAllDomainMin()
                .Result
                .Where(o => targetIds.Contains(o.Id))
                .Where(o => availableOrganizationsIds.Contains(o.Id))
                .Where(o => !blockedOrganizationsIds.Contains(o.Id));

            return targetOrganizations;
        }
    }
}
