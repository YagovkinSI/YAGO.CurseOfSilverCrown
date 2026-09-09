using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

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
                    effects: [],
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
                    "Камилла настоятельно рекомендовала не затягивать с вылетом: путь до орбиты и дальше, до Пояса, " +
                    "займёт почти два месяца. Пока вы будете добираться до космопорта, она проведёт разведку по астероидам — " +
                    "соберёт данные по составу породы, удалённости и потенциалу добычи.",
                    "Чем раньше вы получите её отчёт, тем быстрее сможете принять решение, куда направлять станцию. " +
                    "Время — деньги, особенно в Поясе."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Почему такая спешка?"),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отправляемся")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Путь в Пояс",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Станция уже почти построена, но её ещё предстоит дооснастить и доставить к астероиду. " +
                    "Транспортировка с верфи Психеи займёт несколько недель — и чем дальше окажется цель, тем дольше " +
                    "она будет в пути.",
                    "С орбиты Земли открываются три направления: Церера — главный хаб Пояса, Психея — где располагается " +
                    "главная верфь Пояса, Веста — где пока всего около десятка станций. Астероид нужно выбрать до того, " +
                    "как вы покинете орбиту.",
                    "Камилла уже собирает данные по кандидатам и обещает прислать их в ближайшие дни."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Отправляемся")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "ИИ-агент уже прокладывает ваш маршрут: резервирует билеты, синхронизирует стыковки, готовит " +
                    "инструкции.",
                    "Для рабочих с бюджетной релокацией это несколько пересадок и около суток в пути. Для вас — " +
                    "частный гиперзвуковой самолёт, который доставит вас в любую точку мира за несколько часов.",
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
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "К 2073 году искусственный интеллект развился, но не так стремительно, как прогнозировали " +
                    "в 2030-х. Люди переключились на космос: ресурсы, инвестиции и таланты ушли в колонизацию, " +
                    "а не в создание сверхразума.",
                    "Тем не менее ИИ-агенты стали неотъемлемой частью жизни. В дешёвых магазинах мегаполисов не " +
                    "осталось людей-продавцов. Людей-таксистов почти нет в эконом-классе — автопилоты давно " +
                    "заменили их на массовых маршрутах. Между городами курсируют автономные грузовики.",
                    "Рабочих мест для людей становится всё меньше."
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
                    "занятость через программы «социально-полезного труда»: парки, культура, уход за пожилыми. " +
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
