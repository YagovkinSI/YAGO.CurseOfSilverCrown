using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.ConcludeСontract
{
    public class ConcludeСontractCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long СontractId { get; }

        public ConcludeСontractCommand(long userId, long contractId)
        {
            UserId = userId;
            СontractId = contractId;
        }
    }
}
