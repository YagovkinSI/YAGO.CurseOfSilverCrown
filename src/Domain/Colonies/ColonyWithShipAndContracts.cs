using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Contracts;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyWithDetails
    {
        public Colony Colony { get; private set; }
        public Ship Ship { get; private set; }
        public Dictionary<Contract, int> Contracts { get; private set; }
        public int SolarIncome => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.SolarIncome)
            .Value;
        public double GavernorType => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.GavernorType)
            .Value;
        public int Population => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.Population)
            .Value;
        public int ZonesOccupied => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.ZonesOccupied)
            .Value;
        public IReadOnlyList<ColonyParameter> Parameters { get; private set; }

        public ColonyWithDetails(
            Colony colony,
            Ship ship,
            Dictionary<Contract, int> contracts)
        {
            Colony = colony;
            Ship = ship;
            Contracts = contracts;
            Parameters = RecalculateParameters();
        }

        public void СoncludeСontract(Contract contract, ColonyWithDetails colonyWithShipAndContractsDto)
        {
            if (Math.Abs((int)contract.GavernorType - (int)colonyWithShipAndContractsDto.Colony.CodeOfLaws) > 1)
                throw new YagoException("Недопустимый контракт для выбранных законов.");

            if (Colony.Solars < contract.Cost)
                throw new YagoException("Недостаточно средств.");

            if (Ship.Zones - ZonesOccupied < contract.ZonesOccupied)
                throw new YagoException("Недостаточно секторов.");

            Colony.AddSolars(-contract.Cost);
            Colony.AddContract(contract.Id);

            var contractKey = Contracts.Keys.FirstOrDefault(x => x.Id == contract.Id);
            if (contractKey != null)
                Contracts[contractKey]++;
            else
                Contracts.Add(contract, 1);

            Parameters = RecalculateParameters();
        }

        private IReadOnlyList<ColonyParameter> RecalculateParameters()
        {
            return
            [
                new ColonyParameter(ColonyParameterType.Solars, Colony.Solars),
                new ColonyParameter(ColonyParameterType.SolarIncome, Colony.CalculateSolarIncome(Contracts, Ship, Colony.CodeOfLaws)),
                new ColonyParameter(ColonyParameterType.GavernorType, Colony.CalculateGavernorType(Contracts)),
                new ColonyParameter(ColonyParameterType.Population, Colony.CalculatePopulation(Contracts)),
                new ColonyParameter(ColonyParameterType.ZonesOccupied, Colony.CalculateZonesOccupied(Contracts))
            ];
        }
    }
}
