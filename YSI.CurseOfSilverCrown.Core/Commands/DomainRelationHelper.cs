using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class DomainRelationHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId)
        {
            var organization = await context.Domains.FindAsync(organizationId);

            var result = context.Domains.AsQueryable();

            //Не пишем отношения к себе
            result.Where(d => d.Id != organizationId);

            //убираем отношения со своим королевством
            var kingdomIds = KingdomHelper.GetAllDomainsIdInKingdoms(context.Domains, organization);
            result.Where(d => !kingdomIds.Contains(d.Id));

            //Убираем тех к кому уже есть приказы
            result.Where(d => !organization.Relations.Any(r => r.TargetDomainId == d.Id));

            return result;
        }

        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context, int organizationId, Command command = null)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
