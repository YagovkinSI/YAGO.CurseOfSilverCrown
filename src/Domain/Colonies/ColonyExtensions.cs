using System;
using System.Linq;
using YAGO.World.Domain.Buildings;
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

        public static void ValidateBuildings(this Colony colony, Building[] buildings)
        {
            for (var i = 0; i < buildings.Length; i++)
            {
                if (buildings[i].Id != colony.BuildingIds[i])
                    throw new YagoException("Несовпадение идентификаторов Building.Id и Colony.BuildingId");
            }
        }

        public static int CalculateSolarIncome(this Colony colony, Building[] buildings, Ship ship)
        {
            ValidateShip(colony, ship);
            ValidateBuildings(colony, buildings);

            return buildings.Sum(x => x.SolarsIncome) + ship.SolarsIncome;
        }

        public static double CalculateGavernorType(this Colony colony, Building[] buildings)
        {
            ValidateBuildings(colony, buildings);

            return buildings
                .Select(x => x.Challenges - 3)
                .Average();
        }

        public static int CalculatePopulation(this Colony colony, Building[] buildings)
        {
            ValidateBuildings(colony, buildings);

            return buildings.Sum(x => x.Population);
        }

        public static int CalculateZonesOccupied(this Colony colony, Building[] buildings)
        {
            ValidateBuildings(colony, buildings);

            return buildings.Sum(x => x.ZonesOccupied);
        }

        public static int CalculateZonesTotal(this Colony colony, Ship ship)
        {
            ValidateShip(colony, ship);

            return ship.Zones;
        }
    }
}
