using System.Linq;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class AreaCapacity
    {
        public int Total { get; private set; }
        public int Occupied { get; private set; }

        public AreaCapacity(Colony colony, ColonyCompanies companies, Ship ship)
        {
            colony.ValidateContracts(companies);
            colony.ValidateShip(ship);

            Total = ship.Zones;
            Occupied = companies.Companies.Sum(x => x.ZonesOccupied) + 20;
        }
    }
}
