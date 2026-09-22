using System;
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

        public AutomationIncentiveLevel Value { get; private set; }

        public AutomationIncentive(AutomationIncentiveLevel value)
        {
            Value = Validate(value);
        }

        internal void Set(AutomationIncentiveLevel value)
        {
            Value = Validate(value);
        }

        public static AutomationIncentive CreateNew()
        {
            return new AutomationIncentive(AutomationIncentiveLevel.Neutral);
        }

        private static AutomationIncentiveLevel Validate(AutomationIncentiveLevel value)
        {
            if (!Enum.IsDefined(value))
                throw new YagoException($"Уровень стимулирования автоматизации не определён: {(int)value}");
            return value;
        }
    }
}