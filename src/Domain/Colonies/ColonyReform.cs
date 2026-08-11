using System.Collections.Generic;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyReform
    {
        public ColonyReformType Type { get; }
        public double Value { get; private set; }

        public ColonyReform(
            ColonyReformType type,
            double value)
        {
            Type = type;
            Value = value;
        }

        internal void Add(double delta)
        {
            Value += delta;
        }

        internal static List<ColonyReform> CreateNew()
        {
            return
            [
                new ColonyReform(ColonyReformType.TaxLevel, value: 3),
                new ColonyReform(ColonyReformType.SocialGuaranteesLevel, value: 3),
                new ColonyReform(ColonyReformType.PublicDebt, value: 30_000),
            ];
        }
    }
}
