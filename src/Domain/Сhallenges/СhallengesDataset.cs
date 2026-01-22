using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Сhallenges
{
    public static class СhallengesDataset
    {
        public static Сhallenge[] Get()
        {
            return
            [
                GetMinersRevolt(),
                GetLossOfCargo(),
                GetFireInResidentialArea(),
                GetGoldMine(),
            ];
        }

        private static Сhallenge GetMinersRevolt()
        {
            return new(
                1,
                "Бунт рудокопов",
                IllustrationRunCycle.MinersRevolt,
                new string[]
                {
                    "Недовольство условиями и долгой изоляцией достигло пика. " +
                    "Группа рудокопов захватила склад скафандров и шлюз, " +
                    "угрожая разгерметизацией корабля, если их требования не будут выполнены.",
                    "Прибыль ушла на подавление мятежа и ремонт."
                },
                -0.2,
                -600,
                [
                    new ParameterModifier(Colonies.ColonyParameterType.GavernorType, 0.15),
                    new ParameterModifier(Colonies.ColonyParameterType.Population, 0.0001)
                ]);
        }

        private static Сhallenge GetLossOfCargo()
        {
            return new(
                2,
                "Потеря груза",
                IllustrationRunCycle.LossOfCargo,
                new string[]
                {
                    "В результате сбоя магнитного захвата манипулятора ценнейший " +
                    "монолитный фрагмент астероида, богатый редкоземельными металлами, " +
                    "вырвался и улетел в космическую пустоту.",
                    "Попытки его вернуть сорвали график добычи.",
                },
                0.2,
                -100,
                []);
        }

        private static Сhallenge GetFireInResidentialArea()
        {
            return new(
                3,
                "Замыкание в жилом секторе",
                IllustrationRunCycle.FireInResidentialArea,
                new string[]
                {
                    "Из-за перегрузки проводки в жилом модуле случился пожар. " +
                    "Отсек залит пеной, оборудование требует замены. " +
                    "Эвакуированных колонистов разместили в соседних отсеках.",
                    "Непредвиденное соседство порождает напряжённость и недовольство.",
                },
                0.0,
                -50,
                [
                    new ParameterModifier(Colonies.ColonyParameterType.Population, 0.0003)
                ]);
        }

        private static Сhallenge GetGoldMine()
        {
            return new(
                4,
                "«Золотая жила»",
                IllustrationRunCycle.GoldMine,
                new string[]
                {
                    "Вскрыв новый участок, геологи наткнулись на компактное месторождение " +
                    "платиноидов высокой чистоты. Его удалось быстро и безопасно извлечь, " +
                    "что резко увеличило стоимость груза.",
                    "На корабле царит приподнятое настроение."
                },
                0.2,
                100,
                []);
        }
    }
}
