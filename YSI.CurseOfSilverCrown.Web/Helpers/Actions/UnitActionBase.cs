using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Turns;
using YSI.CurseOfSilverCrown.Web.Database.Units;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Actions
{
    internal abstract class UnitActionBase : ActionBase
    {
        protected Unit Unit { get; set; }

        public UnitActionBase(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base(context, currentTurn)
        {
            Unit = context.Units.Find(unitId);
        }
    }
}
