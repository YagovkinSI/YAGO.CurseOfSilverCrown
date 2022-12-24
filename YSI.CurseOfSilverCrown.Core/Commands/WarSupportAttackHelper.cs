using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class WarSupportAttackHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context)
        {
            var organizations = await context.Domains
                .Where(d => d.Id <= Constants.MaxPlayerCount)
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .ToListAsync();

            return organizations;
        }
    }
}
