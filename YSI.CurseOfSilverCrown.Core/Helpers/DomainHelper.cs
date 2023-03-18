using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
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
                .Where(u => u.DomainId == domainId && u.InitiatorPersonId == domain.PersonId)
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
                    InitiatorPersonId = domain.PersonId,
                    Status = CommandStatus.ReadyToMove
                };
                context.Add(unit);
            }

            if (currentWarriorsCount > newWarriorCount)
            {
                var percentSave = (double)newWarriorCount / currentWarriorsCount;
                var units = context.Units
                    .Where(u => u.DomainId == domainId && u.InitiatorPersonId == domain.PersonId);
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
    }
}
