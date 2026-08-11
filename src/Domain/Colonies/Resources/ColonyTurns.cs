namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyTurns : ColonyResource<int>
    {
        public override int MinValue => 0;
        public override int MaxValue => int.MaxValue;

        public ColonyTurns(int value) : base(value)
        {
        }
    }
}
