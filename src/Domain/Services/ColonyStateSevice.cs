using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Services
{
    public static class ColonyStateSevice
    {
        public static double GetValue(this ColonyState colonyState, StateKey stateKey)
        {
            return stateKey switch
            {
                StateKey.SolarsCurrent => colonyState.Resources[ColonyResourceType.Solars].Value,
                StateKey.SolarsDelta => colonyState.Resources[ColonyResourceType.Solars].GetDeltaPerTurn(colonyState),

                StateKey.ReformPointsCurrent => colonyState.Resources[ColonyResourceType.ReformPoints].Value,
                StateKey.ReformPointsDelta => colonyState.Resources[ColonyResourceType.ReformPoints].GetDeltaPerTurn(colonyState),

                StateKey.MoodCurrent => colonyState.Resources[ColonyResourceType.Mood].Value,
                StateKey.MoodDelta => colonyState.Resources[ColonyResourceType.Mood].GetDeltaPerTurn(colonyState),

                StateKey.TurnsCurrent => colonyState.Resources[ColonyResourceType.Turns].Value,
                StateKey.TurnsDelta => colonyState.Resources[ColonyResourceType.Turns].GetDeltaPerTurn(colonyState),

                StateKey.ModulesTotal => colonyState.Slots[ColonySlotType.Modules].Total,
                StateKey.ModulesUsed => colonyState.Slots[ColonySlotType.Modules].GetUsed(colonyState),
                StateKey.ModulesFree => colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState),

                StateKey.MiningSlotsTotal => colonyState.Slots[ColonySlotType.Mining].Total,
                StateKey.MiningSlotsUsed => colonyState.Slots[ColonySlotType.Mining].GetUsed(colonyState),
                StateKey.MiningSlotsFree => colonyState.Slots[ColonySlotType.Mining].GetFree(colonyState),

                StateKey.ReformsTaxLevel => colonyState.Reforms[ColonyReformType.TaxLevel].Value,
                StateKey.ReformsSocialGuaranteesLevel => colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value,

                StateKey.BuildingsAdministrativePrivate => colonyState.Buildings[ColonyBuildingType.Administrative].PrivateCount,
                StateKey.BuildingsAdministrativeState => colonyState.Buildings[ColonyBuildingType.Administrative].StateCount,
                StateKey.BuildingsAdministrativeTotal => colonyState.Buildings[ColonyBuildingType.Administrative].Total,

                StateKey.BuildingsMiningPrivate => colonyState.Buildings[ColonyBuildingType.Mining].PrivateCount,
                StateKey.BuildingsMiningState => colonyState.Buildings[ColonyBuildingType.Mining].StateCount,
                StateKey.BuildingsMiningTotal => colonyState.Buildings[ColonyBuildingType.Mining].Total,

                StateKey.BuildingsProductionPrivate => colonyState.Buildings[ColonyBuildingType.Production].PrivateCount,
                StateKey.BuildingsProductionState => colonyState.Buildings[ColonyBuildingType.Production].StateCount,
                StateKey.BuildingsProductionTotal => colonyState.Buildings[ColonyBuildingType.Production].Total,

                StateKey.BuildingsServicePrivate => colonyState.Buildings[ColonyBuildingType.Service].PrivateCount,
                StateKey.BuildingsServiceState => colonyState.Buildings[ColonyBuildingType.Service].StateCount,
                StateKey.BuildingsServiceTotal => colonyState.Buildings[ColonyBuildingType.Service].Total,

                StateKey.Population => colonyState.GetPopulation(),
                StateKey.Attractiveness => colonyState.GetAttractiveness(),
                StateKey.ServiceNeed => colonyState.GetServiceNeed(),

                StateKey.FlagsFirstWedding => colonyState.Progress[ColonyProgressType.FirstWedding] ? 1 : 0,

                _ => throw new YagoUnknownTypeException(stateKey.ToString())
            };
        }

        public static void AddParameter(this ColonyState colonyState, StateKey stateKey, double delta)
        {
            switch (stateKey)
            {
                case StateKey.SolarsCurrent:
                    colonyState.Resources[ColonyResourceType.Solars].Add(delta);
                    break;
                case StateKey.ReformPointsCurrent:
                    colonyState.Resources[ColonyResourceType.ReformPoints].Add(delta);
                    break;
                case StateKey.MoodCurrent:
                    colonyState.Resources[ColonyResourceType.Mood].Add(delta);
                    break;
                case StateKey.TurnsCurrent:
                    colonyState.Resources[ColonyResourceType.Turns].Add(delta);
                    break;

                case StateKey.ModulesTotal:
                    colonyState.Slots[ColonySlotType.Modules].AddTotal((int)delta);
                    break;
                case StateKey.MiningSlotsTotal:
                    colonyState.Slots[ColonySlotType.Mining].AddTotal((int)delta);
                    break;

                case StateKey.ReformsTaxLevel:
                    colonyState.Reforms[ColonyReformType.TaxLevel].Add(delta);
                    break;
                case StateKey.ReformsSocialGuaranteesLevel:
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Add(delta);
                    break;

                case StateKey.BuildingsAdministrativePrivate:
                    colonyState.Buildings[ColonyBuildingType.Administrative].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsAdministrativeState:
                    colonyState.Buildings[ColonyBuildingType.Administrative].AddState((int)delta);
                    break;

                case StateKey.BuildingsMiningPrivate:
                    colonyState.Buildings[ColonyBuildingType.Mining].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsMiningState:
                    colonyState.Buildings[ColonyBuildingType.Mining].AddState((int)delta);
                    break;

                case StateKey.BuildingsProductionPrivate:
                    colonyState.Buildings[ColonyBuildingType.Production].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsProductionState:
                    colonyState.Buildings[ColonyBuildingType.Production].AddState((int)delta);
                    break;

                case StateKey.BuildingsServicePrivate:
                    colonyState.Buildings[ColonyBuildingType.Service].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsServiceState:
                    colonyState.Buildings[ColonyBuildingType.Service].AddState((int)delta);
                    break;

                case StateKey.FlagsFirstWedding:
                    colonyState.Progress[ColonyProgressType.FirstWedding] = delta > 0;
                    break;

                default:
                    throw new YagoException($"Параметр {stateKey} недоступен для изменения.");
            }
        }

        public static void SetEpisodeParameters(this ColonyState colonyState, IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var parameter in colonyParameters)
            {
                colonyState.AddParameter(parameter.Name, parameter.Value);
            }
        }
    }
}
