using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Database.Units;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
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
