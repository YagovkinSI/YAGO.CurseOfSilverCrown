using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class SkipPrologueEvent
    {
        private const string Id = nameof(SkipPrologueEvent);
        public static GameEvent Get()
        {
            return new(
                id: Id,
                chanceDefault: 1,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.EpisodeCount, 1, isTopThreshold : true)
                ],
                parameterModifiers: [],
                episode: GetEpisode());
        }

        private static Episode GetEpisode()
        {
            return new Episode(
                slides: GetPrologSlides());
        }

        private static Slide[] GetPrologSlides()
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
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, -657)],
                    continueButtonName: "Далее",
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
                    continueButtonName: "Далее",
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент...")]),

                new Choice(
                    id: $"{Id}_2",
                    title: "Стандартный Протокол",
                    imageName: ImageSet.LawsStandart,
                    text: [
                        "Компромиссный каркас для тысяч колоний. Чёткие, но выполнимые нормы по труду, " +
                        "безопасности и экологии. Без излишней нагрузки на бизнес. Сбалансированный налог. " +
                        "Все резиденты и ОПЗ считают колонию благонадёжной. Устойчивый рост без резких колебаний."
                    ],
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 3),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 3),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 80),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 80)],
                    requirements: [],
                    buttonName: "Выбрать",
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_2")]),

                new Choice(
                    id: $"{Id}_3",
                    title: "Гуманистический Устав",
                    imageName: ImageSet.LawsHumanist,
                    text: [
                        "Жёсткие стандарты жизни: жильё, питание, медицина, безопасность. " +
                        "Низкие налоги — для компенсации затрат резидентов. " +
                        "Колония становится магнитом для лучших специалистов и быстро получает привилегированный статус. " +
                        "Но дороговизна отпугивает дешёвую рабочую силу и рисковые проекты."
                    ],
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 1),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 5),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 40),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 60),
                        new KeyValueParameter(ColonyStatNames.Mood_Total, 5)],
                    requirements: [],
                    buttonName: "Выбрать",
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_4", "Корпоративный Регламент..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_3")]),

                new Choice(
                    id: $"{Id}_4",
                    title: "Корпоративный Регламент",
                    imageName: ImageSet.LawsCorporate,
                    text: [
                        "Абсолютный минимум социальных гарантий. Повышенные налоги и сборы — " +
                        "взамен на свободу действий и слабый надзор. " +
                        "Привлекает авантюристов и теневые схемы. Казна быстро пополняется, " +
                        "но колония становится социальной пороховой бочкой."
                    ],
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 5),
                        new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 1),
                        new KeyValueParameter(ColonyStatNames.Industry_Minning_Companies, 4),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 30),
                        new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, 120),
                        new KeyValueParameter(ColonyStatNames.Population_Total, 90),
                        new KeyValueParameter(ColonyStatNames.Mood_Total, -5)],
                    requirements: [],
                    buttonName: "Выбрать",
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Стандартный Протокол..."),
                        SlideButton.GetButtonToSlide($"{Id}_3", "Гуманистический Устав..."),
                        SlideButton.GetSetChoiceButton(Id, $"{Id}_4")])];
        }
    }
}
