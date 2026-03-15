using YAGO.World.Domain.Common;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
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
            var id = "MinersRevolt";
            return new(
                id: id,
                chanceDefault: 1,
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Mood_Total, -0.02)
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Бунт рудокопов",
                        illustration: ImageSet.MinersRevolt,
                        text: new string[]
                        {
                            "Недовольство условиями и долгой изоляцией достигло пика. " +
                            "Группа рудокопов захватила склад скафандров и шлюз, " +
                            "угрожая разгерметизацией корабля, если их требования не будут выполнены.",
                            "Прибыль ушла на подавление мятежа и ремонт."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, -500)
                        ])])
                );
        }

        private static GameEvent GetLossOfCargo()
        {
            var id = "LossOfCargo";
            return new(
                id: id,
                chanceDefault: 0.15,
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, -0.01),
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Потеря груза",
                        illustration: ImageSet.LossOfCargo,
                        text: new string[]
                        {
                            "В результате сбоя магнитного захвата манипулятора ценнейший " +
                            "монолитный фрагмент астероида, богатый редкоземельными металлами, " +
                            "вырвался и улетел в космическую пустоту.",
                            "Попытки его вернуть сорвали график добычи.",
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, -50)
                        ])])
                );
        }

        private static GameEvent GetFireInResidentialArea()
        {
            var id = "FireInResidentialArea";
            return new(
                id: id,
                chanceDefault: -0.1,
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Population_Total, 0.0005),
                    new KeyValueParameter(ColonyStatNames.CurrentWeek, 0.0005)
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Замыкание в жилом секторе",
                        illustration: ImageSet.FireInResidentialArea,
                        text: new string[]
                        {
                            "Из-за перегрузки проводки в жилом модуле случился пожар. " +
                            "Отсек залит пеной, оборудование требует замены. " +
                            "Эвакуированных колонистов разместили в соседних отсеках.",
                            "Непредвиденное соседство порождает напряжённость и недовольство.",
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, -100),
                            new KeyValueParameter(ColonyStatNames.Mood_Total, -3)
                        ])])
                );
        }

        private static GameEvent GetGoldMine()
        {
            var id = "GoldMine";
            return new(
                id: id,
                chanceDefault: 0.15,
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 0.01)
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "«Золотая жила»",
                        illustration: ImageSet.GoldMine,
                        text: new string[]
                        {
                            "Вскрыв новый участок, геологи наткнулись на компактное месторождение " +
                            "платиноидов высокой чистоты. Его удалось быстро и безопасно извлечь, " +
                            "что резко увеличило стоимость груза.",
                            "На корабле царит приподнятое настроение."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, 100),
                            new KeyValueParameter(ColonyStatNames.Mood_Total, +1)
                        ])])
                );
        }

        private static GameEvent GetEngineeringTeam()
        {
            var id = "EngineeringTeam";
            const int zonesOccupied = 3;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied)
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
                    new KeyValueParameter(ColonyStatNames.Laws_CodeOfLaws_HighTax, double.MinValue),
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Инженерная Команда",
                        illustration: ImageSet.EngineeringTeam,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по добыче ресурсов. " +
                            "Это высокотехнологичная инженерная команда с передовым оборудованием AS " +
                            "и горсткой высокооплачиваемых специалистов."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 10),
                        ])])
                );
        }

        private static GameEvent GetMiningBrigade()
        {
            var id = "MiningBrigade";
            const int zonesOccupied = 3;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.04),
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Горнодобывающая Бригада",
                        illustration: ImageSet.MiningBrigade,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по добыче ресурсов. " +
                            "Бригада лицензированных рудокопов с надёжным оборудованием, " +
                            "коих многие тысячи на поясе."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 30),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 15),
                        ])])
                );
        }

        private static GameEvent GetRehabilitationContingent()
        {
            var id = "RehabilitationContingent";
            const int zonesOccupied = 4;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
                    new KeyValueParameter(ColonyStatNames.Laws_CodeOfLaws_HighStandart, double.MinValue),
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Реабилитационный Контингент",
                        illustration: ImageSet.RehabilitationContingent,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по добыче ресурсов. " +
                            "Они используют дешёвое оборудование и контингент должников. " +
                            "Дёшево, но рискованно."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 50),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 30),
                        ])])
                );
        }

        private static GameEvent GetFirstWedding()
        {
            var id = "FirstWedding";
            return new(
                id: id,
                chanceDefault: -0.10,
                requirements: [],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.FirstWedding, double.MinValue),
                    new KeyValueParameter(ColonyStatNames.CurrentWeek, 0.025),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 0.0003)
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Первая свадьба",
                        illustration: ImageSet.FirstWedding,
                        text: new string[]
                        {
                            "Сегодня вы получили официальный запрос от двоих резидентов: инженера и пилота грузового челнока. Они просят вас, как капитана станции, провести церемонию бракосочетания. В отсутствие ЗАГСа такая практика разрешена Орбитальным Правительством Земли — запись в бортовом журнале имеет юридическую силу.",
                            "Церемония проходит в обзорном зале. Жених в строгом костюме, невеста в платье, заказанном с Цереры около месяца назад. Почти всё свободное население станции собралось полукругом, с бокалами синтезированного игристого. Вы произносите короткую речь о том, что в пустоте человеческая связь становится абсолютной ценностью. Жених и невеста обмениваются кольцами. Вы объявляете их супругами и вносите запись в журнал.",
                            "Позже, когда гости расходятся, вы смотрите на мигающее уведомление: запись принята реестром ОПЗ. Запись номер один. Первая семья вашей станции. Ваша станция только что обрела нечто большее, чем руду. Она обрела корни."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, -50),
                            new KeyValueParameter(ColonyStatNames.Mood_Total, +5),
                            new KeyValueParameter(ColonyStatNames.FirstWedding, 1)
                        ])])
                );
        }

        private static GameEvent GetProductionCompany()
        {
            var id = "ProductionCompany";
            const int zonesOccupied = 5;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.02),
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Новая Фабрика",
                        illustration: ImageSet.ProductionCompany,
                        text: new string[]
                        {
                            "К колонии присоединяется производственная компания. " +
                            "Новые колонисты будут производить продукцию компании на нашей станции."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Production_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 25),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 25),
                        ])])
                );
        }

        private static GameEvent GetServiceCompany()
        {
            var id = "ServiceCompany";
            const int zonesOccupied = 3;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Need, 0),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.01),
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Need, 0.5),
                ],
                episode: new Episode(
                    id: id,
                    slides: [ new Slide(
                        title: "Расширение сферы услуг",
                        illustration: ImageSet.ServiceCompany,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по оказанию услуг. " +
                            "Новые колонисты будут оказывать услуги ростущему населению."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Service_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 10),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 10),
                        ])])
                );
        }
    }
}
