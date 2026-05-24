using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class EngineeringTeamEvent
    {
        private const string Id = "EngineeringTeam";
        private const int ZonesOccupied = 3;

        public static GameEvent Get()
        {
            return new(
                id: Id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, ZonesOccupied),
                    new RequirementsParameter(ColonyStatNames.Laws_TaxLevel, 3, isTopThreshold: true),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
                ],
                episode: GetEpisode());
        }

        private static Episode GetEpisode()
        {
            return new Episode(
                slides: GetPrologSlides());
        }

        private static Slide[] GetPrologSlides()
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
                    continueButtonName: "Далее",
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                        SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию...")]),
                GetChoice1(),
                GetChoice2(),
                GetChoice3()
            ];
        }

        private static Choice GetChoice1()
        {
            return new Choice(
                id: $"{Id}_1",
                title: "Согласиться",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Компания откроет небольшой офис и создаст несколько рабочих мест для высокооплачиваемых специалистов." +
                    "Это высокотехнологичная инженерная команда с передовым оборудованием AS."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию..."),
                    SlideButton.GetSetChoiceButton($"{Id}_1")]);
        }

        private static Choice GetChoice2()
        {
            return new Choice(
                id: $"{Id}_2",
                title: "Отказать",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока ресурсы останутся в недрах астероида."
                },
                parameters: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию..."),
                    SlideButton.GetSetChoiceButton($"{Id}_2")]);
        }

        private static Choice GetChoice3()
        {
            const int cost = 600;

            return new Choice(
                id: $"{Id}_3",
                title: "Открыть госкомпанию",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -cost),
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 40),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                requirements: [
                    ChoiceRequirement.Cost(cost)],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetSetChoiceButton($"{Id}_3")]);
        }
    }
}
