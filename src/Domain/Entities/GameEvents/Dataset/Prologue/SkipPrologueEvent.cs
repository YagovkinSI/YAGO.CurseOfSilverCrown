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
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 1250),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 80)],
                    newQuests: [],
                    availableRequirements: [])},
                { $"{Id}_3", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 1),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 5),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 1050),
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
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 1450),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 90),
                        new KeyValueParameter(ColonyStatNames.Mood_Total, -5)],
                    newQuests: [],
                    availableRequirements: [])},
                { "#end", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, -500)],
                    newQuests: [ nameof(MvpQuest) ],
                    availableRequirements: [])}
            };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList),
                changeList: changeList,
                isImmediatelyEvent: false);
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
                    title: "Свод Законов",
                    imageName: ImageSet.RegularCycle,
                    text: new string[]
                    {
                        "Спустя три месяца ты прибыл на станцию и торжественно открыл колонию. " +
                        "Месяц ушел на развёртывание инфраструктуры, запуск оборудования и отладку систем. " +
                        "К концу второго месяца добывающие модули вышли на плановую мощность, переработав первую руду с астероида. " +
                        "Население перевалило за полсотни и продолжает расти, а бюджет вышел в небольшой плюс.",
                        "Ты многое сделал за это время, но главным выбором было определение свода законов, по которому теперь живут колонисты."
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
                        "Компромиссный каркас для десятков колоний. Чёткие, но выполнимые нормы по труду, безопасности и экологии. " +
                        "Без излишней нагрузки на бизнес. Сбалансированный налог. Все резиденты и Консорциум считают колонию благонадёжной. " +
                        "Устойчивый рост без резких колебаний."
                    ],
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_2")]),

                new Slide(
                    id: $"{Id}_3",
                    title: "Гуманистический Устав",
                    imageName: ImageSet.LawsHumanist,
                    text: [
                        "Высокие стандарты жизни: жильё, питание, медицина, безопасность. Низкие налоги — для компенсации затрат резидентов. " +
                        "Колония становится магнитом для лучших специалистов и со временем может получить привилегированный статус. " +
                        "Но дороговизна отпугивает дешёвую рабочую силу и рисковые проекты."
                    ],
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_3")]),

                new Slide(
                    id: $"{Id}_4",
                    title: "Корпоративный Регламент",
                    imageName: ImageSet.LawsCorporate,
                    text: [
                        "Абсолютный минимум социальных гарантий. Повышенные налоги и сборы — взамен на свободу действий " +
                        "и минимальное вмешательство в дела компаний на станции. Привлекает авантюристов и теневые схемы. " +
                        "Казна быстро пополняется, но колония становится социальной пороховой бочкой."
                    ],
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_4")])];
        }
    }
}
