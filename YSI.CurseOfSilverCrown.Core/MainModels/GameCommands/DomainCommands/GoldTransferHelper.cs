using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.Core.MainModels.GameCommands.DomainCommands
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
