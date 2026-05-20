using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.Quests;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class ColonyNameQuest
    {
        public static Quest Get()
        {
            var id = nameof(ColonyNameQuest);
            var name = "Рассвет";
            return new(
                id: id,
                name: name,
                QuestType.Required,
                requirements: [
                    new RequirementsParameter(ColonyStatNames.EpisodeCount, 0, isTopThreshold : true)
                ],
                chanceDefault: 1,
                chanceModifiers: [],
                prologueSlide: GetPrologueSlide(name),
                completeEpisode: GetEpisode(id, name));
        }

        private static Slide GetPrologueSlide(string name)
        {
            return new Slide(
                title: name,
                imageName: ImageSet.Station_1,
                text: new string[] {
                    "Дай название своей будущей колонии."},
                parameters: [],
                textInput: new TextInput(),
                continueButtonName: "Назвать");
        }

        private static Episode GetEpisode(string id, string name)
        {
            return new Episode(
                id: id,
                title: name,
                prologSlides: GetPrologSlides(name),
                dilemma: null);
        }

        private static PrologueSlide[] GetPrologSlides(string name)
        {
            return [
                new PrologueSlide(
                title: name,
                imageName: ImageSet.EarthLeaving,
                text:
                [
                    "2183 год. Миллионы людей покинули Землю добывать руду и лёд в Поясе Астероидов. Там уже десятки тысяч колоний. " +
                    "Каждая — как маленькое государство: свои законы, налоги, порядки.",
                    "В Поясе власть принадлежит частным правителям и корпорациям. Государства Земли почти потеряли своё влияние.",
                    "С опытным советником ты улаживаешь последние формальности по кредиту на создание твоей собственной колонии."
                ],
                parameters: [],
                continueButtonName: "Далее"),

                new PrologueSlide(
                    title: name,
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
                    continueButtonName: "Подписать контракт"),


                new PrologueSlide(
                title: name,
                imageName: ImageSet.Station_1,
                text: new string[] {
                    "Камилла собирает подписанные документы:",
                    "«Поздравляю. Впереди — великое бумажное побоище: пройти регистрацию, получить лицензию, набрать команду. " +
                    "Поверь, месяцы пролетят незаметно. Уже решил, как назовёшь колонию?»",
                    "Ты немало ночей провёл в раздумьях. И сейчас у тебя был готов ответ."},
                parameters: [new KeyValueParameter(ColonyStatNames.EpisodeCount, 1)],
                continueButtonName: "Назвать")];
        }
    }
}
