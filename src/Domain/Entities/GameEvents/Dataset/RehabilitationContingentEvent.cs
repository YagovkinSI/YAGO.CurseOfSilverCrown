using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset
{
    internal static class RehabilitationContingentEvent
    {
        private const int _zonesOccupied = 4;

        public static GameEvent Get()
        {
            var id = "RehabilitationContingent";
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new RequirementsParameter(ColonyStatNames.AreaCapacity_Available, _zonesOccupied),
                    new RequirementsParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 3, isTopThreshold: true)
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
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

        private static PrologueSlide[] GetPrologSlides()
        {
            return [
                new PrologueSlide(
                title: "Расширение сферы добычи",
                imageName: ImageSet.RehabilitationContingent,
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
                id: Guid.Parse("c8e5a401-6cea-4bdc-a364-5daa9e8b406a"),
                title: "Согласиться",
                imageName: ImageSet.RehabilitationContingent,
                text: new string[]
                {
                    "Компания откроет небольшой офис, закупит дешёвое оборудование и наймёт контингент должников. " +
                    "Дёшево, но рискованно."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 50),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 30)]);
        }

        private static Choice GetChoice2()
        {
            return new Choice(
                id: Guid.Parse("230b8464-71a6-4c6c-8a1a-2f9b401c3155"),
                title: "Отказать",
                imageName: ImageSet.RehabilitationContingent,
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
                id: Guid.Parse("a1f68358-87b1-4bb0-a393-3a8cdc4a9a43"),
                title: "Открыть госкомпанию",
                imageName: ImageSet.RehabilitationContingent,
                text: new string[]
                {
                    "Мы вложим крупную сумму, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -cost),
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 100),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 30)],
                requirements: [
                    ChoiceRequirement.Cost(cost)]);
        }
    }
}
