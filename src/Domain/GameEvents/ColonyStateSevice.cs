using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.GameEvents
{
    public static class ColonyStateSevice
    {
        public static double GetValue(this ColonyState colonyState, StateKey stateKey)
        {
            return stateKey switch
            {
                StateKey.SolarsCurrent => colonyState.Resources.Solars.Value,
                StateKey.SolarsDelta => colonyState.Resources.Solars.GetDeltaPerTurn(colonyState),

                StateKey.ActionPointsCurrent => colonyState.Resources.ActionPoints.Value,
                StateKey.ActionPointsDelta => colonyState.Resources.ActionPoints.GetDeltaPerTurn(colonyState),

                StateKey.MoodCurrent => colonyState.Resources.Mood.Value,
                StateKey.MoodDelta => colonyState.Resources.Mood.GetDeltaPerTurn(colonyState),

                StateKey.TurnsCurrent => colonyState.Resources.Turns.Value,

                StateKey.ModulesTotal => colonyState.Slots[ColonySlotType.Modules].GetTotal(colonyState),
                StateKey.ModulesUsed => colonyState.Slots[ColonySlotType.Modules].GetUsed(colonyState),
                StateKey.ModulesFree => colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState),

                StateKey.MiningSlotsTotal => colonyState.Slots[ColonySlotType.Mining].GetTotal(colonyState),
                StateKey.MiningSlotsUsed => colonyState.Slots[ColonySlotType.Mining].GetUsed(colonyState),
                StateKey.MiningSlotsFree => colonyState.Slots[ColonySlotType.Mining].GetFree(colonyState),

                StateKey.ReformsTaxLevel => colonyState.Reforms[ColonyReformType.TaxLevel].Value,
                StateKey.ReformsSocialGuaranteesLevel => colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value,
                StateKey.PublicDebt => colonyState.Reforms[ColonyReformType.PublicDebt].Value,

                StateKey.BuildingsAdministrativePrivate => colonyState.Industries[ColonyIndustryType.Administrative].PrivateCount,
                StateKey.BuildingsAdministrativeState => colonyState.Industries[ColonyIndustryType.Administrative].StateCount,
                StateKey.BuildingsAdministrativeTotal => colonyState.Industries[ColonyIndustryType.Administrative].Total,

                StateKey.BuildingsMiningPrivate => colonyState.Industries[ColonyIndustryType.Mining].PrivateCount,
                StateKey.BuildingsMiningState => colonyState.Industries[ColonyIndustryType.Mining].StateCount,
                StateKey.BuildingsMiningTotal => colonyState.Industries[ColonyIndustryType.Mining].Total,

                StateKey.BuildingsProductionPrivate => colonyState.Industries[ColonyIndustryType.Production].PrivateCount,
                StateKey.BuildingsProductionState => colonyState.Industries[ColonyIndustryType.Production].StateCount,
                StateKey.BuildingsProductionTotal => colonyState.Industries[ColonyIndustryType.Production].Total,

                StateKey.BuildingsServicePrivate => colonyState.Industries[ColonyIndustryType.Service].PrivateCount,
                StateKey.BuildingsServiceState => colonyState.Industries[ColonyIndustryType.Service].StateCount,
                StateKey.BuildingsServiceTotal => colonyState.Industries[ColonyIndustryType.Service].Total,

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
                    colonyState.Resources.Solars.Add(delta);
                    break;
                case StateKey.ActionPointsCurrent:
                    colonyState.Resources.ActionPoints.Add((int)delta);
                    break;
                case StateKey.MoodCurrent:
                    colonyState.Resources.Mood.Add(delta);
                    break;
                case StateKey.TurnsCurrent:
                    colonyState.Resources.Turns.Add((int)delta);
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
                case StateKey.PublicDebt:
                    colonyState.Reforms[ColonyReformType.PublicDebt].Add(delta);
                    break;

                case StateKey.BuildingsAdministrativePrivate:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsAdministrativeState:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddState((int)delta);
                    break;

                case StateKey.BuildingsMiningPrivate:
                    colonyState.Industries[ColonyIndustryType.Mining].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsMiningState:
                    colonyState.Industries[ColonyIndustryType.Mining].AddState((int)delta);
                    break;

                case StateKey.BuildingsProductionPrivate:
                    colonyState.Industries[ColonyIndustryType.Production].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsProductionState:
                    colonyState.Industries[ColonyIndustryType.Production].AddState((int)delta);
                    break;

                case StateKey.BuildingsServicePrivate:
                    colonyState.Industries[ColonyIndustryType.Service].AddPrivate((int)delta);
                    break;
                case StateKey.BuildingsServiceState:
                    colonyState.Industries[ColonyIndustryType.Service].AddState((int)delta);
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
