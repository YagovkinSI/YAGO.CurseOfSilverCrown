using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class SkipPrologueEvent
    {
        private const string Id = nameof(SkipPrologueEvent);
        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var choiceNameList = new Dictionary<string, string>() {
                { $"{Id}_2", "Стандартный Протокол" },
                { $"{Id}_3", "Гуманистический Устав" },
                { $"{Id}_4", "Корпоративный Регламент" },
            };
            var changeList = new Dictionary<string, GameAction>() {
                { $"{Id}_2", new GameAction(
                    changes: [
                        new GameEffect(GameEffectType.AddBuildingsMiningState, 4)],
                    newEventCodes: [],
                    requirements: [])},
                { $"{Id}_3", new GameAction(
                    changes: [
                        new GameEffect(GameEffectType.ReformTaxLevel, 1),
                        new GameEffect(GameEffectType.ReformSocialGuaranteesLevel, 5),
                        new GameEffect(GameEffectType.AddBuildingsMiningState, 4),
                        new GameEffect(GameEffectType.AddMood, 5)],
                    newEventCodes: [],
                    requirements: [])},
                { $"{Id}_4", new GameAction(
                    changes: [
                        new GameEffect(GameEffectType.ReformTaxLevel, 5),
                        new GameEffect(GameEffectType.ReformSocialGuaranteesLevel, 1),
                        new GameEffect(GameEffectType.AddBuildingsMiningState, 4),
                        new GameEffect(GameEffectType.AddMood, -5)],
                    newEventCodes: [],
                    requirements: [])},
                { "#end", new GameAction(
                    changes: [
                        new GameEffect(GameEffectType.SpendSolars, 8500),
                        new GameEffect(GameEffectType.AddBuildingsAdministrativeState, 1)],
                    newEventCodes: [ nameof(MvpQuest) ],
                    requirements: [])}
            };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetPrologSlides(choiceNameList),
                changeList: changeList,
                results: GetResults(choiceNameList));
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
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1")]),

                new Slide(
                    id: $"{Id}_1",
                    title: "Открытие колонии",
                    imageName: ImageSet.RegularTurn,
                    text: new string[]
                    {
                        "Ты прибыл на станцию и торжественно открыл колонию. " +
                        "Месяц ушел на развёртывание инфраструктуры, запуск оборудования и отладку систем. " +
                        "К концу второго месяца добывающие модули вышли на плановую мощность, переработав первую руду с астероида. " +
                        "Население перевалило за полсотни и продолжает расти, а бюджет вышел в небольшой плюс.",
                        "Ты многое сделал за это время, но главным выбором было определение свода законов, по которому теперь живут колонисты."
                    },
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton($"{Id}_2", choiceNameList[$"{Id}_2"], infoSlideId: $"{Id}_2"),
                        SlideButton.GetSetChoiceButton($"{Id}_3", choiceNameList[$"{Id}_3"], infoSlideId: $"{Id}_3"),
                        SlideButton.GetSetChoiceButton($"{Id}_4", choiceNameList[$"{Id}_4"], infoSlideId: $"{Id}_4")]),

                new Slide(
                    id: $"{Id}_2",
                    title: choiceNameList[$"{Id}_2"],
                    imageName: ImageSet.LawsStandart,
                    text: [
                        "Компромиссный каркас для десятков колоний. Чёткие, но выполнимые нормы по труду, безопасности и экологии. " +
                        "Без излишней нагрузки на бизнес. Сбалансированный налог. Все резиденты и Консорциум считают колонию благонадёжной. " +
                        "Устойчивый рост без резких колебаний."
                    ],
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton($"{Id}_2")]),

                new Slide(
                    id: $"{Id}_3",
                    title: choiceNameList[$"{Id}_3"],
                    imageName: ImageSet.LawsHumanist,
                    text: [
                        "Высокие стандарты жизни: жильё, питание, медицина, безопасность. Низкие налоги — для компенсации затрат резидентов. " +
                        "Колония становится магнитом для лучших специалистов и со временем может получить привилегированный статус. " +
                        "Но дороговизна отпугивает дешёвую рабочую силу и рисковые проекты."
                    ],
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton($"{Id}_3")]),

                new Slide(
                    id: $"{Id}_4",
                    title: choiceNameList[$"{Id}_4"],
                    imageName: ImageSet.LawsCorporate,
                    text: [
                        "Абсолютный минимум социальных гарантий. Повышенные налоги и сборы — взамен на свободу действий " +
                        "и минимальное вмешательство в дела компаний на станции. Привлекает авантюристов и теневые схемы. " +
                        "Казна быстро пополняется, но колония становится социальной пороховой бочкой."
                    ],
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton($"{Id}_4")])];
        }

        private static Dictionary<string, GameActionResult> GetResults(
            Dictionary<string, string> choiceNameList)
        {
            const string epilogText = "Теперь в колонии кипит жизнь.";
            var result2 = GameActionResult.CreateNew(
                title: choiceNameList[$"{Id}_2"],
                imageName: ImageSet.LawsStandart,
                text: [epilogText]);
            var result3 = GameActionResult.CreateNew(
                title: choiceNameList[$"{Id}_3"],
                imageName: ImageSet.LawsHumanist,
                text: [epilogText]);
            var result4 = GameActionResult.CreateNew(
                title: choiceNameList[$"{Id}_4"],
                imageName: ImageSet.LawsCorporate,
                text: [epilogText]);
            return new Dictionary<string, GameActionResult>() {
                { $"{Id}_2", result2 },
                { $"{Id}_3", result3 },
                { $"{Id}_4", result4 },
            };
        }
    }
}
