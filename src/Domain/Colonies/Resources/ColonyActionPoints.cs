using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Colonies.Resources
{
    public class ColonyActionPoints : ColonyResource<int>, IDeltaPerTurn<int>
    {
        public override int MinValue => 0;
        public override int MaxValue => 10;

        public ColonyActionPoints(int value) : base(value)
        {
        }

        public int GetDeltaPerTurn(ColonyState colonyState)
        {
            return 2;
        }
    }
}
