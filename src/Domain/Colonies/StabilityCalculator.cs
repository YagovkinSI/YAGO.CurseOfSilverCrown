using System;
using System.Collections.Generic;
using YAGO.World.Domain.Notifications;

namespace YAGO.World.Domain.Colonies
{
    public static class StabilityCalculator
    {
        private const decimal SolarIncomeInStability = 10;

        private static readonly Random _random = new();

        private static readonly Dictionary<StabilityResultType, decimal> _chanceDistributionDefault = new()
        {
            { StabilityResultType.Disaster, 0.02m },
            { StabilityResultType.Crisis, 0.1m },
            { StabilityResultType.Trouble, 0.08m },
            { StabilityResultType.Stability, 0.6m },
            { StabilityResultType.Luck, 0.2m }
        };

        private static readonly Dictionary<StabilityResultType, decimal> _effectDistribution = new()
        {
            { StabilityResultType.Disaster, 0.4m },
            { StabilityResultType.Crisis, 0.8m },
            { StabilityResultType.Trouble, 0.9m },
            { StabilityResultType.Stability, 1m },
            { StabilityResultType.Luck, 1.2m }
        };

        public static Notification CalculateIncome(decimal stability, decimal solarIncome)
        {
            decimal solarChanged;
            var resultCycle = (decimal)_random.NextDouble();

            var risk = -stability * SolarIncomeInStability / solarIncome;
            var currentResult = StabilityResultType.Disaster;
            while (currentResult < StabilityResultType.Luck)
            {
                var MaxCurrentRiskPercent = currentResult < StabilityResultType.Trouble 
                    ? 0.75m 
                    : 1m;
                var currentResultRiskChance = risk * MaxCurrentRiskPercent;
                var currentResultChance = CalcChance(currentResult, currentResultRiskChance);
                if (resultCycle < currentResultChance)
                {
                    solarChanged = solarIncome * _effectDistribution[currentResult];
                    return GetNotification(solarChanged, currentResult);
                }
                resultCycle -= currentResultChance;
                risk -= currentResultRiskChance;
                currentResult++;
            }

            solarChanged = solarIncome * _effectDistribution[StabilityResultType.Luck];
            return GetNotification(solarChanged, StabilityResultType.Luck);
        }

        private static decimal CalcChance(StabilityResultType stabilityResult, decimal disasterRiskChance)
        {
            if (disasterRiskChance == 0 || _effectDistribution[stabilityResult] == 0)
                return _chanceDistributionDefault[stabilityResult];
            return (disasterRiskChance / (1 - _effectDistribution[stabilityResult])) + _chanceDistributionDefault[stabilityResult];
        }

        private static Notification GetNotification(decimal solarChanged, StabilityResultType stabilityResultType)
        {
            var text = stabilityResultType switch
            {
                StabilityResultType.Disaster => "Произошла катастрофа, потребовавшая больших вложений на ликвидацию последствий.",
                StabilityResultType.Crisis => "Произошел кризис, потребовавший вложений на ликвидацию последствий.",
                StabilityResultType.Trouble => "Небольшая проблема привела к небольшому снижению дохода.",
                StabilityResultType.Stability => "Всё прошло по плану, получен ожидаемый доход.",
                StabilityResultType.Luck => "Всё прошло замечательно, доход выше ожидаемого.",
                _ => "Хм...",
            };

            var solarParameter = new ColonyParameter(
                ColonyParameterType.Solars,
                solarChanged);

            return new Notification(
                "Получение дохода",
                Common.IllustrationType.Unknown,
                text,
                new List<ColonyParameter>() { solarParameter });
        }
    }
}
