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
            var colonyIndicators = GetColonyIndicators(colonyParameter);
            var colonyStats = new ColonyStats(
                colonySettings,
                colonyResources,
                colonyIndicators);
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
            var colonyResources = colonyStats.Resources;
            var colonyParameters = GetColonyParameters(colonyStats, colonyResources);
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

        private static ColonyIndicators GetColonyIndicators(ColonyParameters colonyParameter)
        {
            //TODO: Переделать?
            var colonyIndustryList = new ColonyIndustryList(
                colonyParameter.AdministrativeIndustry.ToDomain() as AdministrativeIndustry,
                colonyParameter.MinningIndustry.ToDomain() as MinningIndustry,
                colonyParameter.ProductionIndustry.ToDomain() as ProductionIndustry,
                colonyParameter.ServiceIndustry.ToDomain() as ServiceIndustry);
            return new ColonyIndicators(
                colonyIndustryList,
                colonyParameter.FestivalEffect,
                colonyParameter.CurrentWeek,
                colonyParameter.FirstWedding);
        }

        private static ColonyParameters GetColonyParameters(ColonyStats colonyStats, ColonyResources colonyResources)
        {
            var colonySettings = colonyStats.Settings;
            var colonyIndicators = colonyStats.Indicators;
            var colonyIndustries = colonyIndicators.Industries;

            return new ColonyParameters(
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
        }
    }
}
