using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class EngineeringTeamEvent
    {
        private const string Id = "EngineeringTeam";
        private const int ZonesOccupied = 3;
        private const int Cost = 600;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [
                    new RequirementsParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, ZonesOccupied),
                    new RequirementsParameter(ColonyStatNames.Laws_TaxLevel, 3, isTopThreshold: true),
                ],
                chanceDefault: 0,
                chanceModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
                ]);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { $"{Id}_1", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                    newQuests: [ ],
                    availableRequirements: [])},
                { $"{Id}_2", new GameEventChangeList(
                    colonyStats: [],
                    newQuests: [ ],
                    availableRequirements: [])},
                { $"{Id}_3", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, -Cost),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 40),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                    newQuests: [ ],
                    availableRequirements: [ActionAvailableRequirement.Cost(Cost)])}
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
                    title: "Расширение сферы добычи",
                    imageName: ImageSet.EngineeringTeam,
                    text: new string[]
                    {
                        "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                        "Компания будет заниматься добычей ресурсов на астероиде. Они обещают рабочие места и налоги."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                        SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию...")]),
                GetChoice1(changeList),
                GetChoice2(changeList),
                GetChoice3(changeList)
            ];
        }

        private static Slide GetChoice1(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Согласиться",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Компания откроет небольшой офис и создаст несколько рабочих мест для высокооплачиваемых специалистов." +
                    "Это высокотехнологичная инженерная команда с передовым оборудованием AS."
                },
                parameters: changeList[$"{Id}_1"].ColonyStats,
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
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока ресурсы останутся в недрах астероида."
                },
                parameters: changeList[$"{Id}_2"].ColonyStats,
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
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: changeList[$"{Id}_3"].ColonyStats,
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_3", availableRequirements: changeList[$"{Id}_3"].AvailableRequirements)]);
        }
    }
}
