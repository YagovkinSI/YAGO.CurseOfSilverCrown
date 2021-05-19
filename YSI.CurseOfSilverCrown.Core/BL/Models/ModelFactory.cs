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

        public static async Task<IEnumerable<DomainMain>> GetAllDomainMain(this ApplicationDbContext context)
        {
            var domains = await context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .ToListAsync();
            return domains.Select(d => new DomainMain(d));
        }

        public static async Task<IEnumerable<DomainMin>> GetAllDomainMin(this ApplicationDbContext context)
        {
            var domains = await context.Domains
                .ToListAsync();
            return domains.Select(d => new DomainMin(d));
        }

        public static async Task<UnitMin> GetUnitMin(this ApplicationDbContext context, int id)
        {
            var unit = await context.Units
                .FindAsync(id);
            return new UnitMin(unit);
        }

        public static async Task<UnitMain> GetUnitMain(this ApplicationDbContext context, int id)
        {
            var unit = await context.Units
                .Include(d => d.Domain)
                .Include(d => d.Target)
                .Include(d => d.Target2)
                .Include(d => d.Position)
                .Include(d => d.Initiator)
                .SingleAsync(d => d.Id == id);
            return new UnitMain(unit);
        }

        public static async Task<IEnumerable<UnitMain>> GetUnitsMainAsync(this ApplicationDbContext context, int domainId, int initiatorId)
        {
            var units = await context.Units
                .Include(d => d.Domain)
                .Include(d => d.Target)
                .Include(d => d.Target2)
                .Include(d => d.Position)
                .Include(d => d.Initiator)
                .Where(d => d.DomainId == domainId && d.InitiatorDomainId == initiatorId)
                .ToListAsync();
            return units.Select(u => new UnitMain(u));
        }

        public static async Task<IEnumerable<UnitMain>> GetUnitsMainIn(this ApplicationDbContext context, int positionId)
        {
            var units = await context.Units
                .Include(d => d.Domain)
                .Include(d => d.Target)
                .Include(d => d.Target2)
                .Include(d => d.Position)
                .Include(d => d.Initiator)
                .Where(d => d.PositionDomainId == positionId && d.InitiatorDomainId == d.DomainId)
                .ToListAsync();
            return units.Select(u => new UnitMain(u));
        }
    }
}
