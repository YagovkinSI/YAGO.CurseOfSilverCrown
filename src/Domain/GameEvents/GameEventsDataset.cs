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
                GetFirstWedding(),
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
                0.0,
                [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -50)
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_EngineeringTeam, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_MiningBrigade, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_RehabilitationContingent, 0.02),
                ]);
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
                -0.05,
                [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -100),
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, -3)
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 0.001)
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
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, 100),
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, +1)
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_EngineeringTeam, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_MiningBrigade, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_RehabilitationContingent, 0.02)
                ]);
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
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, 3),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, 60),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 10),
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Extraction, 0.01),
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
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, 3),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, 40),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 15),
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Extraction, 0.01),
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
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, 5),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, 70),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 30),
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Extraction, 0.01),
                    new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws, 0.5),
                ]);
        }

        private static GameEvent GetFirstWedding()
        {
            return new(
                id: 8,
                title: "Первая свадьба",
                image: ImageSet.FirstWedding,
                text: [
                    "Сегодня вы получили официальный запрос от двоих резидентов: инженера и пилота грузового челнока. Они просят вас, как капитана станции, провести церемонию бракосочетания. В отсутствие ЗАГСа такая практика разрешена Орбитальным Правительством Земли — запись в бортовом журнале имеет юридическую силу.",
                    "Церемония проходит в обзорном зале. Жених в строгом костюме, невеста в платье, заказанном с Цереры около месяца назад. Почти всё свободное население станции собралось полукругом, с бокалами синтезированного игристого. Вы произносите короткую речь о том, что в пустоте человеческая связь становится абсолютной ценностью. Жених и невеста обмениваются кольцами. Вы объявляете их супругами и вносите запись в журнал.",
                    "Позже, когда гости расходятся, вы смотрите на мигающее уведомление: запись принята реестром ОПЗ. Запись номер один. Первая семья вашей станции. Ваша станция только что обрела нечто большее, чем руду. Она обрела корни."
                    ],
                chanceDefault: -0.10,
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -50),
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, +5),
                    new KeyValueParameter(ColonyParameterNames.FirstWedding, 1)
                ],
                [
                    new KeyValueParameter(ColonyParameterNames.FirstWedding, double.MinValue),
                    new KeyValueParameter(ColonyParameterNames.CurrentWeek, 0.025),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 0.0003)
                ]);
        }
    }
}
