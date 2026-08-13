using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyMood : ColonyResource<double>, IDeltaPerTurn<double>
    {
        public override double MinValue => 0;
        public override double MaxValue => 100;

        public ColonyMood(double value) : base(value)
        {
        }

        public double GetDeltaPerTurn(ColonyState colonyState) => colonyState.GetMoodDelta();
    }
}
