using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    public static class ColonyStateProvider
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

                StateKey.BuildingsAdministrativePrivate => colonyState.Industries[IndustryType.Administrative].PrivateCount,
                StateKey.BuildingsAdministrativeState => colonyState.Industries[IndustryType.Administrative].StateCount,
                StateKey.BuildingsAdministrativeTotal => colonyState.Industries[IndustryType.Administrative].Total,

                StateKey.BuildingsMiningPrivate => colonyState.Industries[IndustryType.Mining].PrivateCount,
                StateKey.BuildingsMiningState => colonyState.Industries[IndustryType.Mining].StateCount,
                StateKey.BuildingsMiningTotal => colonyState.Industries[IndustryType.Mining].Total,

                StateKey.BuildingsProductionPrivate => colonyState.Industries[IndustryType.Production].PrivateCount,
                StateKey.BuildingsProductionState => colonyState.Industries[IndustryType.Production].StateCount,
                StateKey.BuildingsProductionTotal => colonyState.Industries[IndustryType.Production].Total,

                StateKey.BuildingsServicePrivate => colonyState.Industries[IndustryType.Service].PrivateCount,
                StateKey.BuildingsServiceState => colonyState.Industries[IndustryType.Service].StateCount,
                StateKey.BuildingsServiceTotal => colonyState.Industries[IndustryType.Service].Total,

                StateKey.Population => colonyState.GetPopulation(),
                StateKey.Attractiveness => colonyState.AttractivenessTotalCalc(),
                StateKey.ServiceNeed => colonyState.ServiceNeedCalculation(colonyState.GetPopulation()),

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
                    colonyState.Industries[IndustryType.Administrative].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsAdministrativeState:
                    colonyState.Industries[IndustryType.Administrative].AddState((int)delta);
                    break;

                case StateKey.BuildingsMiningPrivate:
                    colonyState.Industries[IndustryType.Mining].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsMiningState:
                    colonyState.Industries[IndustryType.Mining].AddState((int)delta);
                    break;

                case StateKey.BuildingsProductionPrivate:
                    colonyState.Industries[IndustryType.Production].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsProductionState:
                    colonyState.Industries[IndustryType.Production].AddState((int)delta);
                    break;

                case StateKey.BuildingsServicePrivate:
                    colonyState.Industries[IndustryType.Service].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsServiceState:
                    colonyState.Industries[IndustryType.Service].AddState((int)delta);
                    break;

                case StateKey.FlagsFirstWedding:
                    colonyState.Progress[ColonyProgressType.FirstWedding] = delta > 0;
                    break;

                default:
                    throw new YagoException($"Параметр {stateKey} недоступен для изменения.");
            }
        }
    }
}
