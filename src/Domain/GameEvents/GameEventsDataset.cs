using YAGO.World.Domain.Colonies.Companies;
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
                GetProductionCompany(),
                GetServiceCompany()
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
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, -0.02)
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -500)
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
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_EngineeringTeam, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_MiningBrigade, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_RehabilitationContingent, 0.02),
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -50)
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
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 0.001),
                    new KeyValueParameter(ColonyParameterNames.CurrentWeek, 0.001)
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -100),
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, -3)
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
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_EngineeringTeam, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_MiningBrigade, 0.02),
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_RehabilitationContingent, 0.02)
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, 100),
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, +1)
                ]);
        }

        private static GameEvent GetEngineeringTeam()
        {
            return new(
                id: 5,
                title: "Инженерная Команда",
                image: ImageSet.EngineeringTeam,
                text: ["К колонии присоединяется компания по добыче ресурсов. Это высокотехнологичная инженерная команда с передовым оборудованием AS и горсткой высокооплачиваемых специалистов."],
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyParameterNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Available, CompanyDataset.EngineeringTeam.ZonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, 0.03),
                    new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws_HighTax, double.MinValue),
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_EngineeringTeam, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, CompanyDataset.EngineeringTeam.ZonesOccupied),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, CompanyDataset.EngineeringTeam.SolarsIncome),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, CompanyDataset.EngineeringTeam.Population),
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
                requirements: [
                    new KeyValueParameter(ColonyParameterNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Available, CompanyDataset.MiningBrigade.ZonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, 0.03),
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_MiningBrigade, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, CompanyDataset.MiningBrigade.ZonesOccupied),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, CompanyDataset.MiningBrigade.SolarsIncome),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, CompanyDataset.MiningBrigade.Population),
                ]);
        }

        private static GameEvent GetRehabilitationContingent()
        {
            return new(
                id: 7,
                title: "Реабилитационный Контингент",
                image: ImageSet.RehabilitationContingent,
                text: ["К колонии присоединяется компания по добыче ресурсов. Они используют дешёвое оборудование и контингент должников. Дёшево, но рискованно."],
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyParameterNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Available, CompanyDataset.RehabilitationContingent.ZonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, 0.03),
                    new KeyValueParameter(ColonyParameterNames.Laws_CodeOfLaws_HighStandart, double.MinValue),
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Companies_Minning_RehabilitationContingent, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, CompanyDataset.RehabilitationContingent.ZonesOccupied),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, CompanyDataset.RehabilitationContingent.SolarsIncome),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, CompanyDataset.RehabilitationContingent.Population),
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
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.FirstWedding, double.MinValue),
                    new KeyValueParameter(ColonyParameterNames.CurrentWeek, 0.025),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, 0.0003)
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Economic_Reserves, -50),
                    new KeyValueParameter(ColonyParameterNames.Mood_Total, +5),
                    new KeyValueParameter(ColonyParameterNames.FirstWedding, 1)
                ]);
        }

        private static GameEvent GetProductionCompany()
        {
            return new(
                id: 9,
                title: "Новая Фабрика",
                image: ImageSet.ProductionCompany,
                text: ["К колонии присоединяется производственная компания. Новые колонисты будут производить продукцию компании на нашей станции."],
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Available, CompanyDataset.ProductionCompany.ZonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, 0.05),
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Industry_Production_Companies, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, CompanyDataset.ProductionCompany.ZonesOccupied),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, CompanyDataset.ProductionCompany.SolarsIncome),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, CompanyDataset.ProductionCompany.Population),
                ]);
        }

        private static GameEvent GetServiceCompany()
        {
            return new(
                id: 10,
                title: "Расширение сферы услуг",
                image: ImageSet.ServiceCompany,
                text: ["К колонии присоединяется компания по оказанию услуг. Новые колонисты будут оказывать услуги ростущему населению."],
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Available, CompanyDataset.ServiceCompany.ZonesOccupied),
                    new KeyValueParameter(ColonyParameterNames.Industry_Service_Need, 0),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyParameterNames.Attractiveness_Total, 0.01),
                    new KeyValueParameter(ColonyParameterNames.Industry_Service_Need, 0.5),
                ],
                parameterChanges: [
                    new KeyValueParameter(ColonyParameterNames.Industry_Service_Companies, 1),
                    new KeyValueParameter(ColonyParameterNames.AreaCapacity_Occupied, CompanyDataset.ServiceCompany.ZonesOccupied),
                    new KeyValueParameter(ColonyParameterNames.Economic_Budget_Balance, CompanyDataset.ServiceCompany.SolarsIncome),
                    new KeyValueParameter(ColonyParameterNames.Population_Total, CompanyDataset.ServiceCompany.Population),
                ]);
        }

    }
}
