using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class JourneyStartEvent
    {
        private const string Id = GameEventConstants.JourneyStart;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { "#default", new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SpendSolars, 6),
                        new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeSpaceElevator)],
                    newEventCodes: [GameEventConstants.SkipPrologue],
                    displayInfoResult: GetEpilog()) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
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
                GetSlide6()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Путь в Пояс",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла ознакомилась с данными по строительству станции. В течение недели она " +
                    "проведёт анализ астероидов Пояса — соберёт данные по потенциалу добычи и удаленности " +
                    "от логистических узлов. Как только данные будут готовы, она пришлёт несколько " +
                    "вариантов на выбор, чтобы вы могли определиться, где будет вестись разработка.",
                    "Она настоятельно рекомендует не затягивать с вылетом в Пояс и отправиться к " +
                    "орбитальному космопорту в ближайшие дни."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Почему такая спешка?"),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отправляюсь")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Путь в Пояс",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Станция ещё не завершена, но транспортировку к астероиду можно начинать " +
                    "уже в ближайшие недели — внутренние работы завершат в пути или уже у " +
                    "астероида. Если цель окажется далеко от верфи у Психеи, перелёт может занять " +
                    "больше двух месяцев. Решение нужно принимать быстро, чтобы не сдвигать " +
                    "сроки открытия.",
                    "Ваш собственный путь от Земли до станции займёт почти столько же — около двух " +
                    "месяцев. И к тому моменту, как вы окажетесь на орбите Земли, у вас уже должно " +
                    "быть понимание, в какую часть Пояса направляться."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отправляюсь")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Вы простились с близкими и через три дня отправились в путь.",
                    "Для большинства людей это несколько пересадок и сутки в пути. Для вас — частный гиперзвуковой " +
                    "самолёт, который за несколько часов доставляет вас прямым рейсом на платформу космического " +
                    "лифта на экваторе в Тихом океане.",
                    "Платформа лифта — это плавучий стальной остров. Терминал не похож на шумные аэропорты " +
                    "Земли: функционально, тихо, без толп."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_3", "К лифту")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Вы занимаете VIP-каюту в капсуле лифта. Большая часть пассажиров едет в скромных спальных " +
                    "капсулах.",
                    "Лифт мягко трогается, и океан начинает уходить вниз. В течение первых часов гравитация постепенно " +
                    "исчезает — начинается настоящая невесомость. Остаток пути до геостационарной орбиты займёт " +
                    "почти десять дней."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Почему так долго?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "Куда летят люди?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "О лифте", kind: SlideButtonKind.Reference),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Подъём на геостационарную орбиту — это 36 000 километров, почти как кругосветное путешествие. " +
                    "Капсула движется со скоростью около 150–200 км/ч — быстрее нельзя: на такой высоте любое " +
                    "ускорение повышает риск аварии. Десять дней — это цена комфорта и безопасности."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_5", "Куда летят люди?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "О лифте", kind: SlideButtonKind.Reference),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Большая часть пассажиров лифта остаются на орбите — отели, верфи, научные станции. " +
                    "Четверть направляется в Пояс, к десяткам тысяч колонистов. Остальные — на Луну, где добывают " +
                    "гелий-3 для термоядерных реакторов, на Марс и дальние станции.",
                    "Пояс — не самый популярный маршрут, но самый быстрорастущий."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Почему так долго?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "О лифте", kind: SlideButtonKind.Reference),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }

        private static Slide GetSlide6()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Космический лифт — величайшее инженерное сооружение в истории человечества. Трос из углеродных " +
                    "нанотрубок длиной 90 000 километров удерживает противовес на орбите. Шесть нитей лифта поднимают " +
                    "до 26 000 тонн грузов и 17 000 пассажиров в год — это главные ворота человечества в космос."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Почему так долго?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "Куда летят люди?"),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }

        private static DisplayInfo GetEpilog()
        {
            return new DisplayInfo(
                name: "Путь в Пояс",
                imageName: ImageSet.Space,
                description: [
                    "Расходы на перелёт и VIP-каюту списаны из бюджета колонии.",
                    "В пути есть доступ к связи, так что вы можете связаться с Камиллой или близкими. Но пока — " +
                    "десять дней подъёма, звёзды за иллюминатором и время подумать о том, что ждёт впереди."
                ]);
        }
    }
}
