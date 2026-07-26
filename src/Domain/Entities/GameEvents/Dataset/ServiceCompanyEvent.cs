using System.Collections.Generic;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class ServiceCompanyEvent
    {
        private const string Id = "ServiceCompany";
        private const int ZonesOccupied = 10;
        private const int Cost = 3000;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [
                    new RequirementsParameter(StateKey.ModulesFree, ZonesOccupied),
                    new RequirementsParameter(StateKey.ServiceNeed, 0),
                ],
                chanceDefault: 0,
                chanceModifiers: [
                    new KeyValueParameter(StateKey.Attractiveness, 0.01),
                    new KeyValueParameter(StateKey.ServiceNeed, 0.5),
                ]);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { $"{Id}_1", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(StateKey.BuildingsServicePrivate, 3)],
                    newQuests: [ ],
                    requirements: [
                        RequirementsParameter.Zones(ZonesOccupied)])},
                { $"{Id}_2", new GameEventChangeList(
                    colonyStats: [],
                    newQuests: [ ],
                    requirements: [])},
                { $"{Id}_3", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(StateKey.BuildingsServiceState, 3),
                        new KeyValueParameter(StateKey.SolarsCurrent, -Cost)],
                    newQuests: [ ],
                    requirements: [
                        RequirementsParameter.Cost(Cost),
                        RequirementsParameter.Zones(ZonesOccupied)])}
            };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList),
                changeList);
        }

        private static Episode GetEpisode(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Episode(
                slides: GetPrologSlides(changeList));
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameEventChangeList> changeList)
        {
            return [
                new Slide(
                    id: $"{Id}_0",
                    title: "Расширение сферы услуг",
                    imageName: ImageSet.ServiceCompany,
                    text: new string[]
                    {
                        "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                        "Компания будет оказывать услуги растущему населению. Они обещают рабочие места и налоги."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                        SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию...")]),

                GetChoice1(),
                GetChoice2(),
                GetChoice3(changeList)];
        }

        private static Slide GetChoice1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Согласиться",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Компания откроет небольшой офис и создаст несколько рабочих мест, привлекая новых колонистов. " +
                    "Сфера услуг не приносит много прибыли ни компании, ни государству, но они необходимы для жизни колонии."
                },
                parameters: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_1")]);
        }

        private static Slide GetChoice2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Отказать",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока колонистам придётся подождать."
                },
                parameters: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_2")]);
        }

        private static Slide GetChoice3(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Открыть госкомпанию",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_3", requirements: changeList[$"{Id}_3"].Requirements)]);
        }
    }
}
