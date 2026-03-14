using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Application.Cycles
{
    public interface ICycleProvider : IProvider<GetCycleCommand, Cycle?>;
}
