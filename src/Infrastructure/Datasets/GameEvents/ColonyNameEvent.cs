using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class ColonyNameEvent
    {
        private const string Id = GameEventConstants.StartColonyEvent;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#end", new GameAction(
                        changes: [
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
                changeList: changeList);
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameAction> changeList)
        {
            return [
                new Slide(
                    id: $"{Id}_0",
                    title: "Рассвет",
                    imageName: ImageSet.EarthLeaving,
                    text: new string[]
                    {
                        "2073 год. Десятки тысяч людей покинули Землю, чтобы добывать ресурсы на астероидах в Поясе. " +
                        "Там уже почти сотня станций, и каждая — как маленькое государство: свои законы, налоги, порядки.",
                        "В Поясе власть принадлежит частным правителям и корпорациям. " +
                        "Государства Земли почти потеряли своё влияние.",
                        "Ты — один из акционеров Консорциума Пояса. " +
                        "И сегодня ты подписываешь контракт, который сделает тебя правителем новой станции."
                    },
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1")]),

                new Slide(
                    id: $"{Id}_1",
                    title: "Рассвет",
                    imageName: ImageSet.ConcEarchOffice,
                    text: new string[]
                    {
                        "Офис Консорциума. На столе — контракт. Напротив тебя — координатор компании.",
                        "«Станция «Рассвет-342» — готова на 90%, приёмка через три месяца. " +
                        "Жилые модули рассчитаны на 150 человек, но есть возможность расширяться до тысячи. " +
                        "Ваша задача — набрать команду, запустить добычу и сделать колонию прибыльной.",
                        "Стартового бюджета с запасом хватит на первые шаги. Всё, что осталось — ваша подпись.»"
                    },
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_2", "Подписать контракт")]),

                GetDilemma()];
        }

        private static Slide GetDilemma()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Рассвет",
                imageName: ImageSet.Station_1,
                text: new string[] {
                    "Сотрудник принимает подписанный контракт.",
                    "«Поздравляю. Советую сразу заняться поиском опытного советника — " +
                    "без местных связей и знаний вы быстро утонете в бумагах и интригах. " +
                    "Поверьте, три месяца до приёмки пролетят незаметно.",
                    "Уже решили, как назовёте колонию?»"},
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButtonForTextInput(isInputCompleted: true, "Назвать" ),
                    SlideButton.GetSetChoiceButtonForTextInput(isInputCompleted: false, "Пока не решил")],
                textInput: new SlideTextInput());
        }
    }
}
