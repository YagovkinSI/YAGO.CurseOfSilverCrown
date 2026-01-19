using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.Contracts
{
    /// <summary>
    /// ОТряд или юнит
    /// </summary>
    public class Contract
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public int Cost { get; }

        /// <summary>
        /// Площадь
        /// </summary>
        public int ZonesOccupied { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public int SolarsIncome { get; }

        /// <summary>
        /// Репутация
        /// </summary>
        public GavernorType GavernorType { get; }

        /// <summary>
        /// Население
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// Текст
        /// </summary>
        public string[] Text { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public string[] Description { get; }

        public Contract(
            long id,
            string name,
            int cost,
            int zonesOccupied,
            int solarsIncome,
            GavernorType gavernorType,
            int population,
            string[] text,
            string[] description)
        {
            Id = id;
            Name = name;
            Cost = cost;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            GavernorType = gavernorType;
            Population = population;
            Text = text;
            Description = description;
        }
    }
}
