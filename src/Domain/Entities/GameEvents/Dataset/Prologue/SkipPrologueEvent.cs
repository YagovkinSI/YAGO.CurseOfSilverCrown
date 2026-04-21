using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class SkipPrologueEvent
    {
        public static GameEvent Get()
        {
            var id = nameof(SkipPrologueEvent);
            return new(
                id: id,
                chanceDefault: 1,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.EpisodeCount, 1, isTopThreshold : true)
                ],
                parameterModifiers: [],
                episode: GetEpisode(id));
        }

        private static Episode GetEpisode(string id)
        {
            return new Episode(
                id: id,
                title: "Свод Законов",
                prologSlides: GetPrologSlides(),
                dilemma: GetDilemma());
        }

        private static PrologueSlide[] GetPrologSlides()
        {
            return [
                new PrologueSlide(
                title: "Свод Законов",
                imageName: ImageSet.Yago,
                text: new string[]
                {
                    "За полгода подготовки вы прошли большой путь. Зарегистрировали колонию в " +
                    "Орбитальном Правительстве Земли (ОПЗ) и получили статус начинающей колонии. " +
                    "Купили лицензию на один из астероидов в Поясе и организовали небольшую добывающую компанию. " +
                    "Познакомились с командой советников и выбрали первых специалистов для работы в колонии."
                },
                parameters: [],
                continueButtonName: "Далее"),

                new PrologueSlide(
                title: "Свод Законов",
                imageName: ImageSet.RegularCycle,
                text: new string[]
                {
                    "Спустя шесть месяцев ты торжественно открыл собственную колонию, " +
                    "а ещё через полтора месяца добывающая компания переработала первую руду с астероида. " +
                    "Население достигло почти сотни человек, а бюджет вышел в небольшой плюс.",
                    "Ты многое сделал за это время, но главным выбором было определение свода законов по которому теперь живут колонисты."
                },
                parameters: [],
                continueButtonName: "Далее")];
        }

        private static Dilemma GetDilemma()
        {
            return new DilemmaSelect(
                choice: [
                    new Choice(
                        id: Guid.Parse("bd1a22e5-d642-421d-9ad8-d2c028fe7ecd"),
                        title: "Стандартный Протокол",
                        imageName: ImageSet.LawsStandart,
                        text: [
                            "Компромиссный каркас для тысяч колоний. Чёткие, но выполнимые нормы по труду, " +
                            "безопасности и экологии. Без излишней нагрузки на бизнес. Сбалансированный налог. " +
                            "Все резиденты и ОПЗ считают колонию благонадёжной. Устойчивый рост без резких колебаний."
                        ],
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 3),
                            new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 3)],
                        requirements: [],
                        buttonName: "Выбрать"
                        ),

                    new Choice(
                        id: Guid.Parse("0a0011a5-a414-4e59-85a7-d063b8926196"),
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
                            new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 5)],
                        requirements: [],
                        buttonName: "Выбрать"
                        ),
                    
                    new Choice(
                        id: Guid.Parse("8e34f141-26a5-4018-a531-0efbf44eff96"),
                        title: "Корпоративный Регламент",
                        imageName: ImageSet.LawsCorporate,
                        text: [
                            "Абсолютный минимум социальных гарантий. Повышенные налоги и сборы — " +
                            "взамен на свободу действий и слабый надзор. " +
                            "Привлекает авантюристов и теневые схемы. Казны быстро пополняется, " +
                            "но колония становится социальной пороховой бочкой."
                        ],
                        parameters: [
                            new KeyValueParameter(ColonyStatNames.Laws_TaxLevel, 5),
                            new KeyValueParameter(ColonyStatNames.Laws_SocialGuaranteesLevel, 1)],
                        requirements: [],
                        buttonName: "Выбрать"
                        )],
                choiceLabel: ["Заложите Фундамент Законов"]);
        }
    }
}
