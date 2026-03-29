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

        public int TaxLevel => (int)CodeOfLaws * 2 - 1;
        public int SocialGuaranteesLevel => 7 - (int)CodeOfLaws * 2;

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
