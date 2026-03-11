using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Политики колонии (определенные игроком)
    /// </summary>
    public class ColonyPolicies
    {
        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId { get; private set; }

        /// <summary>
        /// Установленные законы
        /// </summary>
        public CodeOfLaws CodeOfLaws { get; }

        public ColonyPolicies(
            long shipId,
            CodeOfLaws codeOfLaws)
        {
            ShipId = shipId;
            CodeOfLaws = codeOfLaws;
        }

        public static ColonyPolicies CreateNew(CodeOfLaws gavernorType)
        {
            return new ColonyPolicies(
                shipId: 1,
                codeOfLaws: gavernorType);
        }

        public void SetShip(int shipId)
        {
            ShipId = shipId;
        }

        public void ValidateShip(Ship ship)
        {
            if (ship.Id != ShipId)
                throw new YagoException("Несовпадение идентификаторов Ship.Id и Colony.ShipId");
        }
    }
}
