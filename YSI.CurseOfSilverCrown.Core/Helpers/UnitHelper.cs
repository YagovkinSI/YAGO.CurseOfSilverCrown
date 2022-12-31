using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

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
    }
}
