using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Cycles;

namespace YAGO.World.Host.Controllers.ColonyActions
{
    public class UpdatedColonyEntities
    {
        public MyCycle? MyCycle { get; }
        public MyColony? MyColony { get; }
        public ColonyDetails[] OtherColonies { get; }

        public UpdatedColonyEntities(
            MyCycle? myCycle = null,
            MyColony? myColony = null,
            ColonyDetails[]? otherColonies = null)
        {
            MyCycle = myCycle;
            MyColony = myColony;
            OtherColonies = otherColonies ?? new ColonyDetails[0];
        }
    }
}
