using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleResult : IProcessorResult
    {
        public Episode? Episode { get; }
        public Colony? MyColony { get; }
        public Cycle? MyCycle { get; }

        public RunCycleResult(
            Episode? episode,
            Colony? myColony,
            Cycle? myCycle)
        {
            Episode = episode;
            MyColony = myColony;
            MyCycle = myCycle;
        }
    }
}
