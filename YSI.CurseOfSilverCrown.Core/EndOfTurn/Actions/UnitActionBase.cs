using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.MainModels;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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
