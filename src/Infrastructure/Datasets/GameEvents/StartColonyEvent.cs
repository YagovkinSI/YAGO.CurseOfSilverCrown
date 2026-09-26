using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Domain.Stations;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class StartColonyEvent
    {
        private const string Id = GameEventConstants.StartColony;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.SetStation, code: StationModelConstants.Dawn_342),
                            new GameEffect(GameEffectType.SetAchievement, code: AchievementConstants.RulerContractSigned),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.FactionConsortium),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeShareholders),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.StationDawn),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.PersonsCamilla),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.PersonsCassius),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.PersonsLien),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.PersonsDarius)],
                        newEventCodes: [
                            GameEventConstants.AsteroidChoice,
                            GameEventConstants.NewYorkStrike,
                            GameEventConstants.OpzAnnualReport,
                        ],
                        requirements: [],
                        displayInfoResult: null) } };
            return new(
                code: Id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
actions: changeList);
        }

        private static Slide[] GetPrologSlides() => [
                GetSlide0(),
                GetSlide1(),
                GetSlide2(),
                GetSlide3(),
                GetSlide4(),
                GetSlide5(),
                GetSlide6(),
                GetSlide7(),
                GetSlide8(),
                GetSlide9(),
                GetSlide10(),
                GetSlide11()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Рассвет",
                imageName: ImageSet.SpaceElevator,
                text: new string[]
                {
                    "2073 год.",
                    "Десятки тысяч людей покинули Землю, чтобы добывать ресурсы в Поясе астероидов. Здесь уже почти сотня " +
                    "станций, и каждая — как маленькое государство: свои законы, налоги, порядки.",
                    "Вы — один из акционеров Консорциума Пояса, крупнейшей компании XXI века, владеющей большей частью " +
                    "станций в Поясе."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "О Поясе астероидов", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_2", "О Консорциуме Пояса", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Пояс астероидов",
                imageName: ImageSet.RegularTurn,
                text: new string[]
                {
                    "Пояс астероидов — регион между Марсом и Юпитером. Изначально сюда летели за платиноидами и " +
                    "редкоземельными металлами, которых на Земле почти не осталось. Сегодня Пояс — это не только " +
                    "добыча для Земли. Здесь перерабатывают ресурсы, строят станции и обеспечивают колонии " +
                    "почти всем необходимым.",
                    "Благодаря термоядерным двигателям путь от Земли до Пояса занимает всего один-два месяца."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "О Консорциуме Пояса", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Консорциум Пояса",
                imageName: ImageSet.ConsortiumLogo,
                text: new string[]
                {
                    "Консорциум Пояса основан в 2045 году тремя инвестиционными фондами, которые первыми поверили " +
                    "в добычу на астероидах. Сегодня он владеет большей частью станций и лицензий, а его флот " +
                    "поддерживает порядок там, где нет других законов.",
                    "Вместо жёсткого контроля сверху Консорциум подталкивает станции к конкуренции между собой, " +
                    "считая, что это ускоряет развитие. Акционеры становятся прямыми правителями колоний — как " +
                    "наиболее заинтересованные в их процветании."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "О Поясе астероидов", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Рассвет",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Четыре месяца назад Консорциум предложил вам пост правителя новой колонии — станции типа " +
                    "«Рассвет». Вы согласились.",
                    "На тот момент станция ещё строилась на верфи у астероида Психея."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "Зачем мне это?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "О станции", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_7", "Далее")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Почему я?",
                imageName: ImageSet.ConsortiumDialog2,
                text: new string[]
                {
                    "Консорциум предлагает посты правителей акционерам по очереди, начиная с самых крупных. " +
                    "Верхушка топа состоит из людей, у которых уже есть всё, что нужно. Они предпочитают оставаться " +
                    "на Земле, получая дивиденды без лишних хлопот. Очередь дошла до вас — и это отличный шанс.",
                    "К этому моменту уже больше пятидесяти акционеров согласились стать правителями — и их станции " +
                    "работают. Теперь ваш черёд."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_5", "Зачем мне это?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "О станции", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_7", "Далее")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Зачем мне это?",
                imageName: ImageSet.ConsortiumDialog,
                text: new string[]
                {
                    "Правитель получает фиксированную зарплату из бюджета колонии — около полумиллиона долларов " +
                    "в год до налогов. Если колония процветает, растёт и зарплата.",
                    "Но дело не только в деньгах. Это шанс построить колонию по собственным правилам — и оставить " +
                    "след в истории Пояса."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "О станции", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_7", "Далее")]);
        }

        private static Slide GetSlide6()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Станция «Рассвет»",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "«Рассвет» — самая популярная модель в Поясе. Вращающееся кольцо диаметром 150 метров создаёт " +
                    "гравитацию 0,85 g. Внутри — центральная улица, модульные трёхэтажные секции, компактный ядерный " +
                    "реактор и замкнутая система рециркуляции воды и воздуха.",
                    "Это полноценная платформа для добычи, производства и жизни в космосе, способная существовать " +
                    "автономно долгие месяцы.",
                    "Полная застройка вмещает до тысячи человек. Стартовая — около ста пятидесяти."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "Зачем мне это?"),
                    SlideButton.GetButtonToSlide($"{Id}_7", "Далее")]);
        }

        private static Slide GetSlide7()
        {
            return new Slide(
                id: $"{Id}_7",
                title: "Рассвет",
                imageName: ImageSet.CaptainHall,
                text: new string[]
                {
                    "Вы заступили на должность за три месяца до сдачи станции. Вместе с вами подготовкой занимался " +
                    "совет — четыре специалиста, отобранных Консорциумом.",
                    "Вам предстояло решить, куда транспортировать станцию, как добывать ресурсы и какие законы " +
                    "установить. С этого момента вы не один. Но решения — за вами."
                },
parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(
                        Id,
                        "Закрыть",
                        administratorSlideId: $"{Id}_8",
                        financierSlideId: $"{Id}_9",
                        engineerSlideId: $"{Id}_10",
                        socialSlideId: $"{Id}_11")]);
        }

        private static Slide GetSlide8()
        {
            return new Slide(
                id: $"{Id}_8",
                title: "Администратор",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла Селезнева, 34 года. Администратор.",
                    "Родилась в Новосибирске, отец работал на станции «Рубин». Опыт управления, связи на большинстве " +
                    "крупных станций Пояса. Знает Пояс изнутри.",
                    "Координирует работу станции, отвечает за связь с Консорциумом и замещает правителя " +
                    "в его отсутствие. Решает задачи, которые не входят в компетенцию других советников."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_7", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static Slide GetSlide9()
        {
            return new Slide(
                id: $"{Id}_9",
                title: "Финансист",
                imageName: ImageSet.Cassius,
                text: new string[]
                {
                    "Кассиус Морн аль-Хаким, 36 лет. Финансист и юрист.",
                    "Родился в Дубае, степень Лондонской школы экономики. Двенадцать лет в корпоративном праве " +
                    "и налоговом консалтинге. Холодный ум, знает устав Консорциума лучше, чем сам Консорциум.",
                    "Управляет бюджетом, налогами, контрактами и отчётностью перед Консорциумом. " +
                    "Отвечает за юридическую защиту колонии."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_7", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static Slide GetSlide10()
        {
            return new Slide(
                id: $"{Id}_10",
                title: "Инженер станции",
                imageName: ImageSet.Lien,
                text: new string[]
                {
                    "Лиен Чжан, 30 лет. Инженер станции.",
                    "Родилась в Циндао, в 19 лет улетела на Луну по программе RAS. Прошла путь от разнорабочей " +
                    "до старшего инженера. Ценит безопасность выше эффективности.",
                    "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции. " +
                    "Следит за безопасностью и предотвращает аварии."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_7", "Назад", kind: SlideButtonKind.Return)]);
        }

        private static Slide GetSlide11()
        {
            return new Slide(
                id: $"{Id}_11",
                title: "Социальный советник",
                imageName: ImageSet.Darius,
                text: new string[]
                {
                    "Дариус «Док» Уэбб, 42 года. HR и миграционный контролёр.",
                    "Родился в Хьюстоне, пятнадцать лет служил в миграционной службе ОПЗ на Луне. " +
                    "Считает, что каждый заслуживает шанса.",
                    "Отвечает за найм, удержание колонистов и внутренний климат. " +
                    "Решает конфликты между работниками."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_7", "Назад", kind: SlideButtonKind.Return)]);
        }
    }
}