using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Commands;
using YSI.CurseOfSilverCrown.Web.Database.Domains;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Commands.DomainCommands
{
    public static class GoldTransferHelper
    {
        public const int MaxGoldTransfer = 1500;

        public static async Task<IEnumerable<Domain>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
            Command command)
        {
            var organizations = context.Domains.AsQueryable();

            //не передаём себе
            organizations = organizations
                .Where(o => o.Id != organizationId);

            return await organizations.ToListAsync();
        }
    }
}
