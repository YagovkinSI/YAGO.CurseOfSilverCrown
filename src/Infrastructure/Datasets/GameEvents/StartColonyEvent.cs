using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
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
            var displayInfoResult = GetDisplayInfoResult();
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.AddSolars, 10_000),
                            new GameEffect(GameEffectType.AddPublicDebt, 30_000),
                            new GameEffect(GameEffectType.SetAchievement, code: AchievementConstants.RulerContractSigned),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.Ship_1)],
                        newEventCodes: [nameof(SkipPrologueEvent)],
                        requirements: [
                            GameRequirement.ActionPointsMoreThan(1)],
                        displayInfoResult) } };
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
                    "«Станция сейчас на завершающей стадии строительства — приёмка через три месяца. Но работа начинается уже сейчас: вам предстоит набирать команду, планировать развитие и готовить колонию к запуску.",
                    "Консорциум выделил крупный кредит на строительство станции и стартовый капитал. Проценты по кредиту — основная статья расхода с самого первого дня. Ваша зарплата также идёт из бюджета колонии. Всё это создаёт дефицит, и покрывать его придётся доходами от добычи.»"
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
                imageName: ImageSet.ConcEarchOffice,
                text: new string[] {
                    "«Готовы подписать контракт? Или остались вопросы?»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Подписать контракт"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Зачем мне это?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "Поясните про кредит")]);
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
                    SlideButton.GetButtonToSlide($"{Id}_5", "Поясните про кредит")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Рассвет",
                imageName: ImageSet.ConcEarchOffice,
                text: new string[] {
                    "«Правитель получает фиксированную зарплату из бюджета колонии — около полумиллиона долларов в год до налогов. Неплохое вознаграждение за управление собственной станцией.",
                    "А если колония процветает — растёт и ваша зарплата.»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Подписать контракт"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_5", "Поясните про кредит")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Рассвет",
                imageName: ImageSet.Station_1,
                text: new string[] {
                    "«Это не ваш личный долг, а долг колонии. Что-то вроде государственного долга на Земле.",
                    "Консорциум учитывает все расходы на станцию — строительство, оборудование, стартовый капитал. Это нужно, чтобы понимать, насколько колония эффективна: приносит ли она прибыль сверх затрат или работает в убыток.",
                    "Вам не нужно гасить этот долг из своего кармана. Он погашается из бюджета колонии, когда появляется прибыль. А до тех пор вы просто платите проценты — это часть расходов, заложенных в бизнес-план.»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Подписать контракт"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Почему я?"),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Зачем мне это?")]);
        }

        private static DisplayInfo GetDisplayInfoResult()
        {
            return new DisplayInfo(
                name: "Рассвет",
                imageName: ImageSet.ConcEarchOffice,
                description: [
                    "Сотрудник принимает подписанный контракт.",
                    "«Поздравляю. Советую сразу заняться поиском опытного советника — " +
                    "без местных связей и знаний вы быстро утонете в бумагах и интригах."]);
        }
    }
}
