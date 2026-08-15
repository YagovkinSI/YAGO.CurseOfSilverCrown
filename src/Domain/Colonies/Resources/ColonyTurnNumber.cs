namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyTurnNumber : ColonyResource<int>
    {
        public override int MinValue => 0;
        public override int MaxValue => int.MaxValue;

        public ColonyTurnNumber(int value) : base(value)
        {
        }
    }
}
