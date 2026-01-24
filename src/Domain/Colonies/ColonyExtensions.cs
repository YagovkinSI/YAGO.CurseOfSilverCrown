using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Contracts;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Colonies
{
    public static class ColonyExtensions
    {
        public static void ValidateShip(this Colony colony, Ship ship)
        {
            if (ship.Id != colony.ShipId)
                throw new YagoException("Несовпадение идентификаторов Ship.Id и Colony.ShipId");
        }

        public static void ValidateContracts(this Colony colony, Dictionary<Contract, int> contracts)
        {
            if (contracts.Count != colony.Contracts.Count)
                throw new YagoException("Несовпадение количества Colony.Сontracts и Сontracts");

            var contractIds = contracts.ToDictionary(x => x.Key.Id, x => x.Value);
            var equal = contractIds
                .OrderBy(kv => kv.Key)
                .SequenceEqual(colony.Contracts.OrderBy(kv => kv.Key));
            if (!equal)
                throw new YagoException("Несовпадение Colony.Сontracts и Сontracts");
        }

        public static double CalculateSolarIncome(this Colony colony, Dictionary<Contract, int> contracts, Ship ship, GavernorType codeOfLaws)
        {
            ValidateShip(colony, ship);
            ValidateContracts(colony, contracts);

            var codeOfLawsEffect = 1 + ((double)codeOfLaws - 2) * 0.2;

            return contracts.Sum(x => x.Key.SolarsIncome * x.Value) * codeOfLawsEffect - ship.Maintenance;
        }

        public static double CalculateGavernorType(this Colony colony, Dictionary<Contract, int> contracts)
        {
            ValidateContracts(colony, contracts);

            var humanistWeight = 0;
            switch (colony.CodeOfLaws)
            {
                case GavernorType.Humanist:
                    humanistWeight += 10;
                    break;
                case GavernorType.Capitalist:
                    humanistWeight -= 10;
                    break;
            }

            humanistWeight += contracts
                .Where(x => x.Key.GavernorType == GavernorType.Humanist)
                .Sum(x => x.Value);
            humanistWeight -= contracts
                .Where(x => x.Key.GavernorType == GavernorType.Capitalist)
                .Sum(x => x.Value);

            var maxWeight = 10 + contracts.Sum(x => x.Value);

            var weight = (double)humanistWeight / maxWeight;

            return 2 - weight;
        }

        public static int CalculatePopulation(this Colony colony, Dictionary<Contract, int> contracts)
        {
            ValidateContracts(colony, contracts);

            return contracts.Sum(x => x.Key.Population * x.Value);
        }

        public static int CalculateZonesOccupied(this Colony colony, Dictionary<Contract, int> contracts)
        {
            ValidateContracts(colony, contracts);

            return contracts.Sum(x => x.Key.ZonesOccupied * x.Value);
        }

        public static int CalculateZonesTotal(this Colony colony, Ship ship)
        {
            ValidateShip(colony, ship);

            return ship.Zones;
        }
    }
}
