using YAGO.World.Host.Controllers.Colonies;
using YAGO.World.Host.Controllers.Cycles;

namespace YAGO.World.Host.Controllers.Common
{
    public class UpdatedEntities
    {
        public MyCycle? MyCycle { get; }
        public MyColony? MyColony { get; }
        public ColonyDetails[] OtherColonies { get; }

        public UpdatedEntities(
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
