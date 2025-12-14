using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.AttackColony
{
    public interface IAttackColonyProcessor : IProcessor<AttackColonyCommand, AttackColonyResult>
    {
    }
}
