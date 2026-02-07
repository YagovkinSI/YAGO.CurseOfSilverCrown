using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameEvents
{
    public static class GameEventsDataset
    {
        public static GameEvent[] Get()
        {
            return
            [
                GetMinersRevolt(),
                GetLossOfCargo(),
                GetFireInResidentialArea(),
                GetGoldMine(),
                GetEngineeringTeam()
            ];
        }

        private static GameEvent GetMinersRevolt()
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
                -0.1,
                [
                    new ColonyParameter(ColonyParameterType.Solars, -500)
                ],
                [
                    new ParameterModifier(ColonyParameterType.GavernorType, 0.1),
                    new ParameterModifier(ColonyParameterType.Population, 0.0002)
                ]);
        }

        private static GameEvent GetLossOfCargo()
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
                [
                    new ColonyParameter(ColonyParameterType.Solars, -50)
                ],
                []);
        }

        private static GameEvent GetFireInResidentialArea()
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
                -0.03,
                [
                    new ColonyParameter(ColonyParameterType.Solars, -100)
                ],
                [
                    new ParameterModifier(ColonyParameterType.Population, 0.0005)
                ]);
        }

        private static GameEvent GetGoldMine()
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
                [
                    new ColonyParameter(ColonyParameterType.Solars, 100)
                ],
                []);
        }

        private static GameEvent GetEngineeringTeam()
        {
            return new(
                id: 5,
                title: "Инженерная Команда",
                image: IllustrationRunCycle.MinersRevolt,
                text: ["Передовое оборудование AS и горстка высокооплачиваемых специалистов. Дорого, престижно, эффективно."],
                chanceDefault: 1,
                colonyParameters: [
                    new ColonyParameter(ColonyParameterType.EngineeringTeam, 1),
                    new ColonyParameter(ColonyParameterType.ZonesOccupied, 5),
                    new ColonyParameter(ColonyParameterType.SolarIncome, 60),
                    new ColonyParameter(ColonyParameterType.Population, 20),
                ],
                [

                ]);
        }
    }
}
