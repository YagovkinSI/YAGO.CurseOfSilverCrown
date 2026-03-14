using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    internal static class CycleEntityMapper
    {
        public static Cycle ToDomain(this CycleEntity source)
        {
            return new Cycle(
                source.Id,
                source.ColonyId,
                source.StepNumber,
                source.RunAtUtc,
                source.State);
        }

        public static CycleEntity ToEntity(this Cycle source)
        {
            return new CycleEntity(
                source.Id,
                source.ColonyId,
                source.StepNumber,
                source.RunAtUtc,
                source.State);
        }
    }
}
