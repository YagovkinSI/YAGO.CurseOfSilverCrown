using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.IssueDecree
{
    public class IssueDecreeResult : IProcessorResult
    {
        public Colony MyColony { get; }

        public IssueDecreeResult(Colony myColony)
        {
            MyColony = myColony;
        }
    }
}
