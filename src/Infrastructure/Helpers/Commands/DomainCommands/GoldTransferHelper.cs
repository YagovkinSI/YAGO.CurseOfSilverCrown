using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Helpers.Commands.DomainCommands
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
