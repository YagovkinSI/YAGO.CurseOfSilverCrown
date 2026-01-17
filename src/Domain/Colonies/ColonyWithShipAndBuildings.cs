using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;
using YAGO.World.Domain.Units;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyWithShipAndBuildings
    {
        public Colony Colony { get; private set; }
        public Ship Ship { get; private set; }
        public Units.Unit[] Units { get; private set; }
        public int SolarIncome => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.SolarIncome)
            .Value;
        public int Challenges => (int)Parameters
            .Single(x => x.Type == ColonyParameterType.GavernorType)
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
            Units.Unit[] units)
        {
            Colony = colony;
            Ship = ship;
            Units = units;
            Parameters = RecalculateParameters();
        }

        public void HiringUnit(Units.Unit unit)
        {
            if (Colony.Solars < unit.Cost)
                throw new YagoException("Недостаточно средств.");

            if (Ship.Zones - ZonesOccupied < unit.ZonesOccupied)
                throw new YagoException("Недостаточно секторов.");

            Colony.AddSolars(-unit.Cost);
            Colony.AddBuildingId(unit.Id);

            var list = Units.ToList();
            list.Add(unit);
            Units = list.ToArray();

            Parameters = RecalculateParameters();
        }

        private IReadOnlyList<ColonyParameter> RecalculateParameters()
        {
            return
            [
                new ColonyParameter(ColonyParameterType.Solars, Colony.Solars),
                new ColonyParameter(ColonyParameterType.SolarIncome, Colony.CalculateSolarIncome(Units, Ship)),
                new ColonyParameter(ColonyParameterType.GavernorType, Colony.CalculateGavernorType(Units)),
                new ColonyParameter(ColonyParameterType.Population, Colony.CalculatePopulation(Units)),
                new ColonyParameter(ColonyParameterType.ZonesOccupied, Colony.CalculateZonesOccupied(Units))
            ];
        }
    }
}
