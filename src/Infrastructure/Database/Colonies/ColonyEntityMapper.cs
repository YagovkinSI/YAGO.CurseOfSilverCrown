using Newtonsoft.Json;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes.States;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(source.StatesJson)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            var colonyStats = GetColonyStats(colonyParameters);
            var colonyName = new ColonyName(source.Name, colonyParameters.Named);

            return new Colony(
                source.Id,
                source.UserId,
                colonyName,
                colonyStats,
                colonyParameters.EventIds,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyName = source.Name;
            var colonyStats = source.State;
            var colonySolars = new ColonySolarsEntity(
                colonyStats.GetValue(StateKey.SolarsCurrent),
                colonyStats.GetValue(StateKey.SolarsDelta));
            var colonyReformPoints = new ColonyReformPointsEntity(
                colonyStats.GetValue(StateKey.ReformPointsCurrent),
                colonyStats.GetValue(StateKey.ReformPointsDelta));
            var colonyModules = new ColonyModulesEntity(
                colonyStats.GetValue(StateKey.ModulesTotal),
                colonyStats.GetValue(StateKey.ModulesUsed));
            var colonyMood = new ColonyMoodEntity(
                colonyStats.GetValue(StateKey.MoodCurrent));
            var colonyReforms = new ColonyReformsEntity(
                colonyStats.GetValue(StateKey.ReformsTaxLevel),
                colonyStats.GetValue(StateKey.ReformsSocialGuaranteesLevel));
            var colonyAdminostrative = new ColonyBuildingsEntity(
                colonyStats.GetValue(StateKey.BuildingsAdministrativeState),
                colonyStats.GetValue(StateKey.BuildingsAdministrativePrivate));
            var colonyMining = new ColonyBuildingsEntity(
                colonyStats.GetValue(StateKey.BuildingsMiningState),
                colonyStats.GetValue(StateKey.BuildingsMiningPrivate));
            var colonyService = new ColonyBuildingsEntity(
                colonyStats.GetValue(StateKey.BuildingsServiceState),
                colonyStats.GetValue(StateKey.BuildingsServicePrivate));
            var colonyProduction = new ColonyBuildingsEntity(
                colonyStats.GetValue(StateKey.BuildingsProductionState),
                colonyStats.GetValue(StateKey.BuildingsProductionPrivate));
            var colonyIndustry = new ColonyIndustryEntity(
                colonyAdminostrative,
                colonyMining,
                colonyProduction,
                colonyService);
            var colonyFlags = new ColonyFlagsEntity(
                colonyStats.GetValue(StateKey.FlagsFirstWedding));
            var colonyCounters = new ColonyCountersEntity(
                colonyStats.GetValue(StateKey.TurnsCurrent));
            var colonyStatsEntity = new ColonyStatsEntity(
                colonySolars,
                colonyReformPoints,
                colonyModules,
                colonyMood,
                colonyReforms,
                colonyIndustry,
                colonyFlags,
                colonyCounters);
            var colonyParameters = new ColonyParameters(
                colonyName.Named,
                colonyStatsEntity,
                source.EventIds);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);
            return new ColonyEntity(
                source.Id,
                source.UserId,
                colonyName.DatabaseName,
                solars: colonyStats.GetValue(StateKey.SolarsCurrent),
                statesJson,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        private static ColonyState GetColonyStats(ColonyParameters colonyParameter)
        {
            var states = colonyParameter.States;
            var resources = new List<ColonyResource>
            {
                new ColonySolars(states.Solars.Reserve),
                new ColonyReformPoints(states.ReformPoints.Reserve),
                new ColonyMood(states.Mood.Reserve),
                new ColonyTurns((int)states.Counters.Turns),
            };
            var slots = new List<ColonySlot>
            {
                new ColonyModules(total: (int)states.Modules.Total),
                new ColonyMiningSlots(total: 12),
            };
            var reforms = new List<ColonyReform>
            {
                new ColonyReform(ColonyReformType.TaxLevel, states.Reforms.TaxLevel),
                new ColonyReform(ColonyReformType.SocialGuaranteesLevel, states.Reforms.SocialGuaranteesLevel),
            };
            var industries = new List<ColonyIndustry>
            {
                new ColonyIndustry(IndustryType.Administrative,
                    (int)states.Industries.Administrative.Private, 
                    (int)states.Industries.Administrative.State),
                new ColonyIndustry(IndustryType.Mining,
                    (int)states.Industries.Mining.Private,
                    (int)states.Industries.Mining.State),
                new ColonyIndustry(IndustryType.Production,
                    (int)states.Industries.Production.Private,
                    (int)states.Industries.Production.State),
                new ColonyIndustry(IndustryType.Service,
                    (int)states.Industries.Service.Private,
                    (int)states.Industries.Service.State),
            };
            var progress = new Dictionary<ColonyProgressType, bool>()
            {
                { ColonyProgressType.FirstWedding, states.Flags.FirstWedding > 0.5 }
            };
            var colonyStats = new ColonyState(resources, slots, reforms, industries, progress);
            return colonyStats;
        }
    }
}
