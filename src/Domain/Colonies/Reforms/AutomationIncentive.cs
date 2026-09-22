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
            Value = value;
        }

        internal void Set(AutomationIncentiveLevel value)
        {
            Value = value;
        }

        public static AutomationIncentive CreateNew()
        {
            return new AutomationIncentive(AutomationIncentiveLevel.Neutral);
        }
    }
}