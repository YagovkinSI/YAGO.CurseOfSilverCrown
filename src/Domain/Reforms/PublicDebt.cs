using System;

namespace YAGO.World.Domain.Reforms
{
    /// <summary>
    /// Долг колонии
    /// </summary>
    public class PublicDebt
    {
        public double Value { get; private set; }
        public PublicDebtContext Context { get; }
        public double SolarDelta => -1 * Value * InterestRate / 100.0 / 52.0;
        public double Limit => Context.DebtLimit;
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
        public double DebtLimit { get; }

        public PublicDebtContext(double debtLimit)
        {
            DebtLimit = debtLimit;
        }
    }
}
