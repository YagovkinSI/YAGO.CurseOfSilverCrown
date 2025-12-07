using YAGO.World.Host.Controllers.Cycles;

namespace YAGO.World.Host.Controllers.ColonyActions
{
    public class UpdatedColonyEntities
    {
        public MyCycle? MyCycle { get; }

        public UpdatedColonyEntities(
            MyCycle? myCycle = null)
        {
            MyCycle = myCycle;
        }
    }
}
