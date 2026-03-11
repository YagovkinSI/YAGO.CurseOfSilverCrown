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
            var policies = new ColonyPolicies(
                colonyParameter.ShipId,
                colonyParameter.StartGavernorType);
            var colonyFlags = new ColonyFlags(
                colonyParameter.FirstWedding,
                colonyParameter.Episodes ?? []);

            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                policies,
                colonyStats,
                colonyFlags,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyStats = source.Stats;
            var policies = source.Policies;
            var colonyFlags = source.Flags;
            var colonyParameters = new ColonyParameters(
                policies.ShipId,
                policies.CodeOfLaws,
                colonyStats.CompanyIds,
                colonyStats.FestivalEffect,
                colonyFlags.FirstWedding,
                colonyStats.CurrentWeek,
                colonyFlags.Episodes);
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
