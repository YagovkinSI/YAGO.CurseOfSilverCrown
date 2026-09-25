using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Policies;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(source.JsonData)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            var colonyStats = GetColonyState(colonyParameters);
            return new Colony(
                source.Id,
                source.UserId,
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
            var colonyName = source.State.Name;
            var turnReserve = new TurnReserveEntity(
                source.State.TurnReserve.TurnsAvailableFixed,
                source.State.TurnReserve.LastTurnTimeAtUtc);
            var colonyState = source.State;
            var colonyStatsEntity = GetColonyStatsEntity(source);
            var stationModelId = colonyState.Policy.Station?.Id.ToEntity() ?? null;
            var stationEntity = new StationEntity(stationModelId);
            var asteroidEntity = colonyState.Policy.Asteroid == null
                ? null
                : new AsteroidEntity(AsteroidDataset.ToCode(colonyState.Policy.Asteroid.Id));
            var colonyParameters = new ColonyParameters(
                colonyName.DatabaseName,
                colonyName.Named,
                turnReserve,
                stationEntity,
                colonyStatsEntity,
                asteroidEntity);
            return colonyParameters;
        }

        private static ColonyStateEntity GetColonyStatsEntity(Colony colony)
        {
            var colonySolars = colony.State.Resources.Solars.Value;
            var colonyActionPoints = new ColonyActionPointsEntity(
                colony.State.Resources.ActionPoints.Value,
                colony.GetActionPointsDelta());
            var colonyModules = new ColonyModulesEntity(
                colony.State.Slots[ColonySlotType.Modules].GetTotal(colony.State),
                colony.State.Slots[ColonySlotType.Modules].GetUsed(colony.State));
            var colonyMood = new ColonyMoodEntity(
                colony.State.Mood.Value);
            var colonyReforms = GetColonyReformsEntity(colony);
            var colonyIndustry = GetColonyIndustryEntity(colony);
            var colonyCounters = new ColonyCountersEntity(
                colony.State.TurnNumber.Value);
            var colonyCouncil = CouncilEntityMapper.ToEntity(colony.State.Council);
            var colonyStatsEntity = new ColonyStateEntity(
                colonySolars,
                colonyActionPoints,
                colonyModules,
                colonyMood,
                colonyReforms,
                colonyIndustry,
                colony.State.Achievements.Values,
                colony.State.UnlockedWikiArticles.Values,
                colonyCounters,
                colonyCouncil);
            return colonyStatsEntity;
        }

        private static ColonyReformsEntity GetColonyReformsEntity(Colony colony)
        {
            return new ColonyReformsEntity(
                colony.State.Policy.Reforms.CorporateTaxRate.Value,
                colony.State.Policy.Reforms.MedicalInsurance.Value,
                colony.State.Policy.Reforms.PublicDebt,
                (double)colony.State.Policy.Reforms.AutomationIncentive.Value);
        }

        private static ColonyIndustryEntity GetColonyIndustryEntity(Colony colony)
        {
            var colonyAdminostrative = new ColonyBuildingsEntity(
                colony.State.Industries[ColonyIndustryType.Administrative].StateCount,
                colony.State.Industries[ColonyIndustryType.Administrative].PrivateCount);
            var colonyMining = new ColonyBuildingsEntity(
                colony.State.Industries[ColonyIndustryType.Mining].StateCount,
                colony.State.Industries[ColonyIndustryType.Mining].PrivateCount);
            var colonyService = new ColonyBuildingsEntity(
                colony.State.Industries[ColonyIndustryType.Service].StateCount,
                colony.State.Industries[ColonyIndustryType.Service].PrivateCount);
            var colonyProduction = new ColonyBuildingsEntity(
                colony.State.Industries[ColonyIndustryType.Production].StateCount,
                colony.State.Industries[ColonyIndustryType.Production].PrivateCount);
            var colonyIndustry = new ColonyIndustryEntity(
                colonyAdminostrative,
                colonyMining,
                colonyProduction,
                colonyService);
            return colonyIndustry;
        }

        private static ColonyState GetColonyState(
            ColonyParameters colonyParameters)
        {
            var turnResesve = new TurnReserve(
                colonyParameters.TurnReserve.TurnsAvailableFixed,
                colonyParameters.TurnReserve.LastTurnTimeAtUtc);
            var states = colonyParameters.States;
            var resources = GetResources(states);
            var buildings = GetBuildings(states);
            var achievements = new ColonyAchievements(
                states.Achievements);
            var wikiArticlesRead = new UnlockedWikiArticles(
                states.UnlockedWikiArticles);
            var council = CouncilEntityMapper.ToDomain(states.Council);
            var progress = new ColonyProgress(
                achievements,
                wikiArticlesRead,
                council);
            var colonyName = new ColonyName(
                colonyParameters.DatabaseName,
                colonyParameters.Named);
            var mood = new ColonyMood(states.Mood.Reserve);
            var turns = new ColonyTurnNumber((int)states.Counters.Turns);
            var policy = GetPolicy(colonyParameters);
            var colonyStats = new ColonyState(
                turnResesve,
                resources,
                policy,
                buildings,
                progress,
                colonyName,
                turns,
                mood);
            return colonyStats;
        }

        private static ColonyPolicy GetPolicy(ColonyParameters colonyParameters)
        {
            var states = colonyParameters.States;
            var reforms = GetReforms(states);
            var stationId = colonyParameters.Station.StationModelId;
            var station = stationId != null 
                ? StationModelDataset.Data[stationId.ToStationType()]
                : null;
            var asteroid = GetAsteroid(colonyParameters.Asteroid);
            return new ColonyPolicy(reforms,
                station,
                asteroid);
        }

        private static Asteroid? GetAsteroid(AsteroidEntity? source)
        {
            if (source == null)
                return null;
            return AsteroidDataset.GetRequired(source.AsteroidId);
        }

        private static ColonyReforms GetReforms(ColonyStateEntity states)
        {
            var corporateTaxRateValue = states.Reforms.CorporateTaxRate;
            var corporateTaxRate = corporateTaxRateValue > 0
                ? new CorporateTaxRate(Math.Clamp(corporateTaxRateValue, CorporateTaxRate.Min, CorporateTaxRate.Max))
                : CorporateTaxRate.CreateNew();
            var medicalInsuranceValue = states.Reforms.MedicalInsurance;
            var medicalInsurance = medicalInsuranceValue is >= MedicalInsurance.Min and <= MedicalInsurance.Max
                ? new MedicalInsurance(medicalInsuranceValue)
                : MedicalInsurance.CreateNew();
            var automationIncentiveValue = states.Reforms.AutomationIncentive;
            var automationIncentive = GetAutomationIncentive(automationIncentiveValue);
            return new ColonyReforms(
                corporateTaxRate,
                medicalInsurance,
                automationIncentive,
                states.Reforms.PublicDebt);
        }

        private static AutomationIncentive GetAutomationIncentive(double automationIncentiveValue)
        {
            var automationIncentiveLevel = (AutomationIncentiveLevel)automationIncentiveValue;
            var automationIncentive = new AutomationIncentive(automationIncentiveLevel);
            return automationIncentive;
        }

        private static ColonyResources GetResources(ColonyStateEntity states)
        {
            var solars = new ColonySolars(states.Solars);
            var actionPoints = new ColonyActionPoints(states.ActionPoints.Reserve);
            return new ColonyResources(solars, actionPoints);
        }

        private static List<ColonyIndustry> GetBuildings(ColonyStateEntity states)
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
