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
                GetEngineeringTeam(),
                GetMiningBrigade(),
                GetRehabilitationContingent(),
            ];
        }

        private static GameEvent GetMinersRevolt()
        {
            return new(
                1,
                "Бунт рудокопов",
                ImageSet.MinersRevolt,
                new string[]
                {
                    "Недовольство условиями и долгой изоляцией достигло пика. " +
                    "Группа рудокопов захватила склад скафандров и шлюз, " +
                    "угрожая разгерметизацией корабля, если их требования не будут выполнены.",
                    "Прибыль ушла на подавление мятежа и ремонт."
                },
                1,
                [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -500)
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, -0.02)
                ]);
        }

        private static GameEvent GetLossOfCargo()
        {
            return new(
                2,
                "Потеря груза",
                ImageSet.LossOfCargo,
                new string[]
                {
                    "В результате сбоя магнитного захвата манипулятора ценнейший " +
                    "монолитный фрагмент астероида, богатый редкоземельными металлами, " +
                    "вырвался и улетел в космическую пустоту.",
                    "Попытки его вернуть сорвали график добычи.",
                },
                0.1,
                [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -50)
                ],
                []);
        }

        private static GameEvent GetFireInResidentialArea()
        {
            return new(
                3,
                "Замыкание в жилом секторе",
                ImageSet.FireInResidentialArea,
                new string[]
                {
                    "Из-за перегрузки проводки в жилом модуле случился пожар. " +
                    "Отсек залит пеной, оборудование требует замены. " +
                    "Эвакуированных колонистов разместили в соседних отсеках.",
                    "Непредвиденное соседство порождает напряжённость и недовольство.",
                },
                -0.03,
                [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -100)
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 0.002)
                ]);
        }

        private static GameEvent GetGoldMine()
        {
            return new(
                4,
                "«Золотая жила»",
                ImageSet.GoldMine,
                new string[]
                {
                    "Вскрыв новый участок, геологи наткнулись на компактное месторождение " +
                    "платиноидов высокой чистоты. Его удалось быстро и безопасно извлечь, " +
                    "что резко увеличило стоимость груза.",
                    "На корабле царит приподнятое настроение."
                },
                0.1,
                [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, 100)
                ],
                []);
        }

        private static GameEvent GetEngineeringTeam()
        {
            return new(
                id: 5,
                title: "Инженерная Команда",
                image: ImageSet.EngineeringTeam,
                text: ["К колонии присоединяется компания по добыче ресурсов. Это высокотехнологичная инженерная команда с передовым оборудованием AS и горсткой высокооплачиваемых специалистов."],
                chanceDefault: 0.5,
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_EngineeringTeam, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, 5),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, 60),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 20),
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Extraction, 0.005),
                    new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws, -0.5),
                ]);
        }

        private static GameEvent GetMiningBrigade()
        {
            return new(
                id: 6,
                title: "Горнодобывающая Бригада",
                image: ImageSet.MiningBrigade,
                text: ["К колонии присоединяется компания по добыче ресурсов. Бригада лицензированных рудокопов с надёжным оборудованием, коих многие тысячи на поясе."],
                chanceDefault: 0,
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_MiningBrigade, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, 6),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, 40),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 30),
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Extraction, 0.005),
                ]);
        }

        private static GameEvent GetRehabilitationContingent()
        {
            return new(
                id: 7,
                title: "Реабилитационный Контингент",
                image: ImageSet.RehabilitationContingent,
                text: ["К колонии присоединяется компания по добыче ресурсов. Они используют дешёвое оборудование и контингент должников. Дёшево, но рискованно."],
                chanceDefault: -1.5,
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_RehabilitationContingent, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, 9),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, 70),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 60),
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Extraction, 0.005),
                    new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws, 0.5),
                ]);
        }
    }
}
