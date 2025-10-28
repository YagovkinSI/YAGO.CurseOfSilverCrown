using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyWithShipAndBuildings
    {
        public Colony Colony { get; private set; }
        public Ship Ship { get; private set; }
        public Building[] Buildings { get; private set; }
        public decimal SolarIncome { get; private set; }
        public decimal Reputation { get; private set; }
        public int Population { get; private set; }
        public int ZonesOccupied { get; private set; }

        public ColonyWithShipAndBuildings(
            Colony colony, 
            Ship ship, 
            Building[] buildings)
        {
            Colony = colony;
            Ship = ship;
            Buildings = buildings;
            SolarIncome = colony.CalculateSolarIncome(buildings, ship);
            Reputation = colony.CalculateReputation(buildings);
            Population = colony.CalculatePopulation(buildings);
            ZonesOccupied = colony.CalculateZonesOccupied(buildings);
        }
    }
}
