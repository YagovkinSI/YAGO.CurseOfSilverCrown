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

            var colonyIndustryList = new ColonyIndustryList(
                colonyParameter.MinningIndustry.ToDomain(),
                colonyParameter.ProductionIndustry.ToDomain(),
                colonyParameter.ServiceIndustry.ToDomain());

            var colonyStats = new ColonyStats(
                colonyParameter.StartGavernorType,
                source.Solars,
                colonyParameter.FestivalEffect,
                colonyParameter.CurrentWeek,
                colonyParameter.FirstWedding,
                colonyParameter.Maintenance,
                colonyParameter.Zones,
                colonyIndustryList);

            return new Colony(
                source.Id,
                source.UserId,
                colonyParameter.ShipId,
                source.Name,
                colonyStats,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyStats = source.Stats;

            var colonyParameters = new ColonyParameters(
                source.ShipId,
                colonyStats.CodeOfLaws,
                [],
                colonyStats.FestivalEffect,
                colonyStats.FirstWedding,
                colonyStats.CurrentWeek,
                colonyStats.Maintenance,
                colonyStats.ZonesTotal,
                colonyStats.Industries.Minning.ToEntity(),
                colonyStats.Industries.Production.ToEntity(),
                colonyStats.Industries.Service.ToEntity());
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
