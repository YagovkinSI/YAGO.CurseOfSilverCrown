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
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, -20),
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, 1000)],
                        newQuests: [nameof(SkipPrologueEvent)],
                        availableRequirements: [
                            ActionAvailableRequirement.ActionPoints(1)]) } };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList),
                changeList: changeList,
                isImmediatelyEvent: true);
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
                        "2073 год. Тысячи людей покинули Землю, чтобы работать в Поясе Астероидов. " +
                        "Там уже 76 станций и 32 тысячи колонистов. Каждая станция — как маленькое государство: " +
                        "свои законы, налоги, порядки.",
                        "В Поясе власть принадлежит частным правителям и корпорациям. " +
                        "Государства Земли почти потеряли своё влияние.",
                        "Ты — один из акционеров Консорциума Пояса. И сегодня ты подписываешь контракт, " +
                        "который сделает тебя правителем новой станции."
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
                        "Офис Консорциума на Земле. На столе — контракт. " +
                        "Напротив тебя — координатор компании.",
                        "«Станция «Рассвет-342» — современный проект RAS. " +
                        "Диаметр 150 метров, жилые модули на 150 человек с возможностью расширения до тысячи. " +
                        "Строительство завершено на 90%, приёмка через три месяца.",
                        "Стартового бюджета с запасом хватит, чтобы нанять команду и запустить добычу. " +
                        "Всё, что осталось — ваша подпись.»"
                    },
                    parameters: changeList["#end"].ColonyStats,
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
                    "«Поздравляю, правитель. Впереди — много дел: набрать команду, " +
                    "получить лицензию на добычу, пройти приёмку станции. Поверь, три месяца пролетят незаметно.",
                    "Уже решили, как назовёте колонию?»",
                    "Ты немало ночей провёл в раздумьях. И сейчас у тебя был готов ответ."},
                parameters: [],
                buttons: [
                    SlideButton.GetSetChoiceButtonForTextInput(Id, "Назвать")],
                textInput: new SlideTextInput());
        }
    }
}
