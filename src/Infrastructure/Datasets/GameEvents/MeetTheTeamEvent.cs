using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class MeetTheTeamEvent
    {
        #region Immutable
        private const string Id = GameEventConstants.MeetTheTeam;
        private const string Efficiency = "Efficiency";
        private const string QualityOfLife = "QualityOfLife";
        private const string BecomeTheBest = "BecomeTheBest";
        private const string DontKnow = "DontKnow";
        #endregion

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { Efficiency, GetGameAction(Efficiency) },
                { QualityOfLife, GetGameAction(QualityOfLife) },
                { BecomeTheBest, GetGameAction(BecomeTheBest) },
                { DontKnow, GetGameAction(DontKnow) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static GameAction GetGameAction(string rulerGoal)
        {
            return new GameAction(
                effects: [],
                newEventCodes: [GameEventConstants.JourneyStart],
                displayInfoResult: GetEpilog(rulerGoal));
        }

        // Блок 1. Совет станции

        private static Slide[] GetPrologSlides()
        {
            return [
                GetSlide1(),
                GetSlideCamilla(),
                GetSlideCassius(),
                GetSlideLien(),
                GetSlideDarius(),
                GetSlideToThePoint(),
                GetSlideWhyHurry(),
                GetSlideGoals()];
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Совет станции",
                imageName: ImageSet.CaptainHall,
                text: new string[]
                {
                    "Совет станции собран. Консорциум подбирал команду под задачу.",
                    "Камилла — администратор и самый опытный член команды, ваш главный помощник. Кассиус — финансист и юрист колонии. Док отвечает за найм, миграцию и социальные вопросы. Лиен — инженер, отвечает за безопасность реактора и систем станции.",
                    "Вы уже некоторое время общаетесь и узнаёте друг друга."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее",
                        administratorSlideId: $"{Id}_1A",
                        financierSlideId: $"{Id}_1B",
                        engineerSlideId: $"{Id}_1C",
                        socialSlideId: $"{Id}_1D")]);
        }

        private static Slide GetSlideCamilla()
        {
            return new Slide(
                id: $"{Id}_1A",
                title: "Камилла — администратор",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла Селезнева, 34 года. Администратор.",
                    "Родилась в Новосибирске, отец работал на станции «Рубин». Опыт управления, связи на большинстве " +
                    "крупных станций Пояса. Знает Пояс изнутри.",
                    "Сейчас находится на Церере — координационном центре Консорциума в Поясе. До неё сигнал идёт около получаса.",
                    "Координирует работу совета, отвечает за связь с Консорциумом и замещает правителя в его отсутствие."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее",
                        financierSlideId: $"{Id}_1B",
                        engineerSlideId: $"{Id}_1C",
                        socialSlideId: $"{Id}_1D")]);
        }

        private static Slide GetSlideCassius()
        {
            return new Slide(
                id: $"{Id}_1B",
                title: "Кассиус — финансист",
                imageName: ImageSet.Cassius,
                text: new string[]
                {
                    "Кассиус Морн аль-Хаким, 36 лет. Финансист и юрист.",
                    "Родился в Дубае, степень Лондонской школы экономики. Двенадцать лет в корпоративном праве " +
                    "и налоговом консалтинге. Холодный ум, знает устав Консорциума лучше, чем сам Консорциум. " +
                    "Сейчас в Лондоне.",
                    "Управляет бюджетом, налогами, контрактами и отчётностью перед Консорциумом. Отвечает за юридическую " +
                    "защиту колонии."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее",
                        administratorSlideId: $"{Id}_1A",
                        engineerSlideId: $"{Id}_1C",
                        socialSlideId: $"{Id}_1D")]);
        }

        private static Slide GetSlideLien()
        {
            return new Slide(
                id: $"{Id}_1C",
                title: "Лиен — инженер",
                imageName: ImageSet.Lien,
                text: new string[]
                {
                    "Лиен Чжан, 30 лет. Инженер.",
                    "Родилась в Циндао, в 19 лет улетела на Луну по программе RAS. Прошла путь от разнорабочей " +
                    "до старшего инженера. Ценит безопасность выше эффективности. Сейчас в Китае — навещает " +
                    "родителей перед отлётом в Пояс.",
                    "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции. Следит за безопасностью " +
                    "и предотвращает аварии."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее",
                        administratorSlideId: $"{Id}_1A",
                        financierSlideId: $"{Id}_1B",
                        socialSlideId: $"{Id}_1D")]);
        }

        private static Slide GetSlideDarius()
        {
            return new Slide(
                id: $"{Id}_1D",
                title: "Дариус — социальный советник",
                imageName: ImageSet.Darius,
                text: new string[]
                {
                    "Дариус «Док» Уэбб, 42 года. HR и миграционный контролёр.",
                    "Родился в Хьюстоне, пятнадцать лет служил в миграционной службе ОПЗ на Луне. Считает, что каждый " +
                    "заслуживает шанса. Сейчас на геостационарной орбите, изучает агентские компании по найму персонала.",
                    "Отвечает за найм, удержание колонистов и внутренний климат. Решает конфликты между работниками."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее",
                        administratorSlideId: $"{Id}_1A",
                        financierSlideId: $"{Id}_1B",
                        engineerSlideId: $"{Id}_1C")]);
        }

        // Блок 2. К делу

        private static Slide GetSlideToThePoint()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "К делу",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла переводит разговор к делу.",
                    "«Я проанализирую данные и подберу астероиды, которые подойдут колонии. Кассиус подготовит " +
                    "шаблон свода законов и финансовые данные колонии. Док начнёт искать персонал: " +
                    "медиков, охрану, инженеров, аграриев.»",
                    "Она делает паузу.",
                    "«Вам и Лиен нужно вылететь в Пояс в течение двух недель. Станцию нужно принять: проверить " +
                    "реактор и системы, подписать акты готовности.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2A", "Почему такая спешка?"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlideWhyHurry()
        {
            return new Slide(
                id: $"{Id}_2A",
                title: "Почему такая спешка?",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Путь от Земли до Пояса занимает около двух месяцев. Плюс несколько дней на приёмку и бюрократию, " +
                    "плюс запас на риски — ещё пара недель.",
                    "Если хотите уложиться в срок, лучше решить дела на Земле в ближайшие дни и вылетать на орбиту. " +
                    "К моменту прибытия мы определимся, куда транспортировать станцию."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        // Блок 3. Ваши цели

        private static Slide GetSlideGoals()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Ваши цели",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Когда задачи распределены, Камилла обращается к вам.",
                    "«Вы решили, как хотите развивать колонию? Что для вас важнее всего? Куда вы хотите её привести?",
                    "Это поможет нам определить направление. Под него будем выстраивать бюджет, найм, производство»."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton(Efficiency, "Эффективность"),
                    SlideButton.GetSetChoiceButton(QualityOfLife, "Качество жизни"),
                    SlideButton.GetSetChoiceButton(BecomeTheBest, "Стать лучшими"),
                    SlideButton.GetSetChoiceButton(DontKnow, "Пока не знаю")]);
        }

        private static DisplayInfo? GetEpilog(string rulerGoal)
        {
            var (name, answer) = rulerGoal switch
            {
                Efficiency => ("Эффективность",
                    "«Доход, население, скорость строительства — каждый показатель можно измерить и улучшить. " +
                    "Я хочу сделать станцию, которая выдаёт максимум. Эффективность — это основа. " +
                    "А богатство и стабильность придут как следствие»."),
                QualityOfLife => ("Качество жизни",
                    "«Люди прилетают в Пояс не для того, чтобы выживать. Они хотят жить. Строить дома, растить детей, " +
                    "чувствовать себя в безопасности. Моя станция не будет конвейером. Если люди счастливы — они " +
                    "работают лучше, и колония вырастет сама собой»."),
                BecomeTheBest => ("Стать лучшими",
                    "«Конкуренция — это двигатель. Я хочу, чтобы наша станция была в топе рейтингов. " +
                    "Чтобы о ней говорили в каждом секторе Пояса. Это репутация, влияние, будущее»."),
                DontKnow => ("Пока не знаю",
                    "«Я пока не знаю точно, каким хочу видеть будущее колонии. Готового плана нет. " +
                    "Но я точно знаю, что хочу построить что-то стоящее. И буду искать правильный курс по пути — " +
                    "вместе с вами»."),
                _ => throw new System.NotImplementedException()
            };

            return new DisplayInfo(
                name,
                ImageSet.Station_1,
                [
                    $"Ваш ответ: {answer}"
                ]);
        }
    }
}