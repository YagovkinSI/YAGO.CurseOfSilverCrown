using System;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    internal static class MainStreetDecoratingEvent
    {
        private const string Id = "MainStreetDecorating";

        public static GameEvent Get()
        {
            return new(
                id: Id,
                chanceDefault: int.MinValue,
                requirements: [],
                parameterModifiers: [],
                episode: GetEpisode());
        }

        private static Episode GetEpisode()
        {
            return new Episode(
                slides: [GetPrologSlides()],
                dilemma: GetDilemma());
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
                parameters: [],
                continueButtonName: "Далее",
                buttons: []);
        }

        private static Dilemma GetDilemma()
        {
            return new DilemmaSelect(
                choice: [
                    GetChoicePlants(),
                    GetChoicePublicWorks(),
                    GetChoiceSlideClear(),
                    GetChoiceSlideNothing()],
                choiceLabel: ["Что сделать с главной улицей?"]);
        }

        private static Choice GetChoicePlants()
        {
            return new Choice(
                id: $"{Id}_1",
                "Выделить бюджет на озеленение",
                ImageSet.GrayСorridor,
                [
                    "Через неделю в атриуме появятся первые растения."
                ],
                parameters: []);
        }

        private static Choice GetChoicePublicWorks()
        {
            return new Choice(
                id: $"{Id}_2",
                "Организовать субботник",
                ImageSet.GrayСorridor,
                [
                    "Колонисты сами покрасят стены и расставят самодельные кашпо."
                ],
                parameters: []);
        }

        private static Choice GetChoiceSlideClear()
        {
            return new Choice(
                id: $"{Id}_3",
                "Закрасить граффити и забыть",
                ImageSet.GrayСorridor,
                [
                    "Стены снова будут серые."
                ],
                parameters: []);
        }

        private static Choice GetChoiceSlideNothing()
        {
            return new Choice(
                id: $"{Id}_4",
                "Оставить как есть",
                ImageSet.GrayСorridor,
                [
                    "У правителя есть дела поважнее цветочков."
                ],
                parameters: []);
        }
    }
}
