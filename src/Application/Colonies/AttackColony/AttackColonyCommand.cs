using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.AttackColony
{
    public class AttackColonyCommand : IProcessorCommand
    {
        public long UserId { get; }
        public long TargetColonyId { get; }

        public AttackColonyCommand(
            long userId,
            long targetColonyId)
        {
            UserId = userId;
            TargetColonyId = targetColonyId;
        }
    }
}
