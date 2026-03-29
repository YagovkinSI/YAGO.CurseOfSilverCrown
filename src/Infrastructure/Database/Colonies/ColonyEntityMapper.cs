using Newtonsoft.Json;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameter = JsonConvert.DeserializeObject<ColonyParameters>(source.StatesJson)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            var colonySettings = new ColonySettings(
                colonyParameter.ShipId,
                colonyParameter.StartGavernorType);
            var colonyResources = new ColonyResources(
                source.Solars,
                colonyParameter.Zones);
            var colonyIndicators = new ColonyIndicators(
                colonyParameter.FestivalEffect,
                colonyParameter.CurrentWeek,
                colonyParameter.FirstWedding);

            var colonyIndustryList = new ColonyIndustryList(
                colonyParameter.AdministrativeIndustry.ToDomain() as AdministrativeIndustry,
                colonyParameter.MinningIndustry.ToDomain() as MinningIndustry,
                colonyParameter.ProductionIndustry.ToDomain() as ProductionIndustry,
                colonyParameter.ServiceIndustry.ToDomain() as ServiceIndustry);

            var colonyStats = new ColonyStats(
                colonySettings,
                colonyResources,
                colonyIndicators,
                colonyIndustryList);

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
            var colonySettings = colonyStats.Settings;
            var colonyResources = colonyStats.Resources;
            var colonyIndicators = colonyStats.Indicators;
            var colonyIndustries = colonyStats.Industries;

            var colonyParameters = new ColonyParameters(
                colonySettings.ShipId,
                colonySettings.CodeOfLaws,
                colonyIndicators.FestivalEffect,
                colonyIndicators.FirstWedding,
                colonyIndicators.CurrentWeek,
                colonyResources.ZonesTotal,
                colonyIndustries.Administrative.ToEntity(),
                colonyIndustries.Minning.ToEntity(),
                colonyIndustries.Production.ToEntity(),
                colonyIndustries.Service.ToEntity());
            var statesJson = JsonConvert.SerializeObject(colonyParameters);

            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                colonyResources.Solars,
                statesJson,
                source.Deactivated,
                source.DeactivateAtUtc);
        }
    }
}
