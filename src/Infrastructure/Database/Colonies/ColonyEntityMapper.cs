using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Services;

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
            var colonyEvents = colonyParameters.Events.Select(x => x.ToDomain()).ToList();
            return new Colony(
                source.Id,
                source.UserId,
                colonyName,
                colonyStats,
                colonyEvents);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyName = source.Name;
            var colonyStats = source.State;
            var colonySolars = new ColonySolarsEntity(
                colonyStats.GetValue(StateKey.SolarsCurrent),
                colonyStats.GetValue(StateKey.SolarsDelta));
            var colonyActionPoints = new ColonyActionPointsEntity(
                colonyStats.GetValue(StateKey.ActionPointsCurrent),
                colonyStats.GetValue(StateKey.ActionPointsDelta));
            var colonyModules = new ColonyModulesEntity(
                colonyStats.GetValue(StateKey.ModulesTotal),
                colonyStats.GetValue(StateKey.ModulesUsed));
            var colonyMood = new ColonyMoodEntity(
                colonyStats.GetValue(StateKey.MoodCurrent));
            var colonyReforms = new ColonyReformsEntity(
                colonyStats.GetValue(StateKey.ReformsTaxLevel),
                colonyStats.GetValue(StateKey.ReformsSocialGuaranteesLevel),
                colonyStats.GetValue(StateKey.PublicDebt));
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
                colonyActionPoints,
                colonyModules,
                colonyMood,
                colonyReforms,
                colonyIndustry,
                colonyFlags,
                colonyCounters);
            var colonyEvents = source.Events
                .Select(x => x.ToEntity())
                .ToList();
            var colonyParameters = new ColonyParameters(
                colonyName.Named,
                colonyStatsEntity,
                colonyEvents);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);
            return new ColonyEntity(
                source.Id,
                source.UserId,
                colonyName.DatabaseName,
                solars: colonyStats.GetValue(StateKey.SolarsCurrent),
                statesJson);
        }

        private static ColonyState GetColonyStats(ColonyParameters colonyParameter)
        {
            var states = colonyParameter.States;
            var resources = new List<ColonyResource>
            {
                new ColonySolars(states.Solars.Reserve),
                new ColonyActionPoints(states.ActionPoints.Reserve),
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
                new ColonyReform(ColonyReformType.PublicDebt, states.Reforms.PublicDebt),
            };
            var buildings = new List<ColonyIndustry>
            {
                new ColonyAdministrative(
                    (int)states.Industries.Administrative.Private,
                    (int)states.Industries.Administrative.State),
                new ColonyMining(
                    (int)states.Industries.Mining.Private,
                    (int)states.Industries.Mining.State),
                new ColonyProduction(
                    (int)states.Industries.Production.Private,
                    (int)states.Industries.Production.State),
                new ColonyService(
                    (int)states.Industries.Service.Private,
                    (int)states.Industries.Service.State),
            };
            var progress = new Dictionary<ColonyProgressType, bool>()
            {
                { ColonyProgressType.FirstWedding, states.Flags.FirstWedding > 0.5 }
            };
            var colonyStats = new ColonyState(resources, slots, reforms, buildings, progress);
            return colonyStats;
        }

        private static ColonyEventEntity ToEntity(this ColonyEvent colonyEvent)
        {
            return new ColonyEventEntity(colonyEvent.EventId, colonyEvent.IsRead, colonyEvent.CreatedAtUtc);
        }

        private static ColonyEvent ToDomain(this ColonyEventEntity colonyEvent)
        {
            return new ColonyEvent(colonyEvent.EventId, colonyEvent.IsRead, colonyEvent.CreatedAtUtc);
        }
    }
}
