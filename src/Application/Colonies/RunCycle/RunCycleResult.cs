using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleResult : IProcessorResult
    {
        public Cycle Cycle { get; }

        public RunCycleResult(Cycle cycle)
        {
            Cycle = cycle;
        }
    }
}
