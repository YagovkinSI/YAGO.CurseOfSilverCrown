using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class UnitHelper
    {
        public static async Task<bool> TryUnion(this Unit unitTo, Unit unitFrom, ApplicationDbContext context)
        {
            if (unitTo.PositionDomainId != unitFrom.PositionDomainId ||
                unitTo.DomainId != unitFrom.DomainId)
            {
                return false;
            }

            unitTo.Warriors += unitFrom.Warriors;
            unitTo.Coffers += unitFrom.Coffers;
            context.Remove(unitFrom);
            context.Update(unitTo);
            await context.SaveChangesAsync();
            return true;
        }

        public static async Task<(bool, Unit)> TrySeparate(this Unit unit, int separateCount, ApplicationDbContext context)
        {
            if (unit == null || unit.Warriors <= separateCount)
                return (false, null);

            var newUnit = new Unit
            {
                Warriors = separateCount,
                Coffers = 0,
                InitiatorPersonId = unit.InitiatorPersonId,
                DomainId = unit.DomainId,
                PositionDomainId = unit.PositionDomainId,
                Status = unit.Status,
                Type = enArmyCommandType.WarSupportDefense,
                TypeInt = (int)enArmyCommandType.WarSupportDefense,
                TargetDomainId = unit.DomainId,
                ActionPoints = WarConstants.ActionPointsFullCount
            };
            unit.Warriors -= separateCount;

            context.Update(unit);
            context.Add(newUnit);
            await context.SaveChangesAsync();
            return (true, newUnit);
        }

        public static async Task<bool> TryWar(this Unit unit, int targetDomainId, ApplicationDbContext context)
        {
            unit.TargetDomainId = targetDomainId;
            unit.Type = enArmyCommandType.War;
            context.Update(unit);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
