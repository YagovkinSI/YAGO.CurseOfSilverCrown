using YAGO.World.Domain.Cycles;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Cycles
{
    public static class CycleResponseMapping
    {
        public static MyDataResponse<MyCycle> ToMyDataResponse(this Cycle? source)
        {
            if (source == null)
                return new MyDataResponse<MyCycle>(IsAuthorized: true, Data: null);

            var result = new MyCycle(
                source.Id,
                source.ColonyId,
                source.CompletedUtc);

            return new MyDataResponse<MyCycle>(
                IsAuthorized: true,
                result);
        }
    }
}
