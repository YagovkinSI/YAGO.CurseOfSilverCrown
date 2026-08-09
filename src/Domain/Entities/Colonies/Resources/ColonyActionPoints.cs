namespace YAGO.World.Domain.Entities.Colonies.Resources
{
    public class ColonyActionPoints : ColonyResource
    {
        public override ColonyResourceType Type => ColonyResourceType.ActionPoints;
        public override double MinValue => 0;
        public override double MaxValue => 10;

        public ColonyActionPoints(double value) : base(value)
        {
        }

        public override double GetDeltaPerTurn(ColonyState colonyState)
        {
            return 2;
        }
    }
}
