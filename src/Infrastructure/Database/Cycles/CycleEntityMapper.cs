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
                source.StartAtUtc,
                source.RunAtUtc,
                source.StepNumber,
                source.IsComplited);
        }

        public static CycleEntity ToEntity(this Cycle source)
        {
            return new CycleEntity(
                source.Id,
                source.ColonyId,
                source.StartAtUtc,
                source.RunAtUtc,
                source.StepNumber,
                source.IsComplited,
                "[]");
        }
    }
}
