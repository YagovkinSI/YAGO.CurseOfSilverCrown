using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Domains;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Commands.UnitCommands
{
    public static class WarSupportAttackHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
