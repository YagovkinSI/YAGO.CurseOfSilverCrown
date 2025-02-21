using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Turns;
using YAGO.World.Infrastructure.Database.Models.Units;

namespace YAGO.World.Infrastructure.Helpers.Actions
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
