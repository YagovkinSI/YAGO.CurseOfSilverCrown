using Newtonsoft.Json;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
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
            var colonyStats = source.Stats;
            var colonySolars = new ColonySolarsEntity(
                colonyStats.GetGameParameter(StateKeys.Solars.Reserve),
                colonyStats.GetGameParameter(StateKeys.Solars.Income));
            var colonyReformPoints = new ColonyReformPointsEntity(
                colonyStats.GetGameParameter(StateKeys.ReformPoints.Reserve),
                colonyStats.GetGameParameter(StateKeys.ReformPoints.Income));
            var colonyModules = new ColonyModulesEntity(
                colonyStats.GetGameParameter(StateKeys.Modules.Total),
                colonyStats.GetGameParameter(StateKeys.Modules.Used));
            var colonyMood = new ColonyMoodEntity(
                colonyStats.GetGameParameter(StateKeys.Mood.Reserve));
            var colonyReforms = new ColonyReformsEntity(
                colonyStats.GetGameParameter(StateKeys.Reforms.TaxLevel),
                colonyStats.GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel));
            var colonyAdminostrative = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKeys.Industries.Administrative.Buildings.State),
                colonyStats.GetGameParameter(StateKeys.Industries.Administrative.Buildings.Private));
            var colonyMining = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKeys.Industries.Mining.Buildings.State),
                colonyStats.GetGameParameter(StateKeys.Industries.Mining.Buildings.Private));
            var colonyService = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKeys.Industries.Service.Buildings.State),
                colonyStats.GetGameParameter(StateKeys.Industries.Service.Buildings.Private));
            var colonyProduction = new ColonyBuildingsEntity(
                colonyStats.GetGameParameter(StateKeys.Industries.Production.Buildings.State),
                colonyStats.GetGameParameter(StateKeys.Industries.Production.Buildings.Private));
            var colonyIndustry = new ColonyIndustryEntity(
                colonyAdminostrative,
                colonyMining,
                colonyProduction,
                colonyService);
            var colonyFlags = new ColonyFlagsEntity(
                colonyStats.GetGameParameter(StateKeys.Flags.Events.FirstWedding));
            var colonyCounters = new ColonyCountersEntity(
                colonyStats.GetGameParameter(StateKeys.Counters.Turns));
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
                solars: (colonyStats.States[StateKeys.Solars.Reserve] as State)!.Value,
                statesJson,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        private static ColonyStats GetColonyStats(ColonyParameters colonyParameter)
        {
            var states = colonyParameter.States;
            var result = new Dictionary<string, IState>()
            {
                { StateKeys.Solars.Reserve, new MutableState(StateKeys.Solars.Reserve, states.Solars.Reserve) },
                { StateKeys.ReformPoints.Income, new MutableState(StateKeys.ReformPoints.Income, states.ReformPoints.Income) },
                { StateKeys.Mood.Reserve, new MutableState(StateKeys.Mood.Reserve, states.Mood.Reserve, minValue: 0, maxValue: 100) },
                { StateKeys.Counters.Turns, new MutableState(StateKeys.Counters.Turns, states.Counters.Turns) },
                { StateKeys.Flags.Events.FirstWedding, new MutableState(StateKeys.Flags.Events.FirstWedding, states.Flags.FirstWedding) },
                { StateKeys.ReformPoints.Reserve, new MutableState(StateKeys.ReformPoints.Reserve, states.ReformPoints.Reserve, minValue: 0, maxValue: 10) },
                { StateKeys.Modules.Total, new MutableState(StateKeys.Modules.Total, states.Modules.Total) },
                { StateKeys.Reforms.TaxLevel, new MutableState(StateKeys.Reforms.TaxLevel, states.Reforms.TaxLevel) },
                { StateKeys.Reforms.SocialGuaranteesLevel, new MutableState(StateKeys.Reforms.SocialGuaranteesLevel, states.Reforms.SocialGuaranteesLevel) },
                { StateKeys.Industries.Administrative.Buildings.Private, new MutableState(StateKeys.Industries.Administrative.Buildings.Private, states.Industries.Administrative.Private) },
                { StateKeys.Industries.Administrative.Buildings.State, new MutableState(StateKeys.Industries.Administrative.Buildings.State, states.Industries.Administrative.State) },
                { StateKeys.Industries.Mining.Buildings.Private, new MutableState(StateKeys.Industries.Mining.Buildings.Private, states.Industries.Mining.Private) },
                { StateKeys.Industries.Mining.Buildings.State, new MutableState(StateKeys.Industries.Mining.Buildings.State, states.Industries.Mining.State) },
                { StateKeys.Industries.Service.Buildings.Private, new MutableState(StateKeys.Industries.Service.Buildings.Private, states.Industries.Service.Private) },
                { StateKeys.Industries.Service.Buildings.State, new MutableState(StateKeys.Industries.Service.Buildings.State, states.Industries.Service.State) },
                { StateKeys.Industries.Production.Buildings.Private, new MutableState(StateKeys.Industries.Production.Buildings.Private, states.Industries.Production.Private) },
                { StateKeys.Industries.Production.Buildings.State, new MutableState(StateKeys.Industries.Production.Buildings.State, states.Industries.Production.State) },
            };
            var colonyStats = new ColonyStats(result);
            return colonyStats;
        }
    }
}
