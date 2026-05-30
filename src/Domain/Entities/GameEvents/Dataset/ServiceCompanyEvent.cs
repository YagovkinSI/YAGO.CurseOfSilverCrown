using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class ServiceCompanyEvent
    {
        private const string Id = "ServiceCompany";
        private const int ZonesOccupied = 3;

        public static GameEvent Get()
        {
            return new(
                id: Id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, ZonesOccupied),
                    new RequirementsParameter(ColonyStatNames.Industry_Service_Need, 0),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.01),
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Need, 0.5),
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
                    title: "Расширение сферы услуг",
                    imageName: ImageSet.ServiceCompany,
                    text: new string[]
                    {
                        "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                        "Компания будет оказывать услуги растущему населению. Они обещают рабочие места и налоги."
                    },
                    parameters: [],
                    continueButtonName: "Далее",
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                        SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию...")]),

                GetChoice1(),
                GetChoice2(),
                GetChoice3()];
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
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 10),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                continueButtonName: "Далее",
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
                continueButtonName: "Далее",
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Открыть госкомпанию..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_2")]);
        }

        private static Slide GetChoice3()
        {
            const int cost = 200;

            return new Slide(
                id: $"{Id}_3",
                title: "Открыть госкомпанию",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -cost),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                continueButtonName: "Далее",
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_3", availableRequirements: [ActionAvailableRequirement.Cost(cost)])]);
        }
    }
}
