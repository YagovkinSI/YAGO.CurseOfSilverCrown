using System;
using System.Linq;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Cycles;
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

            Recalclateparameters();
        }

        private void Recalclateparameters()
        {
            SolarIncome = Colony.CalculateSolarIncome(Buildings, Ship);
            Reputation = Colony.CalculateReputation(Buildings);
            Population = Colony.CalculatePopulation(Buildings);
            ZonesOccupied = Colony.CalculateZonesOccupied(Buildings);
        }

        public void AttackColony(ColonyWithShipAndBuildings targetColony, AttackColonyPrizeType attackColonyPrizeType)
        {
            if (targetColony.Colony.States.Any(x => x.Type == ColonyStateType.Recovery))
                throw new YagoException("Атака отменена. Цель не должна иметь статус 'Восстановление'.");

            if (Colony.WarPower <= targetColony.Colony.WarPower)
                throw new YagoException("Атака отменена. Военная сила противника должна быть ниже нашей.");

            var reputationLost = (int)((targetColony.Reputation + 10000) / 500);
            Colony.AddReputationByEvents(-reputationLost);

            AddAttackPrize(targetColony, attackColonyPrizeType);

            targetColony.Colony.AddState(ColonyStateType.Recovery, 25);
        }

        private void AddAttackPrize(ColonyWithShipAndBuildings targetColony, AttackColonyPrizeType attackColonyPrizeType)
        {
            switch (attackColonyPrizeType)
            {
                case AttackColonyPrizeType.Unknown:
                    throw new YagoUnknownTypeException(nameof(AttackColonyPrizeType));
                case AttackColonyPrizeType.Solars:
                    var targetSolarsIncome = targetColony.SolarIncome;
                    var prizeSolars = targetSolarsIncome * 1.2M;
                    Colony.AddSolars(prizeSolars);
                    break;
                case AttackColonyPrizeType.Reputation:
                    var targetReputation = targetColony.Reputation;
                    var prizeReputation = (int)(-(targetReputation - 150) / 10);
                    Colony.AddReputationByEvents(prizeReputation);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
