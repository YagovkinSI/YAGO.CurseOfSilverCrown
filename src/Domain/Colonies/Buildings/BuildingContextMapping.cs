using System;
using System.Linq;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public static class BuildingContextMapping
    {
        public static BuildingContext GetBuildingContext(this ColonyState colonyState)
        {
            var corporateTaxRate = (float)colonyState.Policy.Reforms.CorporateTaxRate.Value;
            var stability = colonyState.GetStability();
            var logisticEffect = colonyState.GetLogisticEffect();
            return new BuildingContext(
                colonyState.Industries.ToDictionary(k => k.Key, v => v.Value.Total),
                corporateTaxRate,
                stability,
                logisticEffect,
                GetAutomationTaxCoefficient(colonyState.Policy.Reforms.AutomationIncentive.Value),
                GetAutomationPopulationCoefficient(colonyState.Policy.Reforms.AutomationIncentive.Value),
                GetAutomationInvestmentCoefficient(colonyState.Policy.Reforms.AutomationIncentive.Value),
                GetAutomationGdpCoefficient(colonyState.Policy.Reforms.AutomationIncentive.Value));
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

        private static double GetAutomationInvestmentCoefficient(AutomationIncentiveLevel automationIncentiveLevel)
        {
            return automationIncentiveLevel switch
            {
                AutomationIncentiveLevel.FullManualLabor => 0.5,
                AutomationIncentiveLevel.ManualLabor => 0.6,
                AutomationIncentiveLevel.ManualPriority => 0.8,
                AutomationIncentiveLevel.Neutral => 1,
                AutomationIncentiveLevel.RobotsPriority => 1.2,
                AutomationIncentiveLevel.Robotization => 1.5,
                AutomationIncentiveLevel.FullRobotization => 2.0,
            };
        }

        private static double GetAutomationGdpCoefficient(AutomationIncentiveLevel automationIncentiveLevel)
        {
            return automationIncentiveLevel switch
            {
                AutomationIncentiveLevel.FullManualLabor => 0.93,
                AutomationIncentiveLevel.ManualLabor => 0.94,
                AutomationIncentiveLevel.ManualPriority => 0.97,
                AutomationIncentiveLevel.Neutral => 1,
                AutomationIncentiveLevel.RobotsPriority => 0.97,
                AutomationIncentiveLevel.Robotization => 0.94,
                AutomationIncentiveLevel.FullRobotization => 0.93,
            };
        }
    }
}
