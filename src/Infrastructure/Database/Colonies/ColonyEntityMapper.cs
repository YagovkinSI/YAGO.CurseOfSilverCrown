using Newtonsoft.Json;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var buildingIds = JsonConvert.DeserializeObject<long[]>(source.BuildingIdsJson);
            return buildingIds == null
                ? throw new YagoException("Не удалось десериализовать список построек из БД.")
                : new Colony(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                buildingIds
                );
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var buildingIdsJson = JsonConvert.SerializeObject(source.BuildingIds);

            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                buildingIdsJson);
        }
    }
}
