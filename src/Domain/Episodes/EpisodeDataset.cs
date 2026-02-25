using System;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Episodes
{
    public static class EpisodeDataset
    {
        public static Episode Get(long episodeId)
        {
            return episodeId switch
            {
                1 => MainStreetDecorating(1),
                _ => throw new NotImplementedException()
            };
        }

        private static Episode MainStreetDecorating(long episodeId)
        {
            var slide = new Slide(
                "Главная улица",
                ImageSet.GrayСorridor,
                [
                    "Прогуливаясь по центральному атриуму, вы замечаете, как серы и унылы стены. Колонисты проходят мимо, не поднимая глаз. Кто-то написал мелом \"Здесь мог бы быть сад\".",
                    "Главный инженер предлагает заняться благоустройством."
                ],
                parameters: []);
            var choice1 = new Slide(
                "Выделить бюджет на озеленение",
                ImageSet.GrayСorridor,
                [
                    "Через неделю в атриуме появятся первые растения."
                ],
                parameters: []);
            var choice2 = new Slide(
                "Организовать субботник",
                ImageSet.GrayСorridor,
                [
                    "Колонисты сами покрасят стены и расставят самодельные кашпо."
                ],
                parameters: []);
            var choice3 = new Slide(
                "Закрасить граффити и забыть",
                ImageSet.GrayСorridor,
                [
                    "Стены снова будут серые."
                ],
                parameters: []);
            var choice4 = new Slide(
                "Оставить как есть",
                ImageSet.GrayСorridor,
                [
                    "У правителя есть дела поважнее цветочков."
                ],
                parameters: []);
            return new Episode(
                id: episodeId,
                [slide],
                "Что сделать с главной улицей?",
                [choice1, choice2, choice3, choice4]);
        }
    }
}
