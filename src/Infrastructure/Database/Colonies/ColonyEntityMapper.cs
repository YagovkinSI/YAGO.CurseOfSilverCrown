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
                colonyParameter.ShipId,
                colonyParameter.StartGavernorType,
                source.Solars,
                colonyParameter.FestivalEffect,
                colonyParameter.Companies,
                colonyParameter.CurrentWeek,
                colonyParameter.FirstWedding,
                colonyParameter.Episodes ?? []);

            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                colonyStats,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyStats = source.Stats;
            var colonyParameters = new ColonyParameters(
                colonyStats.ShipId,
                colonyStats.CodeOfLaws,
                colonyStats.CompanyIds,
                colonyStats.FestivalEffect,
                colonyStats.FirstWedding,
                colonyStats.CurrentWeek,
                colonyStats.Episodes);
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
