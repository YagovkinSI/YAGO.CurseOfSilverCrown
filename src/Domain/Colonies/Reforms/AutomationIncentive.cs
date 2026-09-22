using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Стимулирование автоматизации или занятости
    /// </summary>
    public class AutomationIncentive
    {
        public const double Min = -3;
        public const double Max = 3;

        public double Value { get; private set; }

        public AutomationIncentive(double value)
        {
            Value = Validate(value);
        }

        internal void Set(double value)
        {
            Value = Validate(value);
        }

        public static AutomationIncentive CreateNew()
        {
            return new AutomationIncentive(0);
        }

        private static double Validate(double value)
        {
            if (value is < Min or > Max)
                throw new YagoException($"Стимулирование автоматизации должно быть в диапазоне от {Min} до {Max}. Текущее значение: {value}");
            return value;
        }
    }
}