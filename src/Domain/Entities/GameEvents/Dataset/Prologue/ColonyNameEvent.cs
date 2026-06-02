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
                requirements: [
                    new RequirementsParameter(ColonyStatNames.EpisodeCount, 0, isTopThreshold : true)
                ],
                chanceDefault: 1,
                chanceModifiers: []);
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(),
                isImmediatelyEvent: true);
        }

        private static Episode GetEpisode()
        {
            return new Episode(
                slides: GetPrologSlides());
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                new Slide(
                    id: $"{Id}_0",
                    title: "Рассвет",
                    imageName: ImageSet.EarthLeaving,
                    text: new string[]
                    {
                        "2183 год. Миллионы людей покинули Землю добывать руду и лёд в Поясе Астероидов. Там уже десятки тысяч колоний. " +
                        "Каждая — как маленькое государство: свои законы, налоги, порядки.",
                        "В Поясе власть принадлежит частным правителям и корпорациям. Государства Земли почти потеряли своё влияние.",
                        "С опытным советником ты улаживаешь последние формальности по кредиту на создание твоей собственной колонии."
                    },
                    parameters: [],
                    buttons: [
                        SlideButton.GetButtonToSlide($"{Id}_1")]),

                new Slide(
                    id: $"{Id}_1",
                    title: "Рассвет",
                    imageName: ImageSet.Camilla,
                    text: new string[]
                    {
                        "Офис корпорации «Астер-Инвест» на Земле. Ты сидишь напротив менеджера. Рядом с тобой Камилла, твой советник, " +
                        "просматривает кредитный контракт.",
                        "«Станция \"Рассвет-782\" — современный эталон. Жилые модули на 150 человек и возможность расширить до тысячи. " +
                        "Готовность через полгода. Идеальный запас, чтобы пройти девять кругов бюрократии и быть готовыми к открытию. " +
                        "Всё хорошо.»"
                    },
                    parameters: [
                            new KeyValueParameter(ColonyStatNames.Industry_Administrative_Companies, 1),
                            new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, -20),
                            new KeyValueParameter(ColonyStatNames.Economic_Reserves, 1000)],
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
                    "Камилла собирает подписанные документы:",
                    "«Поздравляю. Впереди — великое бумажное побоище: пройти регистрацию, получить лицензию, набрать команду. " +
                    "Поверь, месяцы пролетят незаметно. Уже решил, как назовёшь колонию?»",
                    "Ты немало ночей провёл в раздумьях. И сейчас у тебя был готов ответ."},
                parameters: [new KeyValueParameter(ColonyStatNames.EpisodeCount, 1)],
                buttons: [
                    SlideButton.GetSetChoiceButtonForTextInput(Id, "Назвать")],
                textInput: new SlideTextInput());
        }
    }
}
