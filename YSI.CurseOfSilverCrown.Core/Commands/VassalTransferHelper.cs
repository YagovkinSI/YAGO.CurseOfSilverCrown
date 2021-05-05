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
    public static class VassalTransferHelper
    {
        public static async Task<IEnumerable<Organization>> GetAvailableTargets(ApplicationDbContext context, string organizationId, Command command = null)
        {
            var organization = await context.Organizations
                .Include(o => o.Vassals)
                .SingleAsync(o => o.Id == organizationId);

            var result = new List<Organization>();
            if (organization.SuzerainId == null)
                result.Add(organization);
            result.AddRange(organization.Vassals);

            var blockedOrganizationsIds = new List<string>();

            //не передаём тех на кого уже есть приказ передачи
            blockedOrganizationsIds.AddRange(organization.Commands
                                .Where(c => c.Type == enCommandType.VassalTransfer && c.Id != command?.Id)
                                .Select(c => c.TargetOrganizationId));

            result = result.Where(o => !blockedOrganizationsIds.Contains(o.Id)).ToList();

            return result;
        }


        public static async Task<IEnumerable<Organization>> GetAvailableTargets2(ApplicationDbContext context, string organizationId, Command command = null)
        {
            var organizations = context.Organizations;

            return await organizations.ToListAsync();
        }
    }
}
