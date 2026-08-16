using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class GameEventsDataset
    {
        public static IReadOnlyList<GameEvent> All => [
            ColonyNameEvent.Get(),
            SkipPrologueEvent.Get(),
            MvpQuest.Get(),

            GetMinersRevolt(),
            GetLossOfCargo(),
            GetFireInResidentialArea(),
            GetGoldMine(),
            GetFirstWedding(),
            GetCredit(),
            MainStreetDecoratingEvent.Get()];

        public static GameEvent Get(string eventCode)
        {
            return All.Single(x => x.Code == eventCode);
        }

        public static IEnumerable<GameEvent> Find(params string[] questIds)
        {
            return All.Where(x => questIds.Contains(x.Code));
        }

        private static GameEvent GetMinersRevolt()
        {
            var id = "MinersRevolt";
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [
                    new RequirementsParameter(StateKey.MoodCurrent, GameEventsConstants.TrustWithRevolt, isTopThreshold: true)
                ],
                chanceDefault: 0.1,
                chanceModifiers: []);
            var changesWithoutChoice = new GameEventChangeList([
                    new KeyValueParameter(StateKey.SolarsCurrent, -300),
                    new KeyValueParameter(StateKey.MoodCurrent, +15),
                ],
                newQuests: []);
            var changeList = new Dictionary<string, GameEventChangeList>() { { "#end", changesWithoutChoice } };
            return new(
                code: id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: [
                    new Slide(
                        id: $"{id}_0",
                        title: "Бунт шахтёров",
                        imageName: ImageSet.MinersRevolt,
                        text: new string[]
                        {
                            "Недовольство условиями и долгой изоляцией достигло пика. " +
                            "Группа шахтёров захватила склад скафандров и шлюз, " +
                            "угрожая разгерметизацией станции, если их требования не будут выполнены.",
                            "Прибыль ушла на подавление мятежа и ремонт."
                        },
                        parameters: changesWithoutChoice.ColonyStats,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetLossOfCargo()
        {
            var id = "LossOfCargo";
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: 0.15,
                chanceModifiers: [
                    new KeyValueParameter(StateKey.MiningSlotsFree, -0.01),]);
            var changesWithoutChoice = new GameEventChangeList([
                    new KeyValueParameter(StateKey.SolarsCurrent, -200)
                ],
                newQuests: []);
            var changeList = new Dictionary<string, GameEventChangeList>() { { "#end", changesWithoutChoice } };
            return new(
                code: id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: [
                    new Slide(
                        id: $"{id}_0",
                        title: "Сбой на руднике",
                        imageName: ImageSet.RegularTurn,
                        text: new string[]
                        {
                            "На одном из модулей добычи вышел из строя вибрационный бур — заклинило привод. " +
                            "Пока инженеры разбирались с механизмом, смена потеряла почти двое суток. " +
                            "Вдобавок вскрытая жила оказалась тощей: руда с низким содержанием металла, " +
                            "которую даже перерабатывать невыгодно. Доходы от добычи временно сократились.",
                        },
                        parameters: changesWithoutChoice.ColonyStats,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetFireInResidentialArea()
        {
            var id = "FireInResidentialArea";
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: -0.1,
                chanceModifiers: [
                    new KeyValueParameter(StateKey.Population, 0.0005),
                    new KeyValueParameter(StateKey.TurnsCurrent, 0.0005)
                ]);
            var changesWithoutChoice = new GameEventChangeList([
                    new KeyValueParameter(StateKey.SolarsCurrent, -1000),
                    new KeyValueParameter(StateKey.MoodCurrent, -3)
                ],
                newQuests: []);
            var changeList = new Dictionary<string, GameEventChangeList>() { { "#end", changesWithoutChoice } };
            return new(
                code: id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: [
                    new Slide(
                        id: $"{id}_0",
                        title: "Замыкание в жилом секторе",
                        imageName: ImageSet.FireInResidentialArea,
                        text: new string[]
                        {
                            "В одном из жилых модулей произошло короткое замыкание. Система пожаротушения сработала штатно, " +
                            "но отсек надолго вышел из строя. Колонистов пришлось расселить по соседним блокам — " +
                            "теснота и отсутствие личного пространства уже вызывают недовольство.",
                        },
                        parameters: changesWithoutChoice.ColonyStats,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetGoldMine()
        {
            var id = "GoldMine";
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: 0.15,
                chanceModifiers: [
                    new KeyValueParameter(StateKey.MiningSlotsFree, 0.01)
                ]);
            var changesWithoutChoice = new GameEventChangeList([
                    new KeyValueParameter(StateKey.SolarsCurrent, 300),
                    new KeyValueParameter(StateKey.MoodCurrent, +1)
                ],
                newQuests: []);
            var changeList = new Dictionary<string, GameEventChangeList>() { { "#end", changesWithoutChoice } };
            return new(
                code: id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: [
                    new Slide(
                        id: $"{id}_0",
                        title: "«Золотая жила»",
                        imageName: ImageSet.GoldMine,
                        text: new string[]
                        {
                            "Разведочный бур вскрыл неожиданно мощный карман с высоким содержанием платиноидов. " +
                            "Руда пошла густая, чистая — таких показателей не видели с прошлого сезона. " +
                            "Перерабатывающий модуль работал на полной мощности, и к концу недели трюмы заметно потяжелели."
                        },
                        parameters: changesWithoutChoice.ColonyStats,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetFirstWedding()
        {
            var id = "FirstWedding";
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: -0.5,
                chanceModifiers: [
                    new KeyValueParameter(StateKey.FlagsFirstWedding, double.MinValue),
                    new KeyValueParameter(StateKey.TurnsCurrent, 0.2),
                    new KeyValueParameter(StateKey.Population, 0.0003)
                ]);
            var changesWithoutChoice = new GameEventChangeList([
                    new KeyValueParameter(StateKey.SolarsCurrent, -20),
                    new KeyValueParameter(StateKey.MoodCurrent, +5),
                    new KeyValueParameter(StateKey.FlagsFirstWedding, 1)
                ],
                newQuests: []);
            var changeList = new Dictionary<string, GameEventChangeList>() { { "#end", changesWithoutChoice } };
            return new(
                code: id,
                eventType: EventType.Default,
                eventOccurrenceOptions,
                slides: [
                    new Slide(
                        id: $"{id}_0",
                        title: "Первая свадьба",
                        imageName: ImageSet.FirstWedding,
                        text: new string[]
                        {
                            "Сегодня вы получили официальный запрос от двоих резидентов: инженера и пилота грузового челнока. " +
                            "Они просят вас, как капитана станции, провести церемонию бракосочетания. " +
                            "В отсутствие ЗАГСа такая практика разрешена Орбитальным Правительством Земли — " +
                            "запись в бортовом журнале имеет юридическую силу."
                        },
                        parameters: [],
                        buttons: [
                            SlideButton.GetButtonToSlide($"{id}_1", "Провести церемонию")]),
                    new Slide(
                        id: $"{id}_1",
                        title: "Первая свадьба",
                        imageName: ImageSet.FirstWedding,
                        text: new string[]
                        {
                            "Церемония проходит в обзорном зале. Жених в строгом костюме, невеста в платье, " +
                            "заказанном с Цереры около месяца назад. Почти всё свободное население станции собралось полукругом, " +
                            "с бокалами синтезированного игристого. Вы произносите короткую речь о том, " +
                            "что в пустоте человеческая связь становится абсолютной ценностью. " +
                            "Жених и невеста обмениваются кольцами. Вы объявляете их супругами и вносите запись в журнал.",
                            "Позже, когда гости расходятся, вы смотрите на мигающее уведомление: запись принята реестром ОПЗ. " +
                            "Запись номер один. Первая семья вашей станции. Ваша станция только что обрела нечто большее, чем руду. " +
                            "Она обрела корни."
                        },
                        parameters: changesWithoutChoice.ColonyStats,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetCredit()
        {
            var id = "GetCredit";
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [
                    new RequirementsParameter(StateKey.SolarsCurrent, 2000, true)],
                chanceDefault: 1,
                chanceModifiers: [
                    new KeyValueParameter(StateKey.SolarsCurrent, -0.001)
                ]);
            var changesWithoutChoice = new GameEventChangeList(
                colonyStats: [],
                newQuests: []);
            var changeList = new Dictionary<string, GameEventChangeList>() { { "#end", changesWithoutChoice } };
            return new(
                code: id,
                eventType: EventType.Default,
                eventOccurrenceOptions,
                slides: [
                    new Slide(
                        id: $"{id}_0",
                        title: "«Казна пустеет»",
                        imageName: ImageSet.ConcEarchOffice,
                        text: new string[]
                        {
                            "Наша казна пустеет, правитель.",
                            "Если необходимы новые средства для инвестиций, то в меню Реформы можно взять " +
                            "дополнительную ссуду у Консорциума."
                        },
                        parameters: [],
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }
    }
}
