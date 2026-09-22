using System;
using YAGO.World.Domain.Colonies.Reforms;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public static class BuildingContextMapping
    {
        public static BuildingContext GetBuildingContext(this ColonyState colonyState)
        {
            var corporateTaxRate = (float)colonyState.Reforms.CorporateTaxRate.Value;
            var stability = colonyState.GetStability();
            var logisticEffect = colonyState.Asteroid == null
                ? 1.0
                : 0.775 / Math.Sqrt(colonyState.Asteroid.DistanceToCeres);
            return new BuildingContext(
                corporateTaxRate,
                stability,
                logisticEffect,
                GetAutomationTaxCoefficient(colonyState.Reforms.AutomationIncentive.Value),
                GetAutomationPopulationCoefficient(colonyState.Reforms.AutomationIncentive.Value),
                GetAutomationGdpCoefficient(colonyState.Reforms.AutomationIncentive.Value));
        }

        private static double GetAutomationTaxCoefficient(AutomationIncentiveLevel automationIncentiveLevel)
        {
            return automationIncentiveLevel switch
            {
                AutomationIncentiveLevel.FullManualLabor => 0.75,
                AutomationIncentiveLevel.ManualLabor => 0.8,
                AutomationIncentiveLevel.ManualPriority => 0.9,
                AutomationIncentiveLevel.Neutral => 1,
                AutomationIncentiveLevel.RobotsPriority => 0.9,
                AutomationIncentiveLevel.Robotization => 0.8,
                AutomationIncentiveLevel.FullRobotization => 0.75,
            };
        }

        private static double GetAutomationPopulationCoefficient(AutomationIncentiveLevel automationIncentiveLevel)
        {
            return automationIncentiveLevel switch
            {
                AutomationIncentiveLevel.FullManualLabor => 2,
                AutomationIncentiveLevel.ManualLabor => 1.6,
                AutomationIncentiveLevel.ManualPriority => 1.3,
                AutomationIncentiveLevel.Neutral => 1,
                AutomationIncentiveLevel.RobotsPriority => 0.8,
                AutomationIncentiveLevel.Robotization => 0.6,
                AutomationIncentiveLevel.FullRobotization => 0.4,
            };
        }

        private static double GetAutomationGdpCoefficient(AutomationIncentiveLevel automationIncentiveLevel)
        {
            return automationIncentiveLevel switch
            {
                AutomationIncentiveLevel.FullManualLabor => 0.85,
                AutomationIncentiveLevel.ManualLabor => 0.9,
                AutomationIncentiveLevel.ManualPriority => 0.95,
                AutomationIncentiveLevel.Neutral => 1,
                AutomationIncentiveLevel.RobotsPriority => 0.95,
                AutomationIncentiveLevel.Robotization => 0.9,
                AutomationIncentiveLevel.FullRobotization => 0.85,
            };
        }
    }
}
