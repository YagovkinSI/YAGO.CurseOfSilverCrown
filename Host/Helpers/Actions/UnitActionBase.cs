using YAGO.World.Host.Database;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Database.Units;

namespace YAGO.World.Host.Helpers.Actions
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
