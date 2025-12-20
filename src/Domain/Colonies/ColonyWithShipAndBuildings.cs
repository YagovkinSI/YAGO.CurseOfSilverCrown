using System;
using System.Linq;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Notifications;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyWithShipAndBuildings
    {
        public Colony Colony { get; private set; }
        public Ship Ship { get; private set; }
        public Building[] Buildings { get; private set; }
        public decimal SolarIncome { get; private set; }
        public decimal Stability { get; private set; }
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
            RecalclateParameters();
        }

        public Notification AddIncome()
        {
            var notification = StabilityCalculator.CalculateIncome(Stability, SolarIncome);
            var solarChange = notification.Parameters.First(x => x.Type == ColonyParameterType.Solars).Value;
            Colony.AddSolars(solarChange);
            return notification;
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

            RecalclateParameters();
        }

        public void AttackColony(ColonyWithShipAndBuildings targetColony)
        {
            if (targetColony.Colony.States.Any(x => x.Type == ColonyStateType.Recovery))
                throw new YagoException("Атака отменена. Цель не должна иметь статус 'Восстановление'.");

            if (Colony.WarPower <= targetColony.Colony.WarPower)
                throw new YagoException("Атака отменена. Военная сила противника должна быть ниже нашей.");

            var targetSolarsIncome = targetColony.SolarIncome;
            var prizeSolars = targetSolarsIncome * 1.2M;
            Colony.AddSolars(prizeSolars);

            targetColony.Colony.AddState(ColonyStateType.Recovery, 25);
        }

        private void RecalclateParameters()
        {
            SolarIncome = Colony.CalculateSolarIncome(Buildings, Ship);
            Stability = Colony.CalculateStability(Buildings);
            Population = Colony.CalculatePopulation(Buildings);
            ZonesOccupied = Colony.CalculateZonesOccupied(Buildings);
        }
    }
}
