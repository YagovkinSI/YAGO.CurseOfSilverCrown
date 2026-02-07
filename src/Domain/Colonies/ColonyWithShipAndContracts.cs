using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Contracts;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyWithShipAndContracts
    {
        public Colony Colony { get; private set; }
        public Ship Ship { get; private set; }
        public Dictionary<Contract, int> Contracts { get; private set; }
        public double SolarIncome { get; private set; }
        public double GavernorType { get; private set; }
        public int Population { get; private set; }
        public int ZonesOccupied { get; private set; }

        public ColonyWithShipAndContracts(
            Colony colony,
            Ship ship,
            Dictionary<Contract, int> contracts)
        {
            Colony = colony;
            Ship = ship;
            Contracts = contracts;

            RecalculateParameters();
        }

        public void СoncludeСontract(Contract contract, ColonyWithShipAndContracts colonyWithShipAndContractsDto)
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

            RecalculateParameters();
        }

        private void RecalculateParameters()
        {
            SolarIncome = Colony.CalculateSolarIncome(Contracts, Ship, Colony.CodeOfLaws);
            GavernorType = Colony.CalculateGavernorType(Contracts);
            Population = Colony.CalculatePopulation(Contracts);
            ZonesOccupied = Colony.CalculateZonesOccupied(Contracts);
        }
    }
}
