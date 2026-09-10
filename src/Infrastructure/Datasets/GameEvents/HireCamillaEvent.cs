using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class HireCamillaEvent
    {
        private const string Id = GameEventConstants.HireCamilla;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetSlides(),
                actions: new Dictionary<string, GameAction> {
                    { "Low", GetHireAction(50) },
                    { "Medium", GetHireAction(55) },
                    { "High", GetHireAction(60) },
                },
                tags: [GameEventTags.CouncilAdministrator]);
        }

        private static GameAction GetHireAction(int initialLoyalty)
        {
            var displayInfo = new DisplayInfo(
                name: "Камилла Селезнёва",
                imageName: ImageSet.Camilla,
                description: [
                    "Камилла приняла пост администратора и берёт на себя координацию работы станции, внешние связи и поиск кадров."]);
            return new GameAction(
                effects:
                [
                    new GameEffect(GameEffectType.SetAdministrator, delta: initialLoyalty, code: PersonConstants.Camilla),
                    new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.GameplayCamilla),
                ],
                newEventCodes: [
                    GameEventConstants.JourneyStart],
                displayInfoResult: displayInfo);
        }

        private static Slide[] GetSlides() => [
            GetSlideResume(),
            GetSlideInterview(),
            GetSlideAnswerAboutPreviousJob(),
            GetSlideAnswerAboutFuture(),
            GetSlideFinalQuestion(),
            GetSlideAnswerEfficiency(),
            GetSlideAnswerQualityOfLife(),
            GetSlideAnswerBecomeTheBest(),
            GetSlideAnswerDontKnow(),];

        private static Slide GetSlideResume()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Найм администратора",
                imageName: ImageSet.Camilla,
                text: [
                    "Вы просматриваете резюме на должность помощника, которое пришло через кадровое агентство Консорциума.",
                    "Камилла Селезнева, 34 года.",
                    "Опыт работы помощником управляющего на станции Консорциума 8 лет. " +
                    "До этого — инженер-технолог на станции «Рубин» (одна из первых колоний в Поясе).",
                    "Рекомендации: безупречные.",
                    "Примечание агента: «Сильный кандидат. Знает Пояс изнутри. Готова начать работу немедленно.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Провести собеседование")]);
        }

        private static Slide GetSlideInterview()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Камилла Селезнева",
                imageName: ImageSet.Camilla,
                text: [
                    "Вы проводите собеседование. Камилла на Церере — сигнал идёт около получаса. Время на ответ есть, но каждое слово должно быть взвешенным. Вы обсудили её опыт и мотивацию. Осталось задать последние вопросы."],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Почему вы покинули прошлое место работы?"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Какой вы видите станцию через пять лет?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "У вас остались вопросы?"),
                    SlideButton.GetSetChoiceButton("Low", "Вы приняты")]);
        }

        private static Slide GetSlideAnswerAboutPreviousJob()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Камилла Селезнева",
                imageName: ImageSet.Camilla,
                text: [
                    "Камилла уверенно отвечает:",
                    "«Мой бывший начальник был хорошим управленцем — он умел держать порядок и не допускал ошибок. " +
                    "Но он не хотел развиваться. Станция работала стабильно, но без роста, без амбиций. " +
                    "Я не видела смысла оставаться там десятилетиями, просто поддерживая одно и то же. " +
                    "Я хочу строить, а не просто поддерживать»."],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_3", "Какой вы видите станцию через пять лет?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "У вас остались вопросы?"),
                    SlideButton.GetSetChoiceButton("Low", "Вы приняты")]);
        }

        private static Slide GetSlideAnswerAboutFuture()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Камилла Селезнева",
                imageName: ImageSet.Camilla,
                text: [
                    "Камилла оживляется:",
                    "«Я вижу станцию, о которой говорят. Не просто ещё одна добывающая колония, а место, куда хотят приехать специалисты, " +
                    "куда стремятся инвесторы. Репутация — это не просто слова. Это контракты, это люди, это будущее. " +
                    "Мне важно, чтобы моё имя ассоциировалось с честной и сильной станцией.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Почему вы покинули прошлое место работы?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "У вас остались вопросы?"),
                    SlideButton.GetSetChoiceButton("Low", "Вы приняты")]);
        }

        private static Slide GetSlideFinalQuestion()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Камилла Селезнева",
                imageName: ImageSet.Camilla,
                text: [
                    "Камилла немного подаётся вперёд:",
                    "«Что для вас важно в этой работе? Куда вы хотите привести колонию? Это поможет мне понять, что я могу вам дать.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_5", "Эффективность"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "Качество жизни"),
                    SlideButton.GetButtonToSlide($"{Id}_7", "Стать лучшими"),
                    SlideButton.GetButtonToSlide($"{Id}_8", "Пока не знаю")]);
        }

        private static Slide GetSlideAnswerEfficiency()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Эффективность",
                imageName: ImageSet.Camilla,
                text: [
                    "Ваш ответ:",
                    "«Доход, прирост населения, скорость строительства — каждый показатель можно измерить и улучшить. Я хочу сделать станцию, которая выдаёт максимум. Эффективность — это основа. А богатство и стабильность придут как следствие.»",
                    "Камилла чуть заметно кивает:",
                    "«Я люблю, когда правитель знает, чего хочет. Мы настроим процессы так, что колония станет примером для всего Пояса.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("Medium", "Добро пожаловать в команду")]);
        }

        private static Slide GetSlideAnswerQualityOfLife()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Качество жизни",
                imageName: ImageSet.Camilla,
                text: [
                    "Ваш ответ:",
                    "«Люди прилетают в Пояс не для того, чтобы выживать. Они хотят жить. Строить дома, растить детей, чувствовать себя в безопасности. Моя станция не будет конвейером. Это будет место, где у людей есть будущее. Если они счастливы — они работают лучше. А значит, колония будет расти сама собой.»",
                    "Камилла улыбается:",
                    "«Это то, что я хотела услышать. Я помогу вам найти ресурсы, чтобы всё это работало.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("High", "Добро пожаловать в команду")]);
        }

        private static Slide GetSlideAnswerBecomeTheBest()
        {
            return new Slide(
                id: $"{Id}_7",
                title: "Стать лучшими",
                imageName: ImageSet.Camilla,
                text: [
                    "Ваш ответ:",
                    "«Конкуренция — это двигатель. Я хочу, чтобы наша станция была в топе рейтингов. Чтобы другие правители смотрели на нас и завидовали. Чтобы нашу станцию знали в каждом секторе Пояса. Это не просто игра в цифры — это репутация, влияние, будущее. Мы должны быть лучшими.»",
                    "Камилла смотрит на вас с живым интересом:",
                    "«Амбиции — это то, чего не хватает большинству правителей. Я помогу вам с дипломатией и контактами — это то, что отделяет топовые станции от рядовых добывающих колоний.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("Medium", "Добро пожаловать в команду")]);
        }

        private static Slide GetSlideAnswerDontKnow()
        {
            return new Slide(
                id: $"{Id}_8",
                title: "Пока не знаю",
                imageName: ImageSet.Camilla,
                text: [
                    "Ваш ответ:",
                    "«Я пока не знаю точно, каким хочу видеть своё будущее. У меня нет готового плана. Но я точно знаю, что хочу построить что-то стоящее. И я буду учиться — у вас, у других советников, у самой жизни на станции. Может быть, правильный путь найдётся в процессе.»",
                    "Камилла слегка наклоняет голову:",
                    "«Честность — редкое качество. Мне нравится, что вы не строите из себя всезнающего стратега. Я помогу вам разобраться.»"],
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("High", "Добро пожаловать в команду")]);
        }
    }
}
