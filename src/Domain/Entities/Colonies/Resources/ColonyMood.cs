using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Entities.Colonies.Resources
{
    public class ColonyMood : ColonyResource
    {
        public override ColonyResourceType Type => ColonyResourceType.Mood;
        public override double MinValue => 0;
        public override double MaxValue => 100;

        public ColonyMood(double value) : base(value)
        {
        }

        public override double GetDeltaPerTurn(ColonyState colonyState)
        {
            var socialGuaranteesCoef = 1 - ((colonyState.GetValue(StateKey.ReformsSocialGuaranteesLevel) - 3) / 10.0);
            return -colonyState.GetPopulation() * 0.01 * socialGuaranteesCoef;
        }
    }
}
