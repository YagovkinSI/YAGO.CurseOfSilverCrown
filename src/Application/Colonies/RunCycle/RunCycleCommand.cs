using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleCommand : IProcessorCommand
    {
        public long UserId { get; }

        public RunCycleCommand(long userId)
        {
            UserId = userId;
        }
    }
}
