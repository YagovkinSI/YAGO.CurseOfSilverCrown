using System;
using System.Collections.Generic;
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
        public int SolarIncome => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.SolarIncome)
            .Value;
        public int Challenges => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.Сhallenges)
            .Value;
        public int Population => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.Population)
            .Value;
        public int ZonesOccupied => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.ZonesOccupied)
            .Value;
        public IReadOnlyList<ColonyParameter> Parameters { get; private set; }

        public ColonyWithShipAndBuildings(
            Colony colony,
            Ship ship,
            Building[] buildings)
        {
            Colony = colony;
            Ship = ship;
            Buildings = buildings;
            Parameters = RecalculateParameters();
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

            Parameters = RecalculateParameters();
        }

        public void AttackColony(ColonyWithShipAndBuildings targetColony)
        {
            if (targetColony.Colony.States.Any(x => x.Type == ColonyStateType.Recovery))
                throw new YagoException("Атака отменена. Цель не должна иметь статус 'Восстановление'.");

            if (Colony.WarPower <= targetColony.Colony.WarPower)
                throw new YagoException("Атака отменена. Военная сила противника должна быть ниже нашей.");

            var targetSolarsIncome = targetColony.SolarIncome;
            var prizeSolars = targetSolarsIncome * 1.2M;
            Colony.AddSolars((int)Math.Round(prizeSolars));

            targetColony.Colony.AddState(ColonyStateType.Recovery, 25);
        }

        private IReadOnlyList<ColonyParameter> RecalculateParameters()
        {
            return
            [
                new ColonyParameter(ColonyParameterType.Solars, Colony.Solars),
                new ColonyParameter(ColonyParameterType.SolarIncome, Colony.CalculateSolarIncome(Buildings, Ship)),
                new ColonyParameter(ColonyParameterType.Сhallenges, Colony.CalculateChallenges(Buildings)),
                new ColonyParameter(ColonyParameterType.Population, Colony.CalculatePopulation(Buildings)),
                new ColonyParameter(ColonyParameterType.ZonesOccupied, Colony.CalculateZonesOccupied(Buildings))
            ];
        }
    }
}
