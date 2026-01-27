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
        public ColonyWithDetails MyColony { get; }

        public RunCycleResult(
            Notification notification,
            Cycle myCycle,
            ColonyWithDetails myColony)
        {
            Notification = notification;
            MyCycle = myCycle;
            MyColony = myColony;
        }
    }
}
