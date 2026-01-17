using System.Linq;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;
using YAGO.World.Domain.Units;

namespace YAGO.World.Domain.Colonies
{
    public static class ColonyExtensions
    {
        public static void ValidateShip(this Colony colony, Ship ship)
        {
            if (ship.Id != colony.ShipId)
                throw new YagoException("Несовпадение идентификаторов Ship.Id и Colony.ShipId");
        }

        public static void ValidateBuildings(this Colony colony, Unit[] units)
        {
            for (var i = 0; i < units.Length; i++)
            {
                if (units[i].Id != colony.UnitIds[i])
                    throw new YagoException("Несовпадение идентификаторов Building.Id и Colony.BuildingId");
            }
        }

        public static int CalculateSolarIncome(this Colony colony, Unit[] units, Ship ship)
        {
            ValidateShip(colony, ship);
            ValidateBuildings(colony, units);

            return units.Sum(x => x.SolarsIncome) + ship.SolarsIncome;
        }

        public static double CalculateGavernorType(this Colony colony, Unit[] units)
        {
            ValidateBuildings(colony, units);

            return units
                .Select(x => (double)x.GavernorType)
                .Average();
        }

        public static int CalculatePopulation(this Colony colony, Unit[] units)
        {
            ValidateBuildings(colony, units);

            return units.Sum(x => x.Population);
        }

        public static int CalculateZonesOccupied(this Colony colony, Unit[] units)
        {
            ValidateBuildings(colony, units);

            return units.Sum(x => x.ZonesOccupied);
        }

        public static int CalculateZonesTotal(this Colony colony, Ship ship)
        {
            ValidateShip(colony, ship);

            return ship.Zones;
        }
    }
}
