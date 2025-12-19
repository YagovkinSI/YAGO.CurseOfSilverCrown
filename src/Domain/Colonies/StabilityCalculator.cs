using System;
using System.Collections.Generic;

namespace YAGO.World.Domain.Colonies
{
    public static class StabilityCalculator
    {
        private const decimal SolarIncomeInStability = 10;

        private static readonly Random _random = new();

        private static readonly Dictionary<StabilityResultType, decimal> _chanceDistributionDefault = new()
        {
            { StabilityResultType.Disaster, 0.01m },
            { StabilityResultType.Crisis, 0.05m },
            { StabilityResultType.Trouble, 0.2m }
        };

        private static readonly Dictionary<StabilityResultType, decimal> _effectDistribution = new()
        {
            { StabilityResultType.Disaster, 0.5m },
            { StabilityResultType.Crisis, 0.2m },
            { StabilityResultType.Trouble, 0.1m },
            { StabilityResultType.Stability, 0 },
            { StabilityResultType.Luck, 1.2m }
        };

        public static decimal CalculateIncome(decimal stability, decimal solarIncome)
        {
            var resultCycle = (decimal)_random.NextDouble();
            if (resultCycle > 0.95m)
                return solarIncome * _effectDistribution[StabilityResultType.Luck];

            var risk = stability * SolarIncomeInStability / solarIncome;
            var currentResult = StabilityResultType.Disaster;
            while (risk > 0 && currentResult < StabilityResultType.Stability)
            {
                const decimal MaxCurrentRiskPercent = 0.75m;
                var currentResultRiskChance = risk * MaxCurrentRiskPercent;
                var currentResultChance = CalcChance(currentResult, currentResultRiskChance);
                if (resultCycle < currentResultChance)
                    return solarIncome * _effectDistribution[currentResult];
                risk -= currentResultRiskChance;
                currentResult++;
            }

            return solarIncome * _effectDistribution[StabilityResultType.Stability];
        }

        private static decimal CalcChance(StabilityResultType stabilityResult, decimal disasterRiskChance)
        {
            return (disasterRiskChance / _effectDistribution[stabilityResult]) + _chanceDistributionDefault[stabilityResult];
        }
    }
}
