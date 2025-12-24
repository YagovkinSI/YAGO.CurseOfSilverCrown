using System;
using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Notifications;

namespace YAGO.World.Domain.Colonies
{
    public static class СhallengesCalculator
    {
        private const decimal SolarIncomeInStability = 5;

        private static readonly Random _random = new();

        private static readonly Dictionary<СhallengesResultType, decimal> _chanceDistributionDefault = new()
        {
            { СhallengesResultType.Disaster, 0.02m },
            { СhallengesResultType.Crisis, 0.1m },
            { СhallengesResultType.Trouble, 0.08m },
            { СhallengesResultType.Stability, 0.6m },
            { СhallengesResultType.Luck, 0.2m }
        };

        private static readonly Dictionary<СhallengesResultType, decimal> _effectDistribution = new()
        {
            { СhallengesResultType.Disaster, 0.4m },
            { СhallengesResultType.Crisis, 0.8m },
            { СhallengesResultType.Trouble, 0.9m },
            { СhallengesResultType.Stability, 1m },
            { СhallengesResultType.Luck, 1.2m }
        };

        public static Notification CalculateIncome(int challenges, int solarIncome)
        {
            decimal solarChanged;
            var resultCycle = (decimal)_random.NextDouble();

            var risk = challenges * SolarIncomeInStability / solarIncome;
            var currentResult = СhallengesResultType.Disaster;
            while (currentResult < СhallengesResultType.Luck)
            {
                var MaxCurrentRiskPercent = currentResult < СhallengesResultType.Trouble
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

            solarChanged = solarIncome * _effectDistribution[СhallengesResultType.Luck];
            return GetNotification(solarChanged, СhallengesResultType.Luck);
        }

        private static decimal CalcChance(СhallengesResultType stabilityResult, decimal disasterRiskChance)
        {
            if (disasterRiskChance == 0 || _effectDistribution[stabilityResult] == 0)
                return _chanceDistributionDefault[stabilityResult];
            return (disasterRiskChance / (1 - _effectDistribution[stabilityResult])) + _chanceDistributionDefault[stabilityResult];
        }

        private static Notification GetNotification(decimal solarChanged, СhallengesResultType stabilityResultType)
        {
            const int temporaryModificator = 3;
            var solarParameter = new ColonyParameter(
                ColonyParameterType.Solars,
                solarChanged * temporaryModificator);

            return stabilityResultType switch
            {
                СhallengesResultType.Disaster => GetDisaster(solarParameter),
                СhallengesResultType.Crisis => GetCrisis(solarParameter),
                СhallengesResultType.Trouble => GetTrouble(solarParameter),
                СhallengesResultType.Stability => GetStability(solarParameter),
                СhallengesResultType.Luck => GetLuck(solarParameter),
                _ => GetUnknown(solarParameter),
            };
        }

        private static Notification GetDisaster(ColonyParameter solarParameter)
        {
            return new Notification(
                "Бунт рудокопов",
                IllustrationRunCycle.MinersRevolt,
                new string[]
                {
                    "Недовольство условиями и долгой изоляцией достигло пика. " +
                    "Группа рудокопов захватила склад скафандров и шлюз, " +
                    "угрожая разгерметизацией корабля, если их требования не будут выполнены.",
                    "Прибыль ушла на подавление мятежа и ремонт."
                },
                new List<ColonyParameter>() { solarParameter });
        }

        private static Notification GetCrisis(ColonyParameter solarParameter)
        {
            return new Notification(
                "Потеря груза",
                IllustrationRunCycle.LossOfCargo,
                new string[]
                {
                    "В результате сбоя магнитного захвата манипулятора ценнейший " +
                    "монолитный фрагмент астероида, богатый редкоземельными металлами, " +
                    "вырвался и улетел в космическую пустоту.",
                    "Попытки его вернуть сорвали график добычи.",
                },
                new List<ColonyParameter>() { solarParameter });
        }

        private static Notification GetTrouble(ColonyParameter solarParameter)
        {
            return new Notification(
                "Замыкание в жилом секторе",
                IllustrationRunCycle.FireInResidentialArea,
                new string[]
                {
                    "Из-за перегрузки проводки в жилом модуле случился пожар. " +
                    "Отсек залит пеной, оборудование требует замены. " +
                    "Эвакуированных колонистов разместили в соседних отсеках.",
                    "Непредвиденное соседство порождает напряжённость и недовольство.",
                },
                new List<ColonyParameter>() { solarParameter });
        }

        private static Notification GetStability(ColonyParameter solarParameter)
        {
            return new Notification(
                "Штатный цикл",
                IllustrationRunCycle.RegularCycle,
                new string[]
                {
                    "Всё идёт по плану. В трюмах ритмично гудят дробилки, " +
                    "на мостике горят зелёные лампочки систем. Рудокопы в своих сменах монотонно, " +
                    "но эффективно откалывают породу.",
                    "Прибыль стабильна.",
                },
                new List<ColonyParameter>() { solarParameter });
        }

        private static Notification GetLuck(ColonyParameter solarParameter)
        {
            return new Notification(
                "«Золотая жила»",
                IllustrationRunCycle.GoldMine,
                new string[]
                {
                    "Вскрыв новый участок, геологи наткнулись на компактное месторождение " +
                    "платиноидов высокой чистоты. Его удалось быстро и безопасно извлечь, " +
                    "что резко увеличило стоимость груза.",
                    "На корабле царит приподнятое настроение."
                },
                new List<ColonyParameter>() { solarParameter });
        }

        private static Notification GetUnknown(ColonyParameter solarParameter)
        {
            return new Notification(
                "-",
                IllustrationRunCycle.Unknown,
                new string[] { "-" },
                new List<ColonyParameter>() { solarParameter });
        }
    }
}
