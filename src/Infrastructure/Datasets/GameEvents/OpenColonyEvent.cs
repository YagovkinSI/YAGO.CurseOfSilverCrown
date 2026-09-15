using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class OpenColonyEvent
    {
        private const string Id = GameEventConstants.OpenColony;

        private static readonly GameRequirement TurnRequirement =
            new GameRequirement(GameRequirementType.TurnNumberMoreThan, 2);

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [],
                        newEventCodes: [GameEventConstants.CodeOfLaws],
                        requirements: [TurnRequirement],
                        displayInfoResult: null) } };
            return new(
                code: Id,
                eventType: EventType.Quest,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides() => [
                GetSlide0()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Открытие колонии",
                imageName: ImageSet.RasShipyard,
                text: [
                    "Станция «Рассвет» ещё не завершена. Строительство, монтаж систем и внутренняя отделка займут ещё какое-то время.",
                    "Когда всё будет готово, комиссия Консорциума проведёт приёмку. Только после этого колония сможет официально " +
                    "открыться и принять первых жителей."
                ],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton(string.Empty, "Завершить приёмку станции", requirements: [TurnRequirement])]);
        }
    }
}