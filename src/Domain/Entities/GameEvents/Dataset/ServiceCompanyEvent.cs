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
                id: id,
                prologSlides: GetPrologSlides(),
                choice: [
                    GetChoice1(),
                    GetChoice2(),
                    GetChoice3()
                ],
                choiceLabel: "Как поступим?");
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
                parameters: [])];
        }

        private static Slide GetChoice1()
        {
            return new Slide(
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

        private static Slide GetChoice2()
        {
            return new Slide(
                title: "Отказать",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Когда будет достаточно средств мы откроем государственную компанию. " +
                    "А пока колонистам придётся подождать."
                },
                parameters: []);
        }

        private static Slide GetChoice3()
        {
            return new Slide(
                title: "Открыть госкомпанию",
                imageName: ImageSet.ServiceCompany,
                text: new string[]
                {
                    "Мы вложим 500 солар, чтобы открыть государственную компанию." +
                    "Это даст больше прибыли в бюджет и больше контроля."
                },
                parameters: [
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Companies, 1),
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -500),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, _zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                    new KeyValueParameter(ColonyStatNames.Population_Total, 10)]);
        }
    }
}
