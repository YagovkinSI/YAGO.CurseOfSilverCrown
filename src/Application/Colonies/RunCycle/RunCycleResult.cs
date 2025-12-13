using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleResult : IProcessorResult
    {
        public Cycle MyCycle { get; }
        public ColonyWithShipAndBuildings MyColony { get; }

        public RunCycleResult(
            Cycle myCycle,
            ColonyWithShipAndBuildings myColony)
        {
            MyCycle = myCycle;
            MyColony = myColony;
        }
    }
}
