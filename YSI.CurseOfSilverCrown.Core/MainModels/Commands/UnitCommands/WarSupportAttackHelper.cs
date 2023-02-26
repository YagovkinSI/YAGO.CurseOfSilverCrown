using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;

namespace YSI.CurseOfSilverCrown.Core.MainModels.Commands.UnitCommands
{
    public static class WarSupportAttackHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
