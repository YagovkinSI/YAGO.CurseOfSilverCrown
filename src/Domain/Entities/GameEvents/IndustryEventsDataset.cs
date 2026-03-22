using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public static class IndustryEventsDataset
    {
        public static GameEvent[] Get()
        {
            return
            [
                GetServiceCompany(),
                GetEngineeringTeam(),
                GetMiningBrigade(),
                GetRehabilitationContingent(),
                GetProductionCompany()
            ];
        }

        private static GameEvent GetEngineeringTeam()
        {
            var id = "EngineeringTeam";
            const int zonesOccupied = 3;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied)
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
                    new KeyValueParameter(ColonyStatNames.Laws_CodeOfLaws_HighTax, double.MinValue),
                ],
                episode: new Episode(
                    id: id,
                    prologSlides: [],
                    choice: [ new Slide(
                        title: "Инженерная Команда",
                        imageName: ImageSet.EngineeringTeam,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по добыче ресурсов. " +
                            "Это высокотехнологичная инженерная команда с передовым оборудованием AS " +
                            "и горсткой высокооплачиваемых специалистов."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 20),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 10),
                        ])])
                );
        }

        private static GameEvent GetMiningBrigade()
        {
            var id = "MiningBrigade";
            const int zonesOccupied = 3;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.04),
                ],
                episode: new Episode(
                    id: id,
                    prologSlides: [],
                    choice: [ new Slide(
                        title: "Горнодобывающая Бригада",
                        imageName: ImageSet.MiningBrigade,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по добыче ресурсов. " +
                            "Бригада лицензированных рудокопов с надёжным оборудованием, " +
                            "коих многие тысячи на поясе."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 30),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 15),
                        ])])
                );
        }

        private static GameEvent GetRehabilitationContingent()
        {
            var id = "RehabilitationContingent";
            const int zonesOccupied = 4;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.Industry_Minning_Available, 1),
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.03),
                    new KeyValueParameter(ColonyStatNames.Laws_CodeOfLaws_HighStandart, double.MinValue),
                ],
                episode: new Episode(
                    id: id,
                    prologSlides: [],
                    choice: [ new Slide(
                        title: "Реабилитационный Контингент",
                        imageName: ImageSet.RehabilitationContingent,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по добыче ресурсов. " +
                            "Они используют дешёвое оборудование и контингент должников. " +
                            "Дёшево, но рискованно."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 50),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 30),
                        ])])
                );
        }

        private static GameEvent GetProductionCompany()
        {
            var id = "ProductionCompany";
            const int zonesOccupied = 5;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.02),
                ],
                episode: new Episode(
                    id: id,
                    prologSlides: [],
                    choice: [ new Slide(
                        title: "Новая Фабрика",
                        imageName: ImageSet.ProductionCompany,
                        text: new string[]
                        {
                            "К колонии присоединяется производственная компания. " +
                            "Новые колонисты будут производить продукцию компании на нашей станции."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Production_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 25),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 25),
                        ])])
                );
        }

        private static GameEvent GetServiceCompany()
        {
            var id = "ServiceCompany";
            const int zonesOccupied = 3;
            return new(
                id: id,
                chanceDefault: 0,
                requirements: [
                    new KeyValueParameter(ColonyStatNames.AreaCapacity_Available, zonesOccupied),
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Need, 0),
                ],
                parameterModifiers: [
                    new KeyValueParameter(ColonyStatNames.Attractiveness_Total, 0.01),
                    new KeyValueParameter(ColonyStatNames.Industry_Service_Need, 0.5),
                ],
                episode: new Episode(
                    id: id,
                    prologSlides: [],
                    choice: [ new Slide(
                        title: "Расширение сферы услуг",
                        imageName: ImageSet.ServiceCompany,
                        text: new string[]
                        {
                            "К колонии присоединяется компания по оказанию услуг. " +
                            "Новые колонисты будут оказывать услуги ростущему населению."
                        },
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Service_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, zonesOccupied),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 10),
                            new KeyValueParameter(ColonyStatNames.Population_Total, 10),
                        ])])
                );
        }
    }
}
