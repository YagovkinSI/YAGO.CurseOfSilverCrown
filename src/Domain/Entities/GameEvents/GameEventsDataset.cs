using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents.Dataset;
using YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public static class GameEventsDataset
    {
        public static GameEvent Get(string eventId)
        {
            return GetAll().Single(x => x.Id == eventId);
        }

        public static GameEvent[] GetAll()
        {
            var allEvents = new List<GameEvent>()
            {
                ColonyNameEvent.Get(),
                SkipPrologueEvent.Get(),

                GetMinersRevolt(),
                GetLossOfCargo(),
                GetFireInResidentialArea(),
                GetGoldMine(),
                GetFirstWedding(),
                MainStreetDecoratingEvent.Get(),

                ServiceCompanyEvent.Get(),
                EngineeringTeamEvent.Get(),
                MiningBrigadeEvent.Get(),
                RehabilitationContingentEvent.Get(),
                ProductionCompanyEvent.Get()
            };
            return allEvents.ToArray();
        }

        private static GameEvent GetMinersRevolt()
        {
            var id = "MinersRevolt";
            return new(
                id: id,
                chanceDefault: 0.1,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.Mood_Total, GameEventsConstants.TrustWithRevolt, isTopThreshold: true)],
                parameterModifiers: [],
                episode: new Episode(
                    id: id,
                    slides: [
                        new Slide(
                            title: "Бунт рудокопов",
                            imageName: ImageSet.MinersRevolt,
                            text: new string[]
                            {
                                "Недовольство условиями и долгой изоляцией достигло пика. " +
                                "Группа рудокопов захватила склад скафандров и шлюз, " +
                                "угрожая разгерметизацией корабля, если их требования не будут выполнены.",
                                "Прибыль ушла на подавление мятежа и ремонт."
                            },
                            parameters: [
                                new KeyValueParameter(ColonyStatNames.Economic_Reserves, -500),
                                new KeyValueParameter(ColonyStatNames.Mood_Total, +5),
                            ],
                            continueButtonName: "Далее")],
                    dilemma: null)
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
                    slides: [
                        new Slide(
                            title: "Потеря груза",
                            imageName: ImageSet.LossOfCargo,
                            text: new string[]
                            {
                                "В результате сбоя магнитного захвата манипулятора ценнейший " +
                                "монолитный фрагмент астероида, богатый редкоземельными металлами, " +
                                "вырвался и улетел в космическую пустоту.",
                                "Попытки его вернуть сорвали график добычи.",
                            },
                            parameters: [
                                new KeyValueParameter(ColonyStatNames.Economic_Reserves, -50)
                            ],
                            continueButtonName: "Далее")],
                    dilemma: null)
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
                    slides: [
                        new Slide(
                            title: "Замыкание в жилом секторе",
                            imageName: ImageSet.FireInResidentialArea,
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
                            ],
                            continueButtonName : "Далее")],
                    dilemma: null)
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
                    slides: [
                        new Slide(
                            title: "«Золотая жила»",
                            imageName: ImageSet.GoldMine,
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
                            ],
                            continueButtonName: "Далее")],
                    dilemma: null)
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
                    slides: [
                        new Slide(
                            title: "Первая свадьба",
                            imageName: ImageSet.FirstWedding,
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
                            ],
                            continueButtonName: "Далее")],
                    dilemma: null)
                );
        }
    }
}
