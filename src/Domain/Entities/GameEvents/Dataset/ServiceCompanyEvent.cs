using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class ServiceCompanyEvent
    {
        private const int _zonesOccupied = 3;

        public static GameEvent Get()
        {
            var id = "ServiceCompany";
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, _zonesOccupied),
                    new RequirementsParameter(ColonyStatNames.Industry_Service_Need, 0),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.01),
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Need, 0.5),
                ],
                episode: GetEpisode(id));
        }

        private static Episode GetEpisode(string id)
        {
            return new Episode(
                slides: GetPrologSlides(),
                dilemma: GetDilemma());
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                new Slide(
                title: "Расширение сферы услуг",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                    "Компания будет оказывать услуги растущему населению. Они обещают рабочие места и налоги."
                },
                parameters: [],
                continueButtonName: "Далее",
                buttons: [])];
        }

        private static Dilemma GetDilemma()
        {
            return new DilemmaSelect(
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
                id: Guid.Parse("003b8f59-d0b9-4f05-be01-fa2a7a89ef65"),
                title: "Согласиться",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Компания откроет небольшой офис и создаст несколько рабочих мест, привлекая новых колонистов. " +
                    "Сфера услуг не приносит много прибыли ни компании, ни государству, но они необходимы для жизни колонии."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 10),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)]);
        }

        private static Choice GetChoice2()
        {
            return new Choice(
                id: Guid.Parse("3a6ee9cd-0fcc-4378-b499-16e7cff5ce98"),
                title: "Отказать",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока колонистам придётся подождать."
                },
                parameters: []);
        }

        private static Choice GetChoice3()
        {
            const int cost = 200;

            return new Choice(
                id: Guid.Parse("f622d40b-7f2c-409e-b362-ae84c9080392"),
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
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                requirements: [
                    ChoiceRequirement.Cost(cost)]);
        }
    }
}
