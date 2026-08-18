using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    internal static class MainStreetDecoratingEvent
    {
        private const string Id = "MainStreetDecorating";

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: int.MinValue,
                chanceModifiers: []);
            return new(
                code: Id,
                eventType: EventType.Default,
                eventOccurrenceOptions,
                slides: [
                    GetPrologSlides(),
                    GetChoicePlants(),
                    GetChoicePublicWorks(),
                    GetChoiceSlideClear(),
                    GetChoiceSlideNothing()]);
        }

        private static Slide GetPrologSlides()
        {
            return new Slide(
                id: $"{Id}_0",
                "Главная улица",
                ImageSet.GrayСorridor,
                [
                    "Прогуливаясь по центральному атриуму, вы замечаете, как серы и унылы стены. Колонисты проходят мимо, не поднимая глаз. Кто-то написал мелом \"Здесь мог бы быть сад\".",
                    "Главный инженер предлагает заняться благоустройством."
                ],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Озеленение..."),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Субботник..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Закрасить графити..."),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Ничего...")]);
        }

        private static Slide GetChoicePlants()
        {
            return new Slide(
                id: $"{Id}_1",
                "Выделить бюджет на озеленение",
                ImageSet.GrayСorridor,
                [
                    "Через неделю в атриуме появятся первые растения."
                ],
                parameterChanges: [],
                buttons: []);
        }

        private static Slide GetChoicePublicWorks()
        {
            return new Slide(
                id: $"{Id}_2",
                "Организовать субботник",
                ImageSet.GrayСorridor,
                [
                    "Колонисты сами покрасят стены и расставят самодельные кашпо."
                ],
                parameterChanges: [],
                buttons: []);
        }

        private static Slide GetChoiceSlideClear()
        {
            return new Slide(
                id: $"{Id}_3",
                "Закрасить граффити и забыть",
                ImageSet.GrayСorridor,
                [
                    "Стены снова будут серые."
                ],
                parameterChanges: [],
                buttons: []);
        }

        private static Slide GetChoiceSlideNothing()
        {
            return new Slide(
                id: $"{Id}_4",
                "Оставить как есть",
                ImageSet.GrayСorridor,
                [
                    "У правителя есть дела поважнее цветочков."
                ],
                parameterChanges: [],
                buttons: []);
        }
    }
}
