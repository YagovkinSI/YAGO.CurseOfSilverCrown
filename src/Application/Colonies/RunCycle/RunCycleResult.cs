using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Notifications;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleResult : IProcessorResult
    {
        public Notification Notification { get; }
        public Cycle MyCycle { get; }
        public ColonyWithShipAndBuildings MyColony { get; }

        public RunCycleResult(
            Notification notification,
            Cycle myCycle,
            ColonyWithShipAndBuildings myColony)
        {
            Notification = notification;
            MyCycle = myCycle;
            MyColony = myColony;
        }
    }
}
