using Newtonsoft.Json;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Turns;

namespace YAGO.World.Infrastructure.Database.Turns
{
    internal static class TurnEntityMapper
    {
        public static Turn ToDomain(this TurnEntity source)
        {
            var turnParameters = JsonConvert.DeserializeObject<TurnParameters>(source.JsonData)
                    ?? throw new YagoException("Не удалось десериализовать параметры хода из БД.");

            return new Turn(
                source.Id,
                source.ColonyId,
                source.StartAtUtc,
                source.RunAtUtc,
                source.IsComplited);
        }

        public static TurnEntity ToEntity(this Turn source)
        {
            var turnParameters = new TurnParameters();
            var statesJson = JsonConvert.SerializeObject(turnParameters);
            return new TurnEntity(
                source.Id,
                source.ColonyId,
                source.StartAtUtc,
                source.RunAtUtc,
                source.IsComplited,
                statesJson);
        }
    }
}
