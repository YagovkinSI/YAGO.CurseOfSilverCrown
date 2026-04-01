namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Ресурсы колонии
    /// </summary>
    public class ColonyResources
    {
        /// <summary>
        /// Солары
        /// </summary>
        public double Solars { get; private set; }

        /// <summary>
        /// Максимальная прощадь под застройку
        /// </summary>
        public int ZonesTotal { get; }

        public ColonyResources(
            double solars,
            int zonesTotal)
        {
            Solars = solars;
            ZonesTotal = zonesTotal;
        }

        public static ColonyResources CreateNew()
        {
            return new ColonyResources(
                solars: 1000,
                zonesTotal: 140);
        }

        internal void AddSolars(double solars)
        {
            Solars += solars;
        }
    }
}
