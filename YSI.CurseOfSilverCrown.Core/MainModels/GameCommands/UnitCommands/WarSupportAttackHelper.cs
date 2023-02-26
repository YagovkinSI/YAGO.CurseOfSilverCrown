using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.Core.MainModels.GameCommands.UnitCommands
{
    public static class WarSupportAttackHelper
    {
        public static async Task<IEnumerable<Domain>> GetAvailableTargets2(ApplicationDbContext context)
        {
            return await context.Domains.ToListAsync();
        }
    }
}
