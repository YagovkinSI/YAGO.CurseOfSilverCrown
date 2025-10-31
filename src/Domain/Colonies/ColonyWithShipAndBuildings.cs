using System.Linq;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Exceptions;
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
            Recalclateparameters();
        }

        public void AddIncome()
        {
            Colony.AddSolars(SolarIncome);
        }

        public void ByuBuilding(Building building)
        {
            if (Colony.Solars < building.Cost)
                throw new YagoException("Недостаточно средств.");

            if (Ship.Zones - ZonesOccupied < building.ZonesOccupied)
                throw new YagoException("Недостаточно секторов.");

            Colony.AddSolars(-building.Cost);
            Colony.AddBuildingId(building.Id);

            var list = Buildings.ToList();
            list.Add(building);
            Buildings = list.ToArray();
        }

        private void Recalclateparameters()
        {
            SolarIncome = Colony.CalculateSolarIncome(Buildings, Ship);
            Reputation = Colony.CalculateReputation(Buildings);
            Population = Colony.CalculatePopulation(Buildings);
            ZonesOccupied = Colony.CalculateZonesOccupied(Buildings);
        }
    }
}
