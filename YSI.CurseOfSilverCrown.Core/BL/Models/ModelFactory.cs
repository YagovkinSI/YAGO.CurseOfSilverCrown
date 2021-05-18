using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.Core.BL.Models
{
    public static class ModelFactory
    {
        public static async Task<DomainMin> GetDomainMin(this ApplicationDbContext context, int id)
        {
            var domain = await context.Domains
                .FindAsync(id);
            return new DomainMin(domain);
        }

        public static async Task<DomainMain> GetDomainMain(this ApplicationDbContext context, int id)
        {
            var domain = await context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .SingleAsync(d => d.Id == id);
            return new DomainMain(domain);
        }

        public static IQueryable<DomainMain> GetAllDomainMain(this ApplicationDbContext context)
        {
            var domains = context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals);
            return domains.Select(d => new DomainMain(d));
        }
    }
}
