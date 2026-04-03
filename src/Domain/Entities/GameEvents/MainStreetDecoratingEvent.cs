using System;
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
                id: Guid.Empty,
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
                id: Guid.Parse("2d8c247e-d018-47ac-8e0b-993868085b60"),
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
                id: Guid.Parse("fa7efc89-8cc7-4696-9289-0e0fcd9d2173"),
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
                id: Guid.Parse("9e58a879-61b3-4abd-a6f5-81d245dccb0b"),
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
                id: Guid.Parse("87d02a18-98fb-42a3-8619-81893980587b"),
                "Оставить как есть",
                ImageSet.GrayСorridor,
                [
                    "У правителя есть дела поважнее цветочков."
                ],
                parameters: []);
        }
    }
}
