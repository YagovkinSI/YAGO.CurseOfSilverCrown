using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAGO.World.Host.Database;
using YAGO.World.Host.Database.Domains;

namespace YAGO.World.Host.Helpers.Commands.UnitCommands
{
    public static class WarSupportAttackHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
