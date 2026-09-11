using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class NewYorkStrikeEvent
    {
        private const string Id = GameEventConstants.NewYorkStrike;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeEarth2070),],
                        newEventCodes: [],
                        requirements: [],
                        displayInfoResult: null) } };
            return new(
                code: Id,
                eventType: EventType.Default,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides() => [
                GetSlide0(),
                GetSlide1()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Забастовка в Нью-Йорке",
                imageName: ImageSet.NewYorkStrike,
                text: new string[]
                {
                    "Забастовка в Нью-Йорке: сотни тысяч на улицах",
                    "Более 300 000 человек вышли на улицы. Причина — сокращение ещё двух тысяч рабочих в порту Нью-Йорка " +
                    "и Нью-Джерси. В начале года порт ввёл полностью автономную систему разгрузки контейнеров.",
                    "Профсоюзы требуют сохранить рабочие места и расширить программы социально-полезного труда. " +
                    "Администрация порта заявляет, что автоматизация необходима для конкурентоспособности."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Забастовка в Нью-Йорке",
                imageName: ImageSet.NewYorkStrike,
                text: new string[]
                {
                    "Это уже третья крупная забастовка в США за год. В марте в Бостоне аналогичный протест закончился " +
                    "столкновениями с полицией — есть пострадавшие.",
                    "Портовое управление отказалось от комментариев. Аналитики отмечают: роботизация продолжает вытеснять " +
                    "людей.",
                    "С каждым годом протестов против автоматизации в мире становится всё больше. " +
                    "По данным ООН, уровень безработицы приближается к 9,5%."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Закрыть")]);
        }
    }
}
