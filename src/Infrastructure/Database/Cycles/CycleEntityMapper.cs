using Newtonsoft.Json;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    internal static class CycleEntityMapper
    {
        public static Cycle ToDomain(this CycleEntity source)
        {
            var cycleParameters = JsonConvert.DeserializeObject<CycleParameters>(source.Parameters)
                ?? throw new YagoException("Не удалось десериализовать параметры хода из БД.");

            return new Cycle(
                source.Id,
                source.ColonyId,
                source.StartAtUtc,
                source.RunAtUtc,
                cycleParameters.ActiveEventId,
                source.StepNumber,
                source.IsComplited,
                cycleParameters.PreviousCycleResult);
        }

        public static CycleEntity ToEntity(this Cycle source)
        {
            var cycleParameters = new CycleParameters(
                source.ActiveEventId,
                source.PreviousCycleResult);
            var statesJson = JsonConvert.SerializeObject(cycleParameters);
            return new CycleEntity(
                source.Id,
                source.ColonyId,
                source.StartAtUtc,
                source.RunAtUtc,
                source.StepNumber,
                source.IsComplited,
                statesJson);
        }
    }
}
