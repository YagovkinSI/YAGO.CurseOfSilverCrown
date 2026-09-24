namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Ступень стимулирования автоматизации или занятости
    /// </summary>
    public enum AutomationIncentiveLevel
    {
        FullManualLabor = -3,
        ManualLabor = -2,
        ManualPriority = -1,
        Neutral = 0,
        RobotsPriority = 1,
        Robotization = 2,
        FullRobotization = 3,
    }
}