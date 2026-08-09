using System;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.Entities.Decrees
{
    /// <summary>
    /// Долг колонии
    /// </summary>
    public class PublicDebt
    {
        public double Value { get; private set; }
        public PublicDebtContext Context { get; }
        public double SolarDelta => -1 * Value * InterestRate / 100.0 / 52.0;
        public double Limit => Context.YagoLevel switch
        {
            YagoLevel.Gray => 100_000,
            YagoLevel.Blue => 300_000,
            YagoLevel.Green => 1_000_000,
            YagoLevel.Gold => 3_000_000,
            _ => 0
        };
        public double InterestRate => Math.Max(3, Value / Limit * 10);

        public PublicDebt(
            double debt,
            PublicDebtContext context)
        {
            Value = debt;
            Context = context;
        }

        internal void Add(double delta)
        {
            Value += delta;
        }

        public bool Check(double delta)
        {
            return Value + delta <= Limit && Value + delta >= 0;
        }
    }

    public class PublicDebtContext
    {
        public YagoLevel YagoLevel { get; }

        public PublicDebtContext(YagoLevel yagoLevel)
        {
            YagoLevel = yagoLevel;
        }
    }
}
