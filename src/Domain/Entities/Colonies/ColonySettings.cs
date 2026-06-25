using System;

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
        /// Уровень налогов
        /// </summary>
        public int TaxLevel { get; private set; }

        /// <summary>
        /// Уровень социальных гарантий
        /// </summary>
        public int SocialGuaranteesLevel { get; private set; }

        public ColonySettings(
            long shipId,
            int taxLevel,
            int socialGuaranteesLevel)
        {
            ShipId = shipId;
            TaxLevel = taxLevel;
            SocialGuaranteesLevel = socialGuaranteesLevel;
        }

        public static ColonySettings CreateNew()
        {
            return new ColonySettings(
                shipId: 1,
                taxLevel: 3,
                socialGuaranteesLevel: 3);
        }

        public string GetShipName()
        {
            return ShipId switch
            {
                1 => "Рассвет-342",
                _ => throw new NotImplementedException("Неизвестный идентификатор станции.")
            };
        }

        public CodeOfLaws GetCodeOfLaws()
        {
            var humanism = SocialGuaranteesLevel - TaxLevel;
            return humanism switch
            {
                > 1 => CodeOfLaws.Humanist,
                < -1 => CodeOfLaws.Capitalist,
                _ => CodeOfLaws.Centrist
            };
        }

        internal void SetTaxLevel(int value)
        {
            TaxLevel = value;
        }

        internal void SetSocialGuaranteesLevel(int value)
        {
            SocialGuaranteesLevel = value;
        }
    }
}
