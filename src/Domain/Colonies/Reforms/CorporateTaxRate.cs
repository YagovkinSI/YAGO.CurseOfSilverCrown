using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Налог на корпорации
    /// </summary>
    public class CorporateTaxRate
    {
        public const double Min = 5;
        public const double Max = 35;

        public double Value { get; private set; }

        public CorporateTaxRate(double value)
        {
            Value = Validate(value);
        }

        internal void Set(double value)
        {
            Value = Validate(value);
        }

        public static CorporateTaxRate CreateNew()
        {
            return new CorporateTaxRate(20);
        }

        private static double Validate(double value)
        {
            if (value is < Min or > Max)
                throw new YagoException($"Налог на корпорации должен быть в диапазоне от {Min} до {Max}. Текущее значение: {value}");
            return value;
        }
    }
}