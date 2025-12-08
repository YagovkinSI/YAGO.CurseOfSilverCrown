using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Cycles;

namespace YAGO.World.Host.Controllers.ColonyActions
{
    public class UpdatedColonyEntities
    {
        public MyCycle? MyCycle { get; }

        public MyColony? MyColony { get; }
        public UpdatedColonyEntities(
            MyCycle? myCycle = null,
            MyColony? myColony = null)
        {
            MyCycle = myCycle;
            MyColony = myColony;
        }
    }
}
