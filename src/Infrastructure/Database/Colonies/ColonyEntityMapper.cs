using Newtonsoft.Json;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Resources;
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
                colonyStats.GetGameParameter(StateKey.SolarsCurrent),
                colonyStats.GetGameParameter(StateKey.SolarsDelta));
            var colonyReformPoints = new ColonyReformPointsEntity(
                colonyStats.GetGameParameter(StateKey.ReformPointsCurrent),
                colonyStats.GetGameParameter(StateKey.ReformPointsDelta));
            var colonyModules = new ColonyModulesEntity(
                colonyStats.GetGameParameter(StateKey.ModulesTotal),
                colonyStats.GetGameParameter(StateKey.ModulesUsed));
            var colonyMood = new ColonyMoodEntity(
                colonyStats.GetGameParameter(StateKey.MoodCurrent));
            var colonyReforms = new ColonyReformsEntity(
                colonyStats.GetGameParameter(StateKey.ReformsTaxLevel),
                colonyStats.GetGameParameter(StateKey.ReformsSocialGuaranteesLevel));
            var colonyAdminostrative = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKey.BuildingsAdministrativeState),
                colonyStats.GetGameParameter(StateKey.BuildingsAdministrativePrivate));
            var colonyMining = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKey.BuildingsMiningState),
                colonyStats.GetGameParameter(StateKey.BuildingsMiningPrivate));
            var colonyService = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKey.BuildingsServiceState),
                colonyStats.GetGameParameter(StateKey.BuildingsServicePrivate));
            var colonyProduction = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKey.BuildingsProductionState),
                colonyStats.GetGameParameter(StateKey.BuildingsProductionPrivate));
            var colonyIndustry = new ColonyIndustryEntity(
                colonyAdminostrative,
                colonyMining,
                colonyProduction,
                colonyService);
            var colonyFlags = new ColonyFlagsEntity(
                colonyStats.GetGameParameter(StateKey.FlagsFirstWedding));
            var colonyCounters = new ColonyCountersEntity(
                colonyStats.GetGameParameter(StateKey.TurnsCurrent));
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
                solars: colonyStats.GetGameParameter(StateKey.SolarsCurrent),
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
            var result = new List<IState>()
            {
                new MutableState(StateKey.ReformPointsDelta, states.ReformPoints.Income),
                new MutableState(StateKey.FlagsFirstWedding, states.Flags.FirstWedding),
                new MutableState(StateKey.ModulesTotal, states.Modules.Total),
                new MutableState(StateKey.ReformsTaxLevel, states.Reforms.TaxLevel),
                new MutableState(StateKey.ReformsSocialGuaranteesLevel, states.Reforms.SocialGuaranteesLevel),
            };
            var colonyStats = new ColonyState(resources, industries, result);
            return colonyStats;
        }
    }
}
