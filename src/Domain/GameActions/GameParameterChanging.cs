using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.GameActions
{
    public class GameParameterChanging
    {
        private static readonly IReadOnlyList<GameParameterType> _stringParameters = [GameParameterType.ColonyName];

        public GameParameterType ParameterType { get; }
        public double? Delta { get; }

        public GameParameterChanging(
            GameParameterType parameterType,
            double? delta)
        {
            ParameterType = parameterType;
            Delta = delta;
        }

        public static GameParameterChanging CreateNumberChanging(
            GameParameterType parameterType,
            double delta)
        {
            if (_stringParameters.Contains(parameterType))
                throw new YagoNotValidException($"Изменяемый тип является строковым, а не числовым. Тип - {parameterType}");
            return new GameParameterChanging(
                parameterType,
                delta);
        }

        public static GameParameterChanging CreateStringChanging(
            GameParameterType parameterType)
        {
            if (!_stringParameters.Contains(parameterType))
                throw new YagoNotValidException($"Изменяемый тип является числовым, а не строковым. Тип - {parameterType}");
            return new GameParameterChanging(
                parameterType,
                delta: null);
        }

        public void Apply(Colony colony, string? stringValue = null)
        {
            var colonyState = colony.State;
            switch (ParameterType)
            {
                case GameParameterType.ColonyName:
                    colony.Name.SetName(stringValue);
                    break;
                case GameParameterType.SolarsCurrent:
                    colonyState.Resources.Solars.Add(Delta!.Value);
                    break;
                case GameParameterType.ActionPointsCurrent:
                    colonyState.Resources.ActionPoints.Add((int)Delta!.Value);
                    break;
                case GameParameterType.MoodCurrent:
                    colonyState.Resources.Mood.Add(Delta!.Value);
                    break;
                case GameParameterType.TurnsCurrent:
                    colonyState.Resources.TurnNumber.Add((int)Delta!.Value);
                    break;

                case GameParameterType.ModulesTotal:
                    colonyState.Slots[ColonySlotType.Modules].AddTotal((int)Delta!.Value);
                    break;
                case GameParameterType.MiningSlotsTotal:
                    colonyState.Slots[ColonySlotType.Mining].AddTotal((int)Delta!.Value);
                    break;

                case GameParameterType.ReformsTaxLevel:
                    colonyState.Reforms[ColonyReformType.TaxLevel].Add(Delta!.Value);
                    break;
                case GameParameterType.ReformsSocialGuaranteesLevel:
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Add(Delta!.Value);
                    break;
                case GameParameterType.PublicDebt:
                    colonyState.Reforms[ColonyReformType.PublicDebt].Add(Delta!.Value);
                    break;

                case GameParameterType.BuildingsAdministrativePrivate:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddPrivate((int)Delta!.Value);
                    break;
                case GameParameterType.BuildingsAdministrativeState:
                    colonyState.Industries[ColonyIndustryType.Administrative].AddState((int)Delta!.Value);
                    break;

                case GameParameterType.BuildingsMiningPrivate:
                    colonyState.Industries[ColonyIndustryType.Mining].AddPrivate((int)Delta!.Value);
                    break;
                case GameParameterType.BuildingsMiningState:
                    colonyState.Industries[ColonyIndustryType.Mining].AddState((int)Delta!.Value);
                    break;

                case GameParameterType.BuildingsProductionPrivate:
                    colonyState.Industries[ColonyIndustryType.Production].AddPrivate((int)Delta!.Value);
                    break;
                case GameParameterType.BuildingsProductionState:
                    colonyState.Industries[ColonyIndustryType.Production].AddState((int)Delta!.Value);
                    break;

                case GameParameterType.BuildingsServicePrivate:
                    colonyState.Industries[ColonyIndustryType.Service].AddPrivate((int)Delta!.Value);
                    break;
                case GameParameterType.BuildingsServiceState:
                    colonyState.Industries[ColonyIndustryType.Service].AddState((int)Delta!.Value);
                    break;

                case GameParameterType.FlagsFirstWedding:
                    colonyState.Progress[ColonyProgressType.FirstWedding] = Delta!.Value > 0;
                    break;

                default:
                    throw new YagoException($"Параметр {ParameterType} недоступен для изменения.");
            }
        }
    }
}
