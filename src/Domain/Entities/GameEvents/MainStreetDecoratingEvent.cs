using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    internal static class MainStreetDecoratingEvent
    {
        public static GameEvent Get()
        {
            var id = "MainStreetDecorating";
            return new(
                id: id,
                chanceDefault: int.MinValue,
                requirements: [],
                parameterModifiers: [],
                episode: GetEpisode(id));
        }

        private static Episode GetEpisode(string id)
        {
            return new Episode(
                id: id,
                prologSlides: [GetPrologSlides()],
                choice: [
                    GetChoicePlants(),
                    GetChoicePublicWorks(),
                    GetChoiceSlideClear(),
                    GetChoiceSlideNothing()],
                choiceLabel: "Что сделать с главной улицей?");
        }

        private static Slide GetPrologSlides()
        {
            return new Slide(
                "Главная улица",
                ImageSet.GrayСorridor,
                [
                    "Прогуливаясь по центральному атриуму, вы замечаете, как серы и унылы стены. Колонисты проходят мимо, не поднимая глаз. Кто-то написал мелом \"Здесь мог бы быть сад\".",
                    "Главный инженер предлагает заняться благоустройством."
                ],
                parameters: []);
        }

        private static Slide GetChoicePlants()
        {
            return new Slide(
                "Выделить бюджет на озеленение",
                ImageSet.GrayСorridor,
                [
                    "Через неделю в атриуме появятся первые растения."
                ],
                parameters: []);
        }

        private static Slide GetChoicePublicWorks()
        {
            return new Slide(
                "Организовать субботник",
                ImageSet.GrayСorridor,
                [
                    "Колонисты сами покрасят стены и расставят самодельные кашпо."
                ],
                parameters: []);
        }

        private static Slide GetChoiceSlideClear()
        {
            return new Slide(
                "Закрасить граффити и забыть",
                ImageSet.GrayСorridor,
                [
                    "Стены снова будут серые."
                ],
                parameters: []);
        }

        private static Slide GetChoiceSlideNothing()
        {
            return new Slide(
                "Оставить как есть",
                ImageSet.GrayСorridor,
                [
                    "У правителя есть дела поважнее цветочков."
                ],
                parameters: []);
        }
    }
}
