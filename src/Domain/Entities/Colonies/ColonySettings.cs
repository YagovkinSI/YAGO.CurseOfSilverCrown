namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Настройки колонии
    /// </summary>
    public class ColonySettings
    {
        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId { get; private set; }

        /// <summary>
        /// Установленные законы
        /// </summary>
        public CodeOfLaws CodeOfLaws { get; }

        public ColonySettings(
            long shipId, 
            CodeOfLaws codeOfLaws)
        {
            ShipId = shipId;
            CodeOfLaws = codeOfLaws;
        }

        public static ColonySettings CreateNew(
            CodeOfLaws gavernorType)
        {
            return new ColonySettings(
                shipId: 1,
                codeOfLaws: gavernorType);
        }
    }
}
