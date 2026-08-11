using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyMood : ColonyResource
    {
        public override ColonyResourceType Type => ColonyResourceType.Mood;
        public override double MinValue => 0;
        public override double MaxValue => 100;

        public ColonyMood(double value) : base(value)
        {
        }

        public override double GetDeltaPerTurn(ColonyState colonyState) => colonyState.GetMoodDelta();
    }
}
