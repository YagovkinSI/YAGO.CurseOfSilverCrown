using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Медицинская страховка колонии
    /// </summary>
    public class MedicalInsurance
    {
        public const double Min = -2;
        public const double Max = 2;

        public double Value { get; private set; }

        public MedicalInsurance(double value)
        {
            Value = Validate(value);
        }

        internal void Set(double value)
        {
            Value = Validate(value);
        }

        public static MedicalInsurance CreateNew()
        {
            return new MedicalInsurance(0);
        }

        private static double Validate(double value)
        {
            if (value is < Min or > Max)
                throw new YagoException($"Уровень медицинской страховки должен быть в диапазоне от {Min} до {Max}. Текущее значение: {value}");
            return value;
        }
    }
}