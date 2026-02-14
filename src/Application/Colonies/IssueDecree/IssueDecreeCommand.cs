using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.IssueDecree
{
    public class IssueDecreeCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long DecreeId { get; }

        public IssueDecreeCommand(long userId, long contractId)
        {
            UserId = userId;
            DecreeId = contractId;
        }
    }
}
