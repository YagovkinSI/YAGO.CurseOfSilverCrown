using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class ColonyNameEvent
    {
        public static GameEvent Get()
        {
            var id = nameof(ColonyNameEvent);
            return new(
                id: id,
                chanceDefault: 1,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.EpisodeCount, 0, isTopThreshold : true)
                ],
                parameterModifiers: [],
                episode: GetEpisode(id));
        }

        private static Episode GetEpisode(string id)
        {
            return new Episode(
                id: id,
                title: "Рассвет",
                prologSlides: GetPrologSlides(),
                dilemma: GetDilemma());
        }

        private static PrologueSlide[] GetPrologSlides()
        {
            return [
                new PrologueSlide(
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
                continueButtonName: "Далее"),

                new PrologueSlide(
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
                parameters: [],
                continueButtonName: "Подписать контракт")];
        }

        private static Dilemma GetDilemma()
        {
            return new DilemmaTextInput(
                slide: new Slide(
                    title: "Рассвет",
                    imageName: ImageSet.Station_1,
                    text: new string[] {
                        "Камилла собирает подписанные документы:",
                        "«Поздравляю. Впереди — великое бумажное побоище: пройти регистрацию, получить лицензию, набрать команду. " +
                        "Поверь, месяцы пролетят незаметно. Уже решил, как назовёшь колонию?»",
                        "Ты немало ночей провёл в раздумьях. И сейчас у тебя был готов ответ."},
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.EpisodeCount, 1)]),
                submitButtonName: "Назвать");
        }
    }
}
