namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyResources
    {
        public ColonySolars Solars { get; }
        public ColonyActionPoints ActionPoints { get; }
        public ColonyMood Mood { get; }
        public ColonyTurnNumber TurnNumber { get; }

        public ColonyResources(
            ColonySolars solars,
            ColonyActionPoints actionPoints,
            ColonyMood mood,
            ColonyTurnNumber turns)
        {
            Solars = solars;
            ActionPoints = actionPoints;
            Mood = mood;
            TurnNumber = turns;
        }

        internal static ColonyResources CreateNew()
        {
            var solars = new ColonySolars(value: 0);
            var actionPoints = new ColonyActionPoints(value: 2);
            var mood = new ColonyMood(value: 50);
            var turns = new ColonyTurnNumber(value: 1);
            return new ColonyResources(solars, actionPoints, mood, turns);
        }
    }
}
