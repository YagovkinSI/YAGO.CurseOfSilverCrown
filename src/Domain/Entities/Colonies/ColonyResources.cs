namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Ресурсы колонии
    /// </summary>
    public class ColonyResources
    {
        /// <summary>
        /// Очки действий
        /// </summary>
        public int ActionPoints { get; private set; }

        /// <summary>
        /// Солары
        /// </summary>
        public double Solars { get; private set; }

        /// <summary>
        /// Максимальная прощадь под застройку
        /// </summary>
        public int ZonesTotal { get; }

        public ColonyResources(
            int actionPoints,
            double solars,
            int zonesTotal)
        {
            ActionPoints = actionPoints;
            Solars = solars;
            ZonesTotal = zonesTotal;
        }

        public static ColonyResources CreateNew()
        {
            return new ColonyResources(
                actionPoints: 1,
                solars: 0,
                zonesTotal: 140);
        }

        internal void AddSolars(double solars)
        {
            Solars += solars;
        }

        internal void AddActionPoints(int actionPoints)
        {
            ActionPoints += actionPoints;
        }
    }
}
