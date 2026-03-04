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

            var colonyStats = new ColonyStats(
                source.Solars,
                colonyParameter.FestivalEffect,
                colonyParameter.Companies,
                colonyParameter.CurrentWeek);

            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                colonyStats,
                colonyParameter.FirstWedding,
                colonyParameter.ShipId,
                colonyParameter.StartGavernorType,
                source.Deactivated,
                source.DeactivateAtUtc,
                colonyParameter.Episodes ?? []);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyStats = source.Stats;
            var colonyParameters = new ColonyParameters(
                source.ShipId,
                source.CodeOfLaws,
                colonyStats.CompanyIds,
                colonyStats.FestivalEffect,
                source.FirstWedding,
                colonyStats.CurrentWeek,
                source.Episodes);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);

            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                colonyStats.Solars,
                statesJson,
                source.Deactivated,
                source.DeactivateAtUtc);
        }
    }
}
