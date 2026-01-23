using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.DeactivateColony
{
    public interface IDeactivateColonyProcessor : IProcessor<DeactivateColonyCommand, DeactivateColonyResult>
    {
    }
}
