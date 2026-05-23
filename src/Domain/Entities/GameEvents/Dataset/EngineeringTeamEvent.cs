using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class EngineeringTeamEvent
    {
        private const int _zonesOccupied = 3;

        public static GameEvent Get()
        {
            var id = "EngineeringTeam";
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, _zonesOccupied),
                    new RequirementsParameter(ColonyStatNames.Laws_TaxLevel, 3, isTopThreshold: true),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
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
                title: "Расширение сферы добычи",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                    "Компания будет заниматься добычей ресурсов на астероиде. Они обещают рабочие места и налоги."
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
                id: Guid.Parse("f791580f-a7d0-4ad2-a8b9-7b82d7cfc1ab"),
                title: "Согласиться",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Компания откроет небольшой офис и создаст несколько рабочих мест для высокооплачиваемых специалистов." +
                    "Это высокотехнологичная инженерная команда с передовым оборудованием AS."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)]);
        }

        private static Choice GetChoice2()
        {
            return new Choice(
                id: Guid.Parse("d6d9be92-f257-4fe2-9923-3670f62e68e8"),
                title: "Отказать",
                imageName: ImageSet.EngineeringTeam,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока ресурсы останутся в недрах астероида."
                },
                parameters: []);
        }

        private static Choice GetChoice3()
        {
            const int cost = 600;

            return new Choice(
                id: Guid.Parse("38b8a486-8b55-4d75-8e87-e9ddc389b63c"),
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
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 40),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)],
                requirements: [
                    ChoiceRequirement.Cost(cost)]);
        }
    }
}
