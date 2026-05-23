using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class MiningBrigadeEvent
    {
        private const int _zonesOccupied = 3;

        public static GameEvent Get()
        {
            var id = "MiningBrigade";
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, _zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.04),
                ],
                episode: GetEpisode(id));
        }

        private static Episode GetEpisode(string id)
        {
            return new Episode(
                id: id,
                title: "Расширение сферы добычи",
                slides: GetPrologSlides(),
                dilemma: GetDilemma());
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                new Slide(
                title: "Расширение сферы добычи",
                imageName: ImageSet.MiningBrigade,
                text: new string[]
                {
                    "Группа предпринимателей предлагает открыть в колонии новую компанию. " +
                    "Компания будет заниматься добычей ресурсов на астероиде. Они обещают рабочие места и налоги."
                },
                parameters: [],
                continueButtonName: "Далее")];
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
                id: Guid.Parse("40757f7f-65c9-463b-bc56-a2c7138fa128"),
                title: "Согласиться",
                imageName: ImageSet.MiningBrigade,
                text: new string[]
                {
                    "Компания откроет небольшой офис и наймёт бригаду лицензированных рудокопов " +
                    "с надёжным оборудованием коих многие тысячи на Поясе."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 30),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 15)]);
        }

        private static Choice GetChoice2()
        {
            return new Choice(
                id: Guid.Parse("83d0a14d-90b7-4c78-a034-fdf82139b794"),
                title: "Отказать",
                imageName: ImageSet.MiningBrigade,
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
                id: Guid.Parse("8f687c42-7e31-45ad-83dd-6465b6d67d6e"),
                title: "Открыть госкомпанию",
                imageName: ImageSet.MiningBrigade,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -cost),
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 60),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 15)],
                requirements: [
                    ChoiceRequirement.Cost(cost)]);
        }
    }
}
