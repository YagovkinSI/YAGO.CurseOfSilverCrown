using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class ProductionCompanyEvent
    {
        private const string Id = "ProductionCompany";
        private const int ZonesOccupied = 5;

        public static GameEvent Get()
        {
            return new(
                id: Id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, ZonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.02),
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
                    title: "Расширение производства",
                    imageName: ImageSet.ProductionCompany,
                    text: new string[]
                    {
                        "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                        "Они обещают рабочие места и налоги. Новые колонисты будут производить продукцию компании на нашей станции."
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
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Компания откроет офис и пару десятков рабочих мест, привлекая новых колонистов. " +
                    "Производство не так выгодно как добыча ресурсов, но зато не иссякает со временем."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Production_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 25),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 25)],
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
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока сосредоточимся на том, что есть."
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
            const int cost = 500;

            return new Slide(
                id: $"{Id}_3",
                title: "Открыть госкомпанию",
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -cost),
                    new KeyValueParameter(ColonyStatNames.Industry_Production_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, ZonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 50),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 25)],
                continueButtonName: "Далее",
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Согласиться..."),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отказать..."),
                    SlideButton.GetSetChoiceButton(Id, $"{Id}_3", availableRequirements: [ActionAvailableRequirement.Cost(cost)])]);
        }
    }
}
