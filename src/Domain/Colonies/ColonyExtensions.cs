using System.Linq;
using YAGO.World.Domain.Companies;
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

        public static void ValidateContracts(this Colony colony, ColonyCompanies companies)
        {
            if (companies.Companies.Count != colony.CompanyIds.Count)
                throw new YagoException("Несовпадение количества Colony.Сontracts и Сontracts");

            if (!colony.CompanyIds
                    .OrderBy(x => x)
                    .SequenceEqual(companies.Companies.Select(x => x.Id).OrderBy(x => x)))
                throw new YagoException("Несовпадение Colony.Сontracts и Сontracts");
        }

        public static int CalculateZonesTotal(this Colony colony, Ship ship)
        {
            ValidateShip(colony, ship);

            return ship.Zones;
        }
    }
}
