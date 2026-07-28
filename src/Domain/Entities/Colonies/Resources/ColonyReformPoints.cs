namespace YAGO.World.Domain.Entities.Colonies.Resources
{
    public class ColonyReformPoints : ColonyResource
    {
        public override ColonyResourceType Type => ColonyResourceType.ReformPoints;
        public override double MinValue => 0;
        public override double MaxValue => 10;

        public ColonyReformPoints(double value) : base(value)
        {
        }

        public override double GetDeltaPerTurn(ColonyState colonyState)
        {
            return 1;
        }
    }
}
