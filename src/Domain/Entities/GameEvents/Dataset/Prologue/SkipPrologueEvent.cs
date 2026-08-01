using System.Collections.Generic;
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
            var choiceNameList = new Dictionary<string, string>() {
                { $"{Id}_2", "Стандартный Протокол" },
                { $"{Id}_3", "Гуманистический Устав" },
                { $"{Id}_4", "Корпоративный Регламент" },
            };
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { $"{Id}_2", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(StateKey.BuildingsMiningState, 4)],
                    newQuests: [],
                    requirements: [])},
                { $"{Id}_3", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(StateKey.ReformsTaxLevel, -2),
                        new KeyValueParameter(StateKey.ReformsSocialGuaranteesLevel, 2),
                        new KeyValueParameter(StateKey.BuildingsMiningState, 4),
                        new KeyValueParameter(StateKey.MoodCurrent, 5)],
                    newQuests: [],
                    requirements: [])},
                { $"{Id}_4", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(StateKey.ReformsTaxLevel, 2),
                        new KeyValueParameter(StateKey.ReformsSocialGuaranteesLevel, -2),
                        new KeyValueParameter(StateKey.BuildingsMiningState, 4),
                        new KeyValueParameter(StateKey.MoodCurrent, -5)],
                    newQuests: [],
                    requirements: [])},
                { "#end", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(StateKey.SolarsCurrent, -4500),
                        new KeyValueParameter(StateKey.BuildingsAdministrativeState, 1)],
                    newQuests: [ nameof(MvpQuest) ],
                    requirements: [])}
            };
            return new(
                id: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                episode: GetEpisode(choiceNameList),
                changeList: changeList,
                results: GetResults(choiceNameList));
        }

        private static Episode GetEpisode(Dictionary<string, string> choiceNameList)
        {
            return new Episode(
                slides: GetPrologSlides(choiceNameList));
        }

        private static Slide[] GetPrologSlides(Dictionary<string, string> choiceNameList)
        {
            return [
                new Slide(
                    id: $"{Id}_0",
                    title: "Открытие колонии",
                    imageName: ImageSet.GrayСorridor,
                    text: new string[]
                    {
                        "Три месяца подготовки пролетели как один день. Время ушло на сбор команды, изучение отчётов по астероидам " +
                        "и согласование деталей с чиновниками Консорциума. Теперь выбор сделан — это твоя зона добычи. " +
                        "Советники уже на месте, оборудование заказано, осталось только дождаться прибытия на станцию " +
                        "и начать воплощать задуманное."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1")]),

                new Slide(
                    id: $"{Id}_1",
                    title: "Открытие колонии",
                    imageName: ImageSet.RegularCycle,
                    text: new string[]
                    {
                        "Ты прибыл на станцию и торжественно открыл колонию. " +
                        "Месяц ушел на развёртывание инфраструктуры, запуск оборудования и отладку систем. " +
                        "К концу второго месяца добывающие модули вышли на плановую мощность, переработав первую руду с астероида. " +
                        "Население перевалило за полсотни и продолжает расти, а бюджет вышел в небольшой плюс.",
                        "Ты многое сделал за это время, но главным выбором было определение свода законов, по которому теперь живут колонисты."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_2", choiceNameList[$"{Id}_2"], infoSlideId: $"{Id}_2"),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_3", choiceNameList[$"{Id}_3"], infoSlideId: $"{Id}_3"),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_4", choiceNameList[$"{Id}_4"], infoSlideId: $"{Id}_4")]),

                new Slide(
                    id: $"{Id}_2",
                    title: choiceNameList[$"{Id}_2"],
                    imageName: ImageSet.LawsStandart,
                    text: [
                        "Компромиссный каркас для десятков колоний. Чёткие, но выполнимые нормы по труду, безопасности и экологии. " +
                        "Без излишней нагрузки на бизнес. Сбалансированный налог. Все резиденты и Консорциум считают колонию благонадёжной. " +
                        "Устойчивый рост без резких колебаний."
                    ],
                    parameters: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_2")]),

                new Slide(
                    id: $"{Id}_3",
                    title: choiceNameList[$"{Id}_3"],
                    imageName: ImageSet.LawsHumanist,
                    text: [
                        "Высокие стандарты жизни: жильё, питание, медицина, безопасность. Низкие налоги — для компенсации затрат резидентов. " +
                        "Колония становится магнитом для лучших специалистов и со временем может получить привилегированный статус. " +
                        "Но дороговизна отпугивает дешёвую рабочую силу и рисковые проекты."
                    ],
                    parameters: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_3")]),

                new Slide(
                    id: $"{Id}_4",
                    title: choiceNameList[$"{Id}_4"],
                    imageName: ImageSet.LawsCorporate,
                    text: [
                        "Абсолютный минимум социальных гарантий. Повышенные налоги и сборы — взамен на свободу действий " +
                        "и минимальное вмешательство в дела компаний на станции. Привлекает авантюристов и теневые схемы. " +
                        "Казна быстро пополняется, но колония становится социальной пороховой бочкой."
                    ],
                    parameters: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_4")])];
        }

        private static Dictionary<string, EventResult> GetResults(
            Dictionary<string, string> choiceNameList)
        {
            const string epilogText = "Теперь в колонии кипит жизнь.";
            var result2 = EventResult.CreateNew(
                title: choiceNameList[$"{Id}_2"],
                imageName: ImageSet.LawsStandart,
                text: [epilogText]);
            var result3 = EventResult.CreateNew(
                title: choiceNameList[$"{Id}_3"],
                imageName: ImageSet.LawsHumanist,
                text: [epilogText]);
            var result4 = EventResult.CreateNew(
                title: choiceNameList[$"{Id}_4"],
                imageName: ImageSet.LawsCorporate,
                text: [epilogText]);
            return new Dictionary<string, EventResult>() {
                { $"{Id}_2", result2 },
                { $"{Id}_3", result3 },
                { $"{Id}_4", result4 },
            };
        }
    }
}
