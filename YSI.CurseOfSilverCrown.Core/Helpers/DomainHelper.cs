using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class DomainHelper
    {
        public static int GetWarriorCount(ApplicationDbContext context, int domainId)
        {
            var domain = context.Domains.Find(domainId);
            return context.Units
                .Where(u => u.DomainId == domainId)
                .Sum(u => u.Warriors);
        }

        public static void SetWarriorCount(ApplicationDbContext context, int domainId, int newWarriorCount)
        {
            var currentWarriorsCount = GetWarriorCount(context, domainId);
            if (currentWarriorsCount == newWarriorCount)
                return;
            var domain = context.Domains.Find(domainId);

            if (currentWarriorsCount < newWarriorCount)
            {
                var unit = new Unit
                {
                    DomainId = domainId,
                    PositionDomainId = domainId,
                    Warriors = newWarriorCount - currentWarriorsCount,
                    Type = UnitCommandType.WarSupportDefense,
                    TargetDomainId = domainId,
                    Status = CommandStatus.ReadyToMove
                };
                context.Add(unit);
            }

            if (currentWarriorsCount > newWarriorCount)
            {
                var percentSave = (double)newWarriorCount / currentWarriorsCount;
                var units = context.Units
                    .Where(u => u.DomainId == domainId);
                foreach (var unit in units)
                {
                    unit.Warriors = (int)Math.Round(unit.Warriors * percentSave);
                    if (unit.Warriors <= 0)
                    {
                        unit.Warriors = 0;
                        unit.Status = CommandStatus.Destroyed;
                    }
                    context.Update(unit);
                }
            }
        }

        public static int GetImprotanceDoamin(ApplicationDbContext context, int domainId)
        {
            var domain = context.Domains.Find(domainId);
            var mainTax = Constants.BaseVassalTax * InvestmentsHelper.GetInvestmentTax(domain.Investments);
            var vassalTaxes = Constants.BaseVassalTax * domain.Vassals.Count *
                InvestmentsHelper.GetInvestmentTax(InvestmentsHelper.StartInvestment * 3);

            var mapImprtance = KingdomHelper.GetAllLevelVassalIds(context.Domains, domainId).Count * 500;

            var importance = 10 * (mainTax + vassalTaxes) + mapImprtance;
            return (int)importance;
        }

        public static DomainPublic GetDomainPublic(ApplicationDbContext context, int domainId)
        {
            var allDomains = context.Domains.ToList();
            var domain = context.Domains.Find(domainId);
            var domainInfoText = KingdomHelper.GetDomainInfoText(context, allDomains, domain);

            return new DomainPublic
            {
                Id = domainId,
                Name = domain.Name,
                Info = domainInfoText
            };
        }

        public static async Task<List<DomainPublic2>> GetDomainPublic2OrderByColumn(ApplicationDbContext context, int? column)
        {
            var domains = await GetDomainsOrderByColumn(context, column);
            var domainPublic2List = domains
                .Select(d => context.Domains.GetDomainPublic2(d.Id))
                .ToList();
            return domainPublic2List;
        }

        private static DomainPublic2 GetDomainPublic2(this DbSet<Domain> domains, int domainId)
        {
            var domain = domains.Find(domainId);
            return new DomainPublic2
            {
                Id = domain.Id,
                Name = domain.Name,
                Warriors = domain.WarriorCount,
                Gold = domain.Gold,
                Investments = domain.Investments,
                Fortifications = FortificationsHelper.GetFortCoef(domain.Fortifications),
                Suzerain = domain.Suzerain == null
                    ? null
                    : new DomainLink { Id = domain.Suzerain.Id, Name = domain.Suzerain.Name },
                VassalCount = domain.Vassals.Count,
                User = domain.User == null
                    ? null
                    : new UserLink { Id = domain.User.Id, Name = domain.User.UserName },
            };
        }

        public static async Task<List<Domain>> GetDomainsOrderByColumn(ApplicationDbContext context, int? column)
        {
            var domains = context.Domains;
            IOrderedQueryable<Domain> orderedDomains = null;
            switch (column)
            {
                case 1:
                    orderedDomains = domains.OrderBy(o => o.Name);
                    break;
                case 2:
                    return domains
                        .ToList()
                        .OrderByDescending(o => o.WarriorCount)
                        .ToList();
                case 3:
                    orderedDomains = domains.OrderByDescending(o => o.Gold);
                    break;
                case 4:
                    orderedDomains = domains.OrderByDescending(o => o.Investments);
                    break;
                case 5:
                    orderedDomains = domains.OrderByDescending(o => o.Fortifications);
                    break;
                case 6:
                    orderedDomains = domains.OrderBy(o => o.Suzerain == null ? "" : o.Suzerain.Name);
                    break;
                case 8:
                    orderedDomains = domains.OrderBy(o => o.User == null ? "" : o.User.UserName);
                    break;
                case 7:
                default:
                    orderedDomains = domains.OrderByDescending(o => o.Vassals.Count);
                    break;
            }
            return await orderedDomains.ToListAsync();
        }
    }
}
