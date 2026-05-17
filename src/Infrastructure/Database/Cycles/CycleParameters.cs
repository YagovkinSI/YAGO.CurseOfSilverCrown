using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleParameters
    {
        public string? ActiveEventId { get; private set; }
        public CycleResult PreviousCycleResult { get; }

        public CycleParameters(
            string? activeEventId,
            CycleResult previousCycleResult)
        {
            ActiveEventId = activeEventId;
            PreviousCycleResult = previousCycleResult;
        }
    }
}
