using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class OpenColonyEvent
    {
        private const string Id = GameEventConstants.OpenColony;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.AddSolars, 10_000),
                            new GameEffect(GameEffectType.AddPublicDebt, 30_000),
                            new(GameEffectType.AddBuildingsAdministrativeState, 1),
                            new(GameEffectType.SetAchievement, code: AchievementConstants.ColonyOpen)
                        ],
                        newEventCodes: [
                            GameEventConstants.MvpQuest],
                        requirements: [],
                        displayInfoResult: GetEpilog()) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                GetSlide0()];
        }

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Открытие колонии",
                imageName: ImageSet.Station_1,
                text: [
                    "Станция «Рассвет» готова. Вы прибыли на борт.",
                    "Док нанял первых тридцать колонистов. Лиен проверила системы и подписала приёмку. Кассиус подготовил бюджет. " +
                    "Камилла уже разбирает почту.",
                    "Колония начинает работать."
                ],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton(string.Empty, "Завершить приёмку станции", requirements: [])]);
        }

        private static DisplayInfo GetEpilog()
        {
            return new(
            name: "Открытие колонии",
            ImageSet.Station_1,
            description:
            [
                "Теперь вы управляете станцией. Каждый ход приносит доход, события и новые возможности. Совет ждёт ваших решений.",
                "Управление станцией переходит в ваши руки."
            ]);
        }
    }
}