namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyResources
    {
        public ColonySolars Solars { get; }
        public ColonyActionPoints ActionPoints { get; }

        public ColonyResources(
            ColonySolars solars,
            ColonyActionPoints actionPoints)
        {
            Solars = solars;
            ActionPoints = actionPoints;
        }

        internal static ColonyResources CreateNew()
        {
            var solars = new ColonySolars(value: 0);
            var actionPoints = new ColonyActionPoints(value: 0);
            return new ColonyResources(solars, actionPoints);
        }
    }
}
