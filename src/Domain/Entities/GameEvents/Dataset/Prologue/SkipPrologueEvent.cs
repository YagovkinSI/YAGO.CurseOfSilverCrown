using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class SkipPrologueEvent
    {
        private const string Id = nameof(SkipPrologueEvent);
        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { $"{Id}_2", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 3),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 3),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 80),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 80)],
                    newQuests: [],
                    availableRequirements: [])},
                { $"{Id}_3", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 1),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 5),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 40),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 60),
                        new KeyValueParameter(ColonyStatNames.Mood_Total, 5)],
                    newQuests: [],
                    availableRequirements: [])},
                { $"{Id}_4", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 5),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 1),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 120),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 90),
                        new KeyValueParameter(ColonyStatNames.Mood_Total, -5)],
                    newQuests: [],
                    availableRequirements: [])},
                { "#end", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, -657)],
                    newQuests: [ nameof(MvpQuest) ],
                    availableRequirements: [])}
            };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList),
                changeList: changeList,
                isImmediatelyEvent: true);
        }

        private static Episode GetEpisode(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Episode(
                slides: GetPrologSlides(changeList));
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameEventChangeList> changeList)
        {
            return [
                new Slide(
                    id: $"{Id}_0",
                    title: "Свод Законов",
                    imageName: ImageSet.RegisterColony,
                    text: new string[]
                    {
                        "За полгода подготовки ты прошёл большой путь. Зарегистрировал колонию в " +
                        "Орбитальном Правительстве Земли (ОПЗ) и получил статус начинающей колонии. " +
                        "Купил лицензию на один из астероидов в Поясе и организовал небольшую добывающую компанию. " +
                        "Познакомился с командой советников и выбрал первых специалистов для работы на станции."
                    },
                    parameters: changeList["#end"].ColonyStats,
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1")]),

                new Slide(
                    id: $"{Id}_1",
                    title: "Свод Законов",
                    imageName: ImageSet.RegularCycle,
                    text: new string[]
                    {
                        "Спустя шесть месяцев ты торжественно открыл колонию, " +
                        "а ещё через полтора месяца добывающая компания переработала первую руду с астероида. " +
                        "Население превысило полсотни человек, а бюджет вышел в небольшой плюс.",
                        "Ты многое сделал за это время, но главным выбором было определение свода законов по которому теперь живут колонисты."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент...")]),

                new Slide(
                    id: $"{Id}_2",
                    title: "Стандартный Протокол",
                    imageName: ImageSet.LawsStandart,
                    text: [
                        "Компромиссный каркас для тысяч колоний. Чёткие, но выполнимые нормы по труду, " +
                        "безопасности и экологии. Без излишней нагрузки на бизнес. Сбалансированный налог. " +
                        "Все резиденты и ОПЗ считают колонию благонадёжной. Устойчивый рост без резких колебаний."
                    ],
                    parameters: changeList[$"{Id}_2"].ColonyStats,
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_2")]),

                new Slide(
                    id: $"{Id}_3",
                    title: "Гуманистический Устав",
                    imageName: ImageSet.LawsHumanist,
                    text: [
                        "Жёсткие стандарты жизни: жильё, питание, медицина, безопасность. " +
                        "Низкие налоги — для компенсации затрат резидентов. " +
                        "Колония становится магнитом для лучших специалистов и быстро получает привилегированный статус. " +
                        "Но дороговизна отпугивает дешёвую рабочую силу и рисковые проекты."
                    ],
                    parameters: changeList[$"{Id}_3"].ColonyStats,
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_3")]),

                new Slide(
                    id: $"{Id}_4",
                    title: "Корпоративный Регламент",
                    imageName: ImageSet.LawsCorporate,
                    text: [
                        "Абсолютный минимум социальных гарантий. Повышенные налоги и сборы — " +
                        "взамен на свободу действий и слабый надзор. " +
                        "Привлекает авантюристов и теневые схемы. Казна быстро пополняется, " +
                        "но колония становится социальной пороховой бочкой."
                    ],
                    parameters: changeList[$"{Id}_4"].ColonyStats,
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_4")])];
        }
    }
}
