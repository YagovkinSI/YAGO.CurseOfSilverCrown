using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    internal static class GameEventsDataset
    {
        public static IReadOnlyList<GameEvent> All => [
            StartColonyEvent.Get(),
            SkipPrologueEvent.Get(),
            MvpQuest.Get(),

            GetMinersRevolt(),
            GetLossOfCargo(),
            GetFireInResidentialArea(),
            GetGoldMine(),
            GetFirstWedding(),
            GetCredit()];

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
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [
                    new GameRequirement(GameRequirementType.MoodLessThan, GameEventConstants.TrustWithRevolt)
                ],
                chanceDefault: 0.1,
                chanceModifiers: [
                    new ProbabilityModifier(
                        new GameRequirement(GameRequirementType.MoodLessThan, GameEventConstants.TrustWithRevoltCritical),
                        ProbabilityModifierType.Multiplicative,
                        value: 3)]);
            var changesWithoutChoice = new GameAction([
                    new GameEffect(GameEffectType.AddSolars, -300),
                    new GameEffect(GameEffectType.AddMood, 15),
                ],
                newEventCodes: []);
            var changeList = new Dictionary<string, GameAction>() { { "#default", changesWithoutChoice } };
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
                        parameterChanges: changesWithoutChoice.Effects,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetLossOfCargo()
        {
            var id = "LossOfCargo";
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [
                    new GameRequirement(GameRequirementType.BuildingMiningMoreThan, 3)],
                chanceDefault: 0.1,
                chanceModifiers: []);
            var changesWithoutChoice = new GameAction([
                    new GameEffect(GameEffectType.AddSolars, -50)],
                newEventCodes: []);
            var changeList = new Dictionary<string, GameAction>() { { "#default", changesWithoutChoice } };
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
                        parameterChanges: changesWithoutChoice.Effects,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetFireInResidentialArea()
        {
            var id = "FireInResidentialArea";
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [
                    new GameRequirement(GameRequirementType.ModulesUsedMoreThan, 80)],
                chanceDefault: 0.1,
                chanceModifiers: []);
            var changesWithoutChoice = new GameAction([
                    new GameEffect(GameEffectType.AddSolars, -200),
                    new GameEffect(GameEffectType.AddMood, -3)],
                newEventCodes: []);
            var changeList = new Dictionary<string, GameAction>() { { "#default", changesWithoutChoice } };
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
                        parameterChanges: changesWithoutChoice.Effects,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetGoldMine()
        {
            var id = "GoldMine";
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [
                    new GameRequirement(GameRequirementType.BuildingMiningMoreThan, 3)],
                chanceDefault: 0.1,
                chanceModifiers: []);
            var changesWithoutChoice = new GameAction([
                    new GameEffect(GameEffectType.AddSolars, 100),
                    new GameEffect(GameEffectType.AddMood, 1)],
                newEventCodes: []);
            var changeList = new Dictionary<string, GameAction>() { { "#default", changesWithoutChoice } };
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
                        parameterChanges: changesWithoutChoice.Effects,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetFirstWedding()
        {
            var id = "FirstWedding";
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [
                    new GameRequirement(GameRequirementType.DoesntHaveAchievement, achievement: AchievementConstants.FirstWedding),
                    new GameRequirement(GameRequirementType.ModulesUsedMoreThan, 40)],
                chanceDefault: 0.1,
                chanceModifiers: []);
            var changesWithoutChoice = new GameAction([
                    new GameEffect(GameEffectType.AddSolars, -20),
                    new GameEffect(GameEffectType.AddMood, 5),
                    new GameEffect(GameEffectType.SetAchievement, achievement: AchievementConstants.FirstWedding)],
                newEventCodes: []);
            var changeList = new Dictionary<string, GameAction>() { { "#default", changesWithoutChoice } };
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
                        parameterChanges: [],
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
                        parameterChanges: changesWithoutChoice.Effects,
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }

        private static GameEvent GetCredit()
        {
            var id = "GetCredit";
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [
                    new GameRequirement(GameRequirementType.SolarsLessThan, 500)],
                chanceDefault: 0.7,
                chanceModifiers: []);
            var changesWithoutChoice = new GameAction(
                effects: [],
                newEventCodes: []);
            var changeList = new Dictionary<string, GameAction>() { { "#default", changesWithoutChoice } };
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
                        parameterChanges: [],
                        buttons: [
                            SlideButton.GetCloseNewsButton(id)])],
                changeList);
        }
    }
}
