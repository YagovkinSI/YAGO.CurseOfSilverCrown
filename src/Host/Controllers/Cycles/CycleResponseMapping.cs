using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Cycles
{
    public static class CycleResponseMapping
    {
        public static ApiResponse<MyCycle> ToMyDataResponse(this Cycle? source)
        {
            if (source == null)
                return ApiResponse<MyCycle>.CreateSuccess(data: null);

            var result = source.ToMyCycle();

            return ApiResponse<MyCycle>.CreateSuccess(data: result);
        }

        public static MyCycle ToMyCycle(this Cycle source)
        {
            return new MyCycle(
                source.Id,
                source.ColonyId,
                source.StepNumber,
                source.StartAtUtc,
                source.RunAtUtc,
                source.State);
        }
    }
}
