using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class StartColonyEvent
    {
        private const string Id = GameEventConstants.StartColonyEvent;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.SetColonyName),
                            new GameEffect(GameEffectType.AddSolars, 10_000),
                            new GameEffect(GameEffectType.AddPublicDebt, 30_000)],
                        newEventCodes: [nameof(SkipPrologueEvent)],
                        requirements: [
                            GameRequirement.ActionPointsMoreThan(1)]) } };
            return new(
                code: Id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: GetPrologSlides(changeList),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameAction> changeList) => [
                GetSlide0(),
                GetSlide1(),
                GetSlide2(),
                GetSlide3(),
                GetSlide4(),
                GetSlide5()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Рассвет",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "2073 год.",
                    "Десятки тысяч людей покинули Землю, чтобы добывать ресурсы в Поясе астероидов. Здесь уже почти сотня станций, и каждая — как маленькое государство: свои законы, налоги, порядки.",
                    "В Поясе власть принадлежит частным правителям и корпорациям. Государства Земли почти потеряли своё влияние.",
                    "Вы — акционер Консорциума Пояса. Сегодня вам предстоит подписать контракт, который сделает вас правителем новой станции."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Рассвет",
                imageName: ImageSet.ConcEarchOffice,
                text: new string[]
                {
                    "Офис Консорциума. На столе — контракт. Напротив — координатор компании.",
                    "«Станция «Рассвет-342» готова на 90%. Приёмка через три месяца. Жилые модули рассчитаны на 150 человек, с возможностью расширения до тысячи.»",
                    "«Ваша задача — набрать команду, запустить добычу и сделать колонию прибыльной. Стартовый бюджет выделен, но помните: станция куплена в кредит, и проценты по долгу будут основным расходом в первые годы.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Рассвет",
                imageName: ImageSet.Station_1,
                text: new string[] {
                    "«Готовы подписать контракт? Или остались вопросы?»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Подписать контракт"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Зачем мне это?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "В контракте указан кредит...")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Рассвет",
                imageName: ImageSet.ConcEarchOffice,
                text: new string[] {
                    "«Консорциум предлагает посты правителей акционерам по очереди, начиная с самых крупных. Верхушка топа состоит из людей, у которых уже есть всё, что нужно. Они предпочитают оставаться на Земле или Церере, получая дивиденды без лишних хлопот. Очередь дошла до вас — и это отличный шанс.",
                    "К этому моменту уже больше пятидесяти акционеров согласились стать правителями — и их станции работают. Теперь ваш черёд.»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Подписать контракт"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Зачем мне это?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "В контракте указан кредит...")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Рассвет",
                imageName: ImageSet.ConcEarchOffice,
                text: new string[] {
                    "«Правитель получает фиксированную зарплату из бюджета колонии — 40 Солар в год до налогов. Это около полумиллиона долларов. Неплохое вознаграждение за управление собственной станцией.",
                    "А если колония процветает — растёт и ваша зарплата.»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Подписать контракт"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "В контракте указан кредит...")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Рассвет",
                imageName: ImageSet.Station_1,
                text: new string[] {
                    "«Станция куплена за счёт кредита. Тело долга остаётся, а колония платит проценты — фиксированную сумму каждый год.",
                    "Это не ваш личный долг, а долг колонии. Он ложится на бюджет станции, но вы не отвечаете по нему лично. Когда колония начнёт приносить прибыль, сможете гасить долг быстрее.»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Зачем мне это?")]);
        }
    }
}
