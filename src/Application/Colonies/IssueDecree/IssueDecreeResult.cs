using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.IssueDecree
{
    public class IssueDecreeResult : IProcessorResult
    {
        public ColonyWithDetails MyColony { get; }

        public IssueDecreeResult(ColonyWithDetails myColony)
        {
            MyColony = myColony;
        }
    }
}
