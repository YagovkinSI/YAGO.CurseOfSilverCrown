using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Quests;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(source.StatesJson)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            var colonyStats = GetColonyStats(source, colonyParameters);
            var colonyQuests = GetColonyQuests(colonyStats, colonyParameters);

            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                colonyStats,
                colonyQuests,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyStats = source.Stats;
            var colonyResources = colonyStats.Resources;
            var colonyParameters = GetColonyParameters(colonyStats, colonyResources, source.Quests);
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

        private static ColonyStats GetColonyStats(ColonyEntity source, ColonyParameters colonyParameter)
        {
            var colonySettings = new ColonySettings(
                colonyParameter.ShipId,
                colonyParameter.TaxLevel,
                colonyParameter.SocialGuaranteesLevel);
            var colonyResources = new ColonyResources(
                colonyParameter.ActionPoints,
                source.Solars,
                colonyParameter.Zones); var colonyIndustryList = new ColonyIndustryList(
                colonyParameter.AdministrativeIndustry.ToAdministrativeIndustry(),
                colonyParameter.MinningIndustry.ToMinningIndustry(),
                colonyParameter.ProductionIndustry.ToProductionIndustry(),
                colonyParameter.ServiceIndustry.ToServiceIndustry());
            var colonyStats = new ColonyStats(
                colonySettings,
                colonyResources,
                colonyIndustryList,
                colonyParameter.ActionPointsTrend,
                colonyParameter.MoodTotal,
                colonyParameter.CurrentWeek,
                colonyParameter.EpisodeCount,
                colonyParameter.FirstWedding);
            return colonyStats;
        }

        private static IReadOnlyList<ColonyQuest> GetColonyQuests(ColonyStats colonyStats, ColonyParameters colonyParameters)
        {
            return [.. colonyParameters.QuestIds.Select(x => new ColonyQuest(colonyStats, QuestDataset.Get(x)))];
        }

        private static ColonyParameters GetColonyParameters(
            ColonyStats colonyStats,
            ColonyResources colonyResources,
            IReadOnlyList<ColonyQuest> colonyQuests)
        {
            var colonySettings = colonyStats.Settings;
            var colonyIndustries = colonyStats.Industries;

            return new ColonyParameters(
                colonyResources.ActionPoints.Value,
                colonyStats.ActionPointsTrend,
                colonySettings.ShipId,
                colonySettings.TaxLevel,
                colonySettings.SocialGuaranteesLevel,
                colonyStats.MoodTotal.Value,
                colonyStats.FirstWedding,
                colonyStats.CurrentWeek,
                colonyStats.EpisodeCount,
                colonyResources.ZonesTotal,
                colonyIndustries.Administrative.ToEntity(),
                colonyIndustries.Minning.ToEntity(),
                colonyIndustries.Production.ToEntity(),
                colonyIndustries.Service.ToEntity(),
                [.. colonyQuests.Select(x => x.Id)]);
        }
    }
}
