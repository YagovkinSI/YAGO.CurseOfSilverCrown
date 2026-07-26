namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    /// <summary>
    /// Постройка
    /// </summary>
    public class BuildingSettings
    {
        /// <summary>
        /// Тип пострйоки
        /// </summary>
        public ColonyBuildingType Type { get; }

        /// <summary>
        /// Стоимость пострйоки
        /// </summary>
        public double Cost { get; }

        /// <summary>
        /// Занимаеммая площадь
        /// </summary>
        public int ZonesOccupied { get; }

        /// <summary>
        /// Занятое население (включая иждевенциев)
        /// </summary>
        public int Population { get; }

        /// <summary>
        /// Доход соларов
        /// </summary>
        public double SolarsIncome { get; }

        public BuildingSettings(
            ColonyBuildingType type, 
            double cost, 
            int zonesOccupied, 
            int population, 
            double solarsIncome)
        {
            Type = type;
            Cost = cost;
            ZonesOccupied = zonesOccupied;
            Population = population;
            SolarsIncome = solarsIncome;
        }
    }
}
