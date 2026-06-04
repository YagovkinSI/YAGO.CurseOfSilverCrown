using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class MvpQuest
    {
        private const string Id = nameof(MvpQuest);
        const int ActionPoints = 7;
        const int Cost = 10000;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [
                    new RequirementsParameter(ColonyStatNames.EpisodeCount, 0, isTopThreshold : true)
                ],
                chanceDefault: 1,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { "end", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.ActionPoints_Resourses, ActionPoints),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 120),
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, Cost)],
                    newQuests: [ ],
                    availableRequirements: [
                        ActionAvailableRequirement.ActionPoints(ActionPoints),
                        ActionAvailableRequirement.Cost(Cost),
                        new ActionAvailableRequirement(
                            new RequirementsParameter(ColonyStatNames.AreaCapacity_Occupied, 120),
                            "Занято мало пространства")])}
            };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList));
        }

        private static Episode GetEpisode(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Episode(
                slides: GetPrologSlides(changeList));
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameEventChangeList> changeList)
        {
            var id = nameof(MvpQuest);
            var name = "Резолют-206";

            return [
                new Slide(
                    id: $"{id}_0",
                    name,
                    ImageSet.Station_1,
                    [
                        "Станция Рассвет может иметь не более 140 жилых модулей и не более 1000 жителей. " +
                        "Когда её лимит будет подходить к концу нам нужно будет перейти на станцию следующего уровня.",
                        "Станция Резолют-206 имеет более широкое колько диаметром 2 километра и расчитано на 3000 жителей. " +
                        "Это дорогостоящий переход, но если мы планируем увеличивать колонию и далее, то об этом переходе не стоит забывать."],
                    parameters: changeList["end"].ColonyStats,
                    buttons: [
                        SlideButton.GetSetChoiceButton(
                            id,
                            dilemmaResolving: "Complete",
                            name: "Перейти на следующий уровень",
                            availableRequirements: changeList["end"].AvailableRequirements)])];
        }
    }
}
