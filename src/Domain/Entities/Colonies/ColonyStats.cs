using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes.States;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyStats
    {
        public Dictionary<string, IState> States { get; }

        public ColonyIndustryList Industries { get; }

        public ColonyStats(
            ColonyIndustryList industries,
            Dictionary<string, IState> states)
        {
            Industries = industries;
            States = states;
        }

        public static ColonyStats CreateNew()
        {
            var colonyIndustryList = new ColonyIndustryList(
                administrativeIndustry: AdministrativeIndustry.CreateNew(),
                minningIndustry: MinningIndustry.CreateNew(),
                productionIndustry: ProductionIndustry.CreateNew(),
                serviceIndustry: ServiceIndustry.CreateNew());
            var states = new Dictionary<string, IState>()
            {
                { StateKeys.Solars.Reserve, new MutableState(StateKeys.Solars.Reserve, 0) },
                { StateKeys.ReformPoints.Income, new MutableState(StateKeys.ReformPoints.Income, 1) },
                { StateKeys.Mood.Reserve, new MutableState(StateKeys.Mood.Reserve, 50, minValue: 0, maxValue: 100) },
                { StateKeys.Counters.Turns, new MutableState(StateKeys.Counters.Turns, 1) },
                { StateKeys.Flags.Events.FirstWedding, new MutableState(StateKeys.Flags.Events.FirstWedding, 0) },
                { StateKeys.ReformPoints.Reserve, new MutableState(StateKeys.ReformPoints.Reserve, 1, minValue: 0, maxValue: 10) },
                { StateKeys.Modules.Total, new MutableState(StateKeys.Modules.Total, 140) },
                { StateKeys.Reforms.TaxLevel, new MutableState(StateKeys.Reforms.TaxLevel, 3) },
                { StateKeys.Reforms.SocialGuaranteesLevel, new MutableState(StateKeys.Reforms.SocialGuaranteesLevel, 3) },
            };
            return new ColonyStats(
                colonyIndustryList,
                states);
        }

        public double GetGameParameter(string parameterName)
        {
            return States.ContainsKey(parameterName)
                ? States[parameterName].GetValue(this)
                : parameterName switch
                {
                    StateKeys.Mood.Income => MoodTotalBalanceCacl(),
                    StateKeys.Population => GetPopulation(),
                    StateKeys.Modules.Used => GetZonesOccupied(),
                    StateKeys.Solars.Income => GetSolarsIncome(),
                    StateKeys.Modules.Free => GetZonesAvailable(),
                    StateKeys.Industries.Attractiveness => AttractivenessTotalCalc(),
                    StateKeys.Industries.Service.Buildings.Need => Industries.Service.NeedCalculation(GetPopulation()),

                    StateKeys.Industries.Administrative.Buildings.State => Industries.Administrative.StateOwnedBuildingCount,
                    StateKeys.Industries.Administrative.Buildings.Private => Industries.Administrative.PrivateBuildingCount,
                    StateKeys.Industries.Minning.Buildings.Available => Industries.Minning.UnitAvailable,
                    StateKeys.Industries.Minning.Buildings.State => Industries.Minning.StateOwnedBuildingCount,
                    StateKeys.Industries.Minning.Buildings.Private => Industries.Minning.PrivateBuildingCount,
                    StateKeys.Industries.Production.Buildings.State => Industries.Production.StateOwnedBuildingCount,
                    StateKeys.Industries.Production.Buildings.Private => Industries.Production.PrivateBuildingCount,
                    StateKeys.Industries.Service.Buildings.State => Industries.Service.StateOwnedBuildingCount,
                    StateKeys.Industries.Service.Buildings.Private => Industries.Service.PrivateBuildingCount,
                    _ => throw new YagoUnknownTypeException(parameterName)
                };
        }

        public int GetPopulation()
        {
            var result = 0;
            foreach (var industry in Industries)
            {
                var building = BuildingDataset.GetByType(industry.Type);
                result += industry.BuildingCount * building.Population;
            }
            return result;
        }

        public int GetZonesOccupied()
        {
            var result = 0;
            foreach (var industry in Industries)
            {
                var building = BuildingDataset.GetByType(industry.Type);
                result += industry.BuildingCount * building.ZonesOccupied;
            }
            return result;
        }

        public double GetSolarsIncome()
        {
            var result = 0.0;
            foreach (var industry in Industries)
            {
                var building = BuildingDataset.GetByType(industry.Type);
                result += (industry.PrivateBuildingCount + (3 * industry.StateOwnedBuildingCount)) * building.SolarsIncome;
            }
            return result;
        }

        public int GetZonesAvailable()
        {
            return (int)GetGameParameter(StateKeys.Modules.Total) - GetZonesOccupied();
        }

        public void IssueDecree(Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.ReformPoints.Reserve)?.Value ?? 0;
            if (States[StateKeys.ReformPoints.Reserve].IsLessThan(-actionPoints))
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Solars.Reserve)?.Value ?? 0;
            if (States[StateKeys.Solars.Reserve].IsLessThan(-solarResservesParameter))
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = GetZonesAvailable();
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Modules.Used)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");


            foreach (var parameter in decree.Parameters)
            {
                (States[parameter.Name] as IMutableState)?.Add(solarResservesParameter);
            }
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var paramter in colonyParameters)
            {
                if (States.ContainsKey(paramter.Name))
                {
                    if (States[paramter.Name] is not IMutableState state)
                        throw new YagoException($"Параметр {paramter.Name} не доступен для изменения.");
                    state.Add(paramter.Value);
                    continue;
                }

                Action action = paramter.Name switch
                {
                    StateKeys.Industries.Administrative.Buildings.State => () => Industries.Administrative.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Administrative.Buildings.Private => () => Industries.Administrative.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Industries.Minning.Buildings.State => () => Industries.Minning.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Minning.Buildings.Private => () => Industries.Minning.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Industries.Production.Buildings.State => () => Industries.Production.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Production.Buildings.Private => () => Industries.Production.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Industries.Service.Buildings.State => () => Industries.Service.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Service.Buildings.Private => () => Industries.Service.AddPrivateBuilding((int)paramter.Value),

                    _ => () => { }
                    ,
                };
                action.Invoke();
            }
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -15 * GetGameParameter(StateKeys.Reforms.TaxLevel);
            var standartsEffect = -15 * GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel);
            var turns = GetGameParameter(StateKeys.Counters.Turns);
            var stabilityEffect = Math.Min(50, turns / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double MoodTotalBalanceCacl()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel) - 3) / 10.0);
            return -GetPopulation() * 0.01 * socialGuaranteesCoef;
        }

        public double GdpCalc()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel) - 3) / 10.0);
            return GetPopulation() * socialGuaranteesCoef * 10.0;
        }

        public double GdpTrendCalc()
        {
            var miningWorkerTrend = Industries.Minning.UnitAvailable > 0 ? 20 : 0;
            var productWorkerTrend = AttractivenessTotalCalc() / 100.0 * 20;
            var population = GetPopulation();
            var serviceWorkerTrend = Industries.Service.NeedCalculation(population) * 10;
            var workersTrend = miningWorkerTrend + productWorkerTrend + serviceWorkerTrend;

            return workersTrend / population * 100.0;
        }
    }
}