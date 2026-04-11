using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class ProductionCompanyEvent
    {
        private const int _zonesOccupied = 5;

        public static GameEvent Get()
        {
            var id = "ProductionCompany";
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, _zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.02),
                ],
                episode: GetEpisode(id));
        }

        private static Episode GetEpisode(string id)
        {
            return new Episode(
                id: id,
                prologSlides: GetPrologSlides(),
                dilemma: GetDilemma());
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                new Slide(
                title: "Расширение производства",
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                    "Они обещают рабочие места и налоги. Новые колонисты будут производить продукцию компании на нашей станции."
                },
                parameters: [],
                buttonName: "Далее")];
        }

        private static Dilemma GetDilemma()
        {
            return new Dilemma(
                choice: [
                    GetChoice1(),
                    GetChoice2(),
                    GetChoice3()
                ],
                choiceLabel: ["Как поступим?"]);
        }

        private static Choice GetChoice1()
        {
            return new Choice(
                id: Guid.Parse("07414d28-603f-41c6-a442-e436433c2871"),
                title: "Согласиться",
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Компания откроет офис и пару десятков рабочих мест, привлекая новых колонистов. " +
                    "Производство не так выгодно как добыча ресурсов, но зато не иссякает со временем."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Production_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 25),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 25)]);
        }

        private static Choice GetChoice2()
        {
            return new Choice(
                id: Guid.Parse("d90806b2-9ad4-4821-bf19-b6470e5e9eb5"),
                title: "Отказать",
                imageName: ImageSet.ProductionCompany,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока сосредоточимся на том, что есть."
                },
                parameters: []);
        }

        private static Choice GetChoice3()
        {
            const int cost = 500;

            return new Choice(
                id: Guid.Parse("e92ab972-cf0b-4639-9b52-a509a3a9a040"),
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
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 50),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 25)],
                requirements: [
                    ChoiceRequirement.Cost(cost)]);
        }
    }
}
