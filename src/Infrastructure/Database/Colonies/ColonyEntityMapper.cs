using Newtonsoft.Json;
using YAGO.World.Domain.Entities.Colonies;
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
                colonyParameter.ShipId,
                source.Name,
                colonyParameter.StartGavernorType,
                source.Solars,
                colonyParameter.FestivalEffect,
                colonyParameter.Companies,
                colonyParameter.CurrentWeek,
                colonyParameter.FirstWedding,
                source.Deactivated,
                source.DeactivateAtUtc,
                colonyParameter.Maintenance,
                colonyParameter.Zones);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyParameters = new ColonyParameters(
                source.ShipId,
                source.CodeOfLaws,
                source.CompanyIds,
                source.FestivalEffect,
                source.FirstWedding,
                source.CurrentWeek,
                source.Maintenance,
                source.Zones);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);

            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                statesJson,
                source.Deactivated,
                source.DeactivateAtUtc);
        }
    }
}
