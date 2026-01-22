using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.ConcludeContract
{
    public class ConcludeContractCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long СontractId { get; }

        public ConcludeContractCommand(long userId, long contractId)
        {
            UserId = userId;
            СontractId = contractId;
        }
    }
}
