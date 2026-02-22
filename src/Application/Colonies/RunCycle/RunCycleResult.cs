using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Episodes;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleResult : IProcessorResult
    {
        public Episode? Episode { get; }
        public ColonyWithDetails MyColony { get; }
        public Cycle? MyCycle { get; }

        public RunCycleResult(
            Episode? episode,
            ColonyWithDetails myColony,
            Cycle? myCycle)
        {
            Episode = episode;
            MyColony = myColony;
            MyCycle = myCycle;
        }
    }
}
