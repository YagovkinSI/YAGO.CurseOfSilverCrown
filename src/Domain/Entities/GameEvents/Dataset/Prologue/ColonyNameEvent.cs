using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class ColonyNameEvent
    {
        private const string Id = nameof(ColonyNameEvent);

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                    { "#end", new GameEventChangeList(
                        colonyStats: [
                            new KeyValueParameter(ColonyStatNames.Industry_Administrative_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, -1040),
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, 10000)],
                        newQuests: [nameof(SkipPrologueEvent)],
                        availableRequirements: [
                            ActionAvailableRequirement.ActionPoints(1)]) } };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList),
                changeList: changeList,
                isImmediatelyEvent: true,
                isAutostartEvent: true);
        }

        private static Episode GetEpisode(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Episode(
                slides: GetPrologSlides(changeList));
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameEventChangeList> changeList)
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
                    parameters: [],
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
                    parameters: [],
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
                parameters: [],
                buttons: [
                    SlideButton.GetSetChoiceButtonForTextInput(Id, isInputCompleted: true, "Назвать" ),
                    SlideButton.GetSetChoiceButtonForTextInput(Id, isInputCompleted: false, "Пока не решил")],
                textInput: new SlideTextInput());
        }
    }
}
