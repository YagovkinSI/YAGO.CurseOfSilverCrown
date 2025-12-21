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
            var title = GetTitle(stabilityResultType);
            var text = GetText(stabilityResultType);

            var solarParameter = new ColonyParameter(
                ColonyParameterType.Solars,
                solarChanged);

            return new Notification(
                title,
                Common.IllustrationType.Unknown,
                text,
                new List<ColonyParameter>() { solarParameter });
        }

        private static string GetTitle(StabilityResultType stabilityResultType)
        {
            return  stabilityResultType switch
            {
                StabilityResultType.Disaster => "Бунт рудокопов",
                StabilityResultType.Crisis => "Потеря груза",
                StabilityResultType.Trouble => "Замыкание в жилом секторе",
                StabilityResultType.Stability => "Штатный цикл",
                StabilityResultType.Luck => "«Золотая жила»",
                _ => "-",
            };
        }

        private static string GetText(StabilityResultType stabilityResultType)
        {
            return stabilityResultType switch
            {
                StabilityResultType.Disaster =>
                    "Недовольство условиями и долгой изоляцией достигло пика. " +
                    "Группа рудокопов захватила склад скафандров и шлюз, " +
                    "угрожая разгерметизацией корабля, если их требования не будут выполнены. " +
                    "Прибыль ушла на подавление мятежа и ремонт.",
                StabilityResultType.Crisis =>
                    "Во время манёвра стыковки с астероидом произошёл сбой " +
                    "в системе магнитных захватов. Ценный контейнер с " +
                    "редкоземельными металлами сорвался и улетел в глубины космоса. " +
                    "Попытки его вернуть сорвали график добычи.",
                StabilityResultType.Trouble =>
                    "Из-за перегрузки проводки в жилом модуле случился пожар. " +
                    "Отсек залит пеной, оборудование требует замены. " +
                    "Эвакуированных колонистов разместили в соседних отсеках. " +
                    "Непредвиденное соседство порождает напряжённость и недовольство.",
                StabilityResultType.Stability =>
                    "Всё идёт по плану. В трюмах ритмично гудят дробилки, " +
                    "на мостике горят зелёные лампочки систем. Рудокопы в своих сменах монотонно, " +
                    "но эффективно откалывают породу. Прибыль стабильна.",
                StabilityResultType.Luck =>
                    "Вскрыв новый участок, геологи наткнулись на компактное месторождение " +
                    "платиноидов высокой чистоты. Его удалось быстро и безопасно извлечь, " +
                    "что резко увеличило стоимость груза. На корабле царит приподнятое настроение.",
                _ => "-",
            };
        }
    }
}
