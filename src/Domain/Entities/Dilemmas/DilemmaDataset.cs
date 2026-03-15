using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Dilemmas
{
    public static class DilemmaDataset
    {
        public static Episode Get(string episodeId)
        {
            return episodeId switch
            {
                "MainStreetDecorating" => MainStreetDecorating(),
                _ => throw new NotImplementedException()
            };
        }

        private static Episode MainStreetDecorating()
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
                id: "MainStreetDecorating",
                [slide],
                "Что сделать с главной улицей?",
                [choice1, choice2, choice3, choice4]);
        }
    }
}
