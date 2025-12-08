using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies.AttackColony
{
    public class AttackColonyCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long TargetColonyId { get; }
        public AttackColonyPrizeType PrizeType { get; }

        public AttackColonyCommand(
            long userId, 
            long targetColonyId, 
            AttackColonyPrizeType prizeType)
        {
            UserId = userId;
            TargetColonyId = targetColonyId;
            PrizeType = prizeType;
        }
    }
}
