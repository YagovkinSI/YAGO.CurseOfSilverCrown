using System;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prolog
{
    internal static class ColonyNameEvent
    {
        public static GameEvent Get()
        {
            var id = "ColonyNameEvent";
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
                imageName: ImageSet.Feature,
                text: new string[]
                {
                    "2183 год. Национальные государства пали. Их место заняли корпорации и владельцы космических городов.",
                    "В Поясе Астероидов уже десятки тысяч колоний. Миллионы людей покинули Землю, чтобы работать там — " +
                    "где добыча руды и льда кормит всю Солнечную систему.",
                    "Каждая колония — как маленькое государство. Свои законы, налоги, порядки. Скоро и ты станешь правителем " +
                    "своей собственной колонии."
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
                choice: [
                    GetChoice()
                ],
                choiceLabel: new string[]
                {
                    "Камилла собирает подписанные документы:",
                    "«Поздравляю. Впереди — великое бумажное побоище: пройти регистрацию, получить лицензию, набрать команду. " +
                    "Поверь, месяцы пролетят незаметно. Уже решил, как назовёшь колонию?»",
                    "Ты немало ночей провёл в раздумьях. И сейчас у тебя был готов ответ."
                });
        }

        private static Choice GetChoice()
        {
            return new Choice(
                id: Guid.Parse("99355251-e17f-45cf-8c2d-066eb4970719"),
                title: "Рассвет",
                imageName: ImageSet.Station_1,
                text: [],
                parameters: [
                    new KeyValueParameter(ColonyStatNames.EpisodeCount, 1)],
                buttonName: "Произнести название");
        }
    }
}
