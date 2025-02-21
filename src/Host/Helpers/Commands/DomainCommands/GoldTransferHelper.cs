using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Commands.DomainCommands
{
    public static class GoldTransferHelper
    {
        public const int MaxGoldTransfer = 1500;

        public static async Task<IEnumerable<Organization>> GetAvailableTargets(ApplicationDbContext context, int organizationId,
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
