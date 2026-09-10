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
                        new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeEarth2070)],
                    newEventCodes: [GameEventConstants.JourneyToElevator],                    
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
                GetSlide4()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Путь в Пояс",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла ознакомилась с данными по строительству станции. В течении недели она " +
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
                    "астероида. Если цель окажется далеко от верфи Психеи, перелёт может занять " +
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
                    "ИИ-агент прокладывает ваш маршрут к космическому лифту: резервирует билеты, синхронизирует " +
                    "стыковки, готовит инструкции.",
                    "Для рабочих с бюджетной релокацией это несколько пересадок и около суток в пути. Для вас — " +
                    "частный гиперзвуковой самолёт, который доставит вас в любую точку Земли за несколько часов.",
                    "ИИ-агент предлагает вылететь через три дня. В городе как раз закончится очередная забастовка " +
                    "против автоматизации — а у вас будет время попрощаться с близкими."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_3", "Развитие ИИ"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Протесты"),
                    SlideButton.GetCloseNewsButton(Id, "Завершить")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Путь в Пояс",
                imageName: ImageSet.CitizenInVR,
                text: new string[]
                {
                    "К 2073 году искусственный интеллект не стал сверхразумом, но прочно вошёл в " +
                    "повседневную жизнь. Основные инвестиции и таланты ушли в колонизацию, а не " +
                    "в создание общего ИИ.",
                    "Тем не менее ИИ-агенты стали привычными помощниками. Они следят за запасами " +
                    "в холодильнике и заказывают продукты, управляют клинингом и роботами-уборщиками, " +
                    "подбирают одежду по погоде, бронируют столики и покупают билеты по голосовой " +
                    "команде. Это уже не футуризм — это обычный день жителя мегаполиса."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Протесты"),
                    SlideButton.GetCloseNewsButton(Id, "Завершить")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Автоматизация и роботизация заменили миллионы людей в сфере обслуживания, производства и " +
                    "логистики. Безработица на Земле достигла 9,3% — и продолжает расти.",
                    "Профсоюзы требуют сохранить рабочие места, а сторонники базового дохода считают, что " +
                    "автоматизация — это шанс освободить людей от рутины. Правительства искусственно поддерживают " +
                    "занятость через программы социально-полезного труда: парки, культура, уход за пожилыми. " +
                    "Это позволяет сглаживать напряжение, но проблему не решает.",
                    "А те, кто готов работать в Поясе, просто улетают."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_3", "Развитие ИИ"),
                    SlideButton.GetCloseNewsButton(Id, "Завершить")]);
        }

        private static DisplayInfo GetEpilog()
        {
            return new DisplayInfo(
                name: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                description: ["ИИ-агент подтвердил бронирование и списал деньги. Через три дня вы покинете Землю."]);
        }
    }
}
