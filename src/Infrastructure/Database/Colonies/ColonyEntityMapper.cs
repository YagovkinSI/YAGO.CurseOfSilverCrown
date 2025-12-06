using Newtonsoft.Json;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var buildingIds = JsonConvert.DeserializeObject<long[]>(source.BuildingIdsJson)
                ?? throw new YagoException("Не удалось десериализовать список построек из БД.");

            var states = JsonConvert.DeserializeObject<ColonyState[]>(source.StatesJson)
                ?? throw new YagoException("Не удалось десериализовать список состояний из БД.");

            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                buildingIds,
                source.ReputationByEvents,
                states);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var buildingIdsJson = JsonConvert.SerializeObject(source.BuildingIds);

            var statesJson = JsonConvert.SerializeObject(source.States);

            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                buildingIdsJson,
                source.ReputationByEvents,
                statesJson);
        }
    }
}
