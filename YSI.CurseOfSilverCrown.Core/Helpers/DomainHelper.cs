using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class DomainHelper
    {
        public static int GetWarriorCount(ApplicationDbContext context, int domainId)
        {
            return context.Units
                .Where(u => u.DomainId == domainId && u.InitiatorDomainId == domainId)
                .Sum(u => u.Warriors);
        }

        public static void SetWarriorCount(ApplicationDbContext context, int domainId, int newWarriorCount)
        {
            var currentWarriorsCount = GetWarriorCount(context, domainId);
            if (currentWarriorsCount == newWarriorCount)
                return;

            if (currentWarriorsCount < newWarriorCount)
            {
                var unit = new Unit
                {
                    DomainId = domainId,
                    PositionDomainId = domainId,
                    Warriors = newWarriorCount - currentWarriorsCount,
                    Type = enArmyCommandType.WarSupportDefense,
                    TargetDomainId = domainId,
                    InitiatorDomainId = domainId,
                    Status = enCommandStatus.ReadyToMove,
                    ActionPoints = WarConstants.ActionPointsFullCount
                };
                context.Add(unit);
            }

            if (currentWarriorsCount > newWarriorCount)
            {
                var percentSave = (double)newWarriorCount / currentWarriorsCount;
                var units = context.Units
                    .Where(u => u.DomainId == domainId && u.InitiatorDomainId == domainId);
                foreach (var unit in units)
                {
                    unit.Warriors = (int)Math.Round(unit.Warriors * percentSave);
                    if (unit.Warriors <= 0)
                    {
                        unit.Warriors = 0;
                        unit.Status = enCommandStatus.Destroyed;
                    }
                    context.Update(unit);
                }
            }
        }
    }
}
