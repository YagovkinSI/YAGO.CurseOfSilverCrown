using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.MainModels.Units;

namespace YSI.CurseOfSilverCrown.Core.Actions
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
