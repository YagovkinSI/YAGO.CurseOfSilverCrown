using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class DomainRelationHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId)
        {
            var organization = await context.Domains.FindAsync(organizationId);

            var result = context.Domains
                .Where(d => d.Id <= Constants.MaxPlayerCount)
                .ToList();

            //Не пишем отношения к себе
            result.RemoveAll(d => d.Id == organizationId);

            //убираем отношения со своим королевством
            var kingdomIds = KingdomHelper.GetAllDomainsIdInKingdoms(context.Domains, organization);
            result.RemoveAll(d => kingdomIds.Contains(d.Id));

            //Убираем тех к кому уже есть приказы
            result.RemoveAll(d => organization.Relations.Any(r => r.TargetDomainId == d.Id));

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
