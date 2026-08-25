namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonySolars : ColonyResource<double>
    {
        public override double MinValue => double.MinValue;
        public override double MaxValue => double.MaxValue;

        public ColonySolars(double value) : base(value)
        {
        }
    }
}
