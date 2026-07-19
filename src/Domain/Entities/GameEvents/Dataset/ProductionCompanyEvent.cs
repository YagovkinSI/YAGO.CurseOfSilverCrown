using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class ProductionCompanyEvent
    {
        private const string Id = "ProductionCompany";
        private const int ZonesOccupied = 15;
        private const int Cost = 7500;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, ZonesOccupied),
                ],
                chanceDefault: 0,
                chanceModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.02),
                ]);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { $"{Id}_1", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Industry_Production_Companies_Private, 3)],
                    newQuests: [ ],
                    requirements: [
                        RequirementsParameter.Zones(ZonesOccupied)])},
                { $"{Id}_2", new GameEventChangeList(
                    colonyStats: [],
                    newQuests: [ ],
                    requirements: [])},
                { $"{Id}_3", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, -Cost),
                        new KeyValueParameter(ColonyStatNames.Industry_Production_Companies_StateOwned, 3)],
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
                    title: "Расширение производства",
                    imageName: ImageSet.ProductionCompany,
                    text: new string[]
                    {
                        "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                        "Они обещают рабочие места и налоги. Новые колонисты будут производить продукцию компании на нашей станции."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                        SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию...")]),

                GetChoice1(changeList),
                GetChoice2(changeList),
                GetChoice3(changeList)];
        }

        private static Slide GetChoice1(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Согласиться",
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Компания откроет офис и пару десятков рабочих мест, привлекая новых колонистов. " +
                    "Производство не так выгодно как добыча ресурсов, но зато не иссякает со временем."
                },
                parameters: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_1")]);
        }

        private static Slide GetChoice2(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Отказать",
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока сосредоточимся на том, что есть."
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
                imageName: ImageSet.ProductionCompany,
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
