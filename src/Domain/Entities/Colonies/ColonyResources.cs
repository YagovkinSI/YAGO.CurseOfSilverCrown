using YAGO.World.Domain.ValueTypes;

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
        public LimitedInt ActionPoints { get; private set; }

        /// <summary>
        /// Максимальная прощадь под застройку
        /// </summary>
        public int ZonesTotal { get; }

        public ColonyResources(
            int actionPoints,
            int zonesTotal)
        {
            ActionPoints = new LimitedInt(actionPoints, minValue: 0, maxValue: 10);
            ZonesTotal = zonesTotal;
        }

        public static ColonyResources CreateNew()
        {
            return new ColonyResources(
                actionPoints: 1,
                zonesTotal: 140);
        }

        internal void AddActionPoints(int actionPoints)
        {
            ActionPoints += actionPoints;
        }
    }
}
