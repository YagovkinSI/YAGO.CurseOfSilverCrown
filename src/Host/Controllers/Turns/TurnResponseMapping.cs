using YAGO.World.Domain.Turns;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Turns
{
    public static class TurnResponseMapping
    {
        public static ApiResponse<MyTurn> ToMyDataResponse(this Turn? source)
        {
            if (source == null)
                return ApiResponse<MyTurn>.CreateSuccess(data: null);

            var result = source.ToMyTurn();

            return ApiResponse<MyTurn>.CreateSuccess(data: result);
        }

        public static MyTurn ToMyTurn(
            this Turn source)
        {
            return new MyTurn(
                source.Id,
                source.ColonyId,
                source.StartAtUtc,
                source.RunAtUtc);
        }
    }
}
