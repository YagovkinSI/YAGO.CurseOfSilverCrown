using Newtonsoft.Json;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameter = JsonConvert.DeserializeObject<ColonyParameters>(source.StatesJson)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                colonyParameter.StartGavernorType,
                colonyParameter.Contracts);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyParameters = new ColonyParameters(source.CodeOfLaws, source.Contracts);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);

            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                "[]",
                statesJson);
        }
    }
}
