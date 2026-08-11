using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyTurns : ColonyResource
    {
        public override ColonyResourceType Type => ColonyResourceType.Turns;
        public override double MinValue => 0;
        public override double MaxValue => double.MaxValue;

        public ColonyTurns(int value) : base(value)
        {
        }

        public override double GetDeltaPerTurn(ColonyState colonyState)
        {
            return 1;
        }
    }
}
