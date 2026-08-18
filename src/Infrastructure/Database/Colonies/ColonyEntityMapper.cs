using Newtonsoft.Json;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(source.JsonData)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            var turnResesve = new TurnReserve(
                colonyParameters.TurnReserve.TurnsAvailableFixed,
                colonyParameters.TurnReserve.LastTurnTimeAtUtc);
            var colonyStats = GetColonyState(colonyParameters);
            var colonyName = new ColonyName(colonyParameters.DatabaseName, colonyParameters.Named);
            return new Colony(
                source.Id,
                source.UserId,
                turnResesve,
                colonyName,
                colonyStats);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyParameters = ToColonyParameters(source);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);
            return new ColonyEntity(
                source.Id,
                source.UserId,
                statesJson);
        }

        private static ColonyParameters ToColonyParameters(Colony source)
        {
            var colonyName = source.Name;
            var turnReserve = new TurnReserveEntity(
                source.TurnReserve.TurnsAvailableFixed,
                source.TurnReserve.LastTurnTimeAtUtc);
            var colonyState = source.State;
            var colonyStatsEntity = GetColonyStatsEntity(colonyState);
            var stationModelId = colonyState.Station.Model.Id.ToEntity();
            var stationEntity = new StationEntity(colonyState.Station.Id, stationModelId);
            var colonyParameters = new ColonyParameters(
                colonyName.DatabaseName,
                colonyName.Named,
                turnReserve,
                stationEntity,
                colonyStatsEntity);
            return colonyParameters;
        }

        private static ColonyStatsEntity GetColonyStatsEntity(ColonyState colonyState)
        {
            var colonySolars = new ColonySolarsEntity(
                colonyState.GetValue(GameParameterType.SolarsCurrent),
                colonyState.GetValue(GameParameterType.SolarsDelta));
            var colonyActionPoints = new ColonyActionPointsEntity(
                colonyState.Resources.ActionPoints.Value,
                colonyState.Resources.ActionPoints.GetDeltaPerTurn(colonyState));
            var colonyModules = new ColonyModulesEntity(
                colonyState.GetValue(GameParameterType.ModulesTotal),
                colonyState.GetValue(GameParameterType.ModulesUsed));
            var colonyMood = new ColonyMoodEntity(
                colonyState.GetValue(GameParameterType.MoodCurrent));
            var colonyReforms = GetColonyReformsEntity(colonyState);
            var colonyIndustry = GetColonyIndustryEntity(colonyState);
            var colonyFlags = new ColonyFlagsEntity(
                colonyState.GetValue(GameParameterType.FlagsFirstWedding));
            var colonyCounters = new ColonyCountersEntity(
                colonyState.GetValue(GameParameterType.TurnsCurrent));
            var colonyStatsEntity = new ColonyStatsEntity(
                colonySolars,
                colonyActionPoints,
                colonyModules,
                colonyMood,
                colonyReforms,
                colonyIndustry,
                colonyFlags,
                colonyCounters);
            return colonyStatsEntity;
        }

        private static ColonyReformsEntity GetColonyReformsEntity(ColonyState colonyState)
        {
            return new ColonyReformsEntity(
                colonyState.GetValue(GameParameterType.ReformsTaxLevel),
                colonyState.GetValue(GameParameterType.ReformsSocialGuaranteesLevel),
                colonyState.GetValue(GameParameterType.PublicDebt));
        }

        private static ColonyIndustryEntity GetColonyIndustryEntity(ColonyState colonyState)
        {
            var colonyAdminostrative = new ColonyBuildingsEntity(
                colonyState.GetValue(GameParameterType.BuildingsAdministrativeState),
                colonyState.GetValue(GameParameterType.BuildingsAdministrativePrivate));
            var colonyMining = new ColonyBuildingsEntity(
                colonyState.GetValue(GameParameterType.BuildingsMiningState),
                colonyState.GetValue(GameParameterType.BuildingsMiningPrivate));
            var colonyService = new ColonyBuildingsEntity(
                colonyState.GetValue(GameParameterType.BuildingsServiceState),
                colonyState.GetValue(GameParameterType.BuildingsServicePrivate));
            var colonyProduction = new ColonyBuildingsEntity(
                colonyState.GetValue(GameParameterType.BuildingsProductionState),
                colonyState.GetValue(GameParameterType.BuildingsProductionPrivate));
            var colonyIndustry = new ColonyIndustryEntity(
                colonyAdminostrative,
                colonyMining,
                colonyProduction,
                colonyService);
            return colonyIndustry;
        }

        private static ColonyState GetColonyState(
            ColonyParameters colonyParameter)
        {
            var station = new Station(
                colonyParameter.Station.Id,
                colonyParameter.Station.StationModelId.ToStationType());
            var states = colonyParameter.States;
            var resources = GetResources(states);
            var slots = GetSlots();
            var reforms = GetReforms(states);
            var buildings = GetBuildings(states);
            var progress = new Dictionary<ColonyProgressType, bool>()
            {
                { ColonyProgressType.FirstWedding, states.Flags.FirstWedding > 0.5 }
            };
            var colonyStats = new ColonyState(station, resources, slots, reforms, buildings, progress);
            return colonyStats;
        }

        private static List<ColonySlot> GetSlots()
        {
            return
            [
                new ColonyModules(),
                new ColonyMiningSlots(),
            ];
        }

        private static List<ColonyReform> GetReforms(ColonyStatsEntity states)
        {
            return
            [
                new(ColonyReformType.TaxLevel, states.Reforms.TaxLevel),
                new(ColonyReformType.SocialGuaranteesLevel, states.Reforms.SocialGuaranteesLevel),
                new(ColonyReformType.PublicDebt, states.Reforms.PublicDebt),
            ];
        }

        private static ColonyResources GetResources(ColonyStatsEntity states)
        {
            var solars = new ColonySolars(states.Solars.Reserve);
            var actionPoints = new ColonyActionPoints(states.ActionPoints.Reserve);
            var mood = new ColonyMood(states.Mood.Reserve);
            var turns = new ColonyTurnNumber((int)states.Counters.Turns);
            return new ColonyResources(solars, actionPoints, mood, turns);
        }

        private static List<ColonyIndustry> GetBuildings(ColonyStatsEntity states)
        {
            return
            [
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
            ];
        }

        private static string ToEntity(this StationModelId stationType)
        {
            return stationType switch
            {
                StationModelId.Dawn_342 => "Dawn-342",
                StationModelId.Resolute_120 => "Resolute-120",
                _ => throw new System.NotImplementedException(),
            };
        }

        private static StationModelId ToStationType(this string stationType)
        {
            return stationType switch
            {
                "Dawn-342" => StationModelId.Dawn_342,
                "Resolute-120" => StationModelId.Resolute_120,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
