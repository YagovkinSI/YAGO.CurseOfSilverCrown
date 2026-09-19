using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Domain.Stations;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class AsteroidChoiceEvent
    {
        private const string Id = GameEventConstants.AsteroidChoice;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { AsteroidConstants.Large, new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SetAsteroid, code: AsteroidConstants.Large),
                        ..GetDefaultEffects()],
                    newEventCodes: [],
                    displayInfoResult: GetEpilog("крупный")) },
                { AsteroidConstants.Medium, new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SetAsteroid, code: AsteroidConstants.Medium),
                        ..GetDefaultEffects()],
                    newEventCodes: [],
                    displayInfoResult: GetEpilog("средний")) },
                { AsteroidConstants.Small, new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SetAsteroid, code: AsteroidConstants.Small),
                        ..GetDefaultEffects()],
                    newEventCodes: [],
                    displayInfoResult: GetEpilog("малый")) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetSlides(),
                actions: changeList);
        }

        private static Slide[] GetSlides() => [
                GetSlide0(),
                GetSlide1(),
                GetSlide2(),
                GetSlide3(),
                GetSlide4(),
                GetSlide5(),
                GetSlide6(),
                GetSlide7()];

        private static GameEffect[] GetDefaultEffects()
        {
            return [
                new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.FactionAvalon),
                new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.FactionPhoenix)];
        }

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Выбор астероида",
                imageName: ImageSet.Belt,
                text: new string[]
                {
                    "Одним из первых серьёзных решений стал выбор астероида, на котором колония будет вести " +
                    "добычу. Его обсуждали не один день: Камилла собирала данные и предлагала варианты, " +
                    "Кассиус сверял расчёты рентабельности.",
                    "Совет быстро сошёлся: добыча платиноидов — самое выгодное направление. Крупнейшие " +
                    "месторождения рядом с Вестой, но лучшие астероиды уже заняты. Войти туда — значит быть " +
                    "одним из многих."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Далее")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Выбор астероида",
                imageName: ImageSet.Belt,
                text: new string[]
                {
                    "Перебрав варианты, решили остановиться на кантоне Авалона. Всего несколько дней перелёта " +
                    "от крупнейшего хаба Пояса — Цереры. Есть несколько хороших астероидов, богатых платиной, " +
                    "и всего пара соседей: колонии «Авалон» и «Феникс».",
                    "Перспективный кантон, который явно будет расти. У нас есть шанс занять в нём центральное " +
                    "место."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "О колонии Авалон", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_3", "О колонии Феникс", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Далее")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Авалон",
                imageName: ImageSet.Station_2,
                text: new string[]
                {
                    "«Авалон» — старшая колония кантона. Основана семь лет назад, население — около тысячи " +
                    "двухсот человек. Добывают платиноиды, используют дешёвый труд. Управляет Лоренцо Хейл, " +
                    "крупный акционер Консорциума.",
                    "Астероид истощается: верхние слои выработаны, себестоимость растёт. Хейл либо сменит " +
                    "специализацию, либо переедет на соседний астероид. Не любит конкурентов."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_3", "О колонии Феникс", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Далее")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Феникс",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "«Феникс» — молодая колония у ледяного астероида. Около трёхсот человек. Добывают водяной " +
                    "лёд, возят на Цереру сами — дорого. С Авалоном скооперироваться не смогли: не сошлись " +
                    "в условиях.",
                    "Управляет Соня Вильде. Колония почти не развивается, но люди живут неплохо. Если " +
                    "наладить отношения — могут стать поставщиком воды и топлива."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "О колонии Авалон", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_4", "Далее")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Выбор астероида",
                imageName: ImageSet.Belt,
                text: new string[]
                {
                    "Самым сложным оказалось выбрать конкретный астероид. Было три серьёзных претендента.",
                    "Первый — крупный, как у Авалона, но ещё на сутки дальше от Цереры. Хороший вариант, " +
                    "если специализироваться на добыче.",
                    "Второй — меньше, но рядом с Авалоном. Сбалансированный выбор.",
                    "Третий — малый, но ближе к Церере. Можно попытаться стать вратами в кантон и вырасти " +
                    "до серьёзного хаба."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton(AsteroidConstants.Large, "Первый — крупный", financierSlideId: $"{Id}_5"),
                    SlideButton.GetSetChoiceButton(AsteroidConstants.Medium, "Второй — средний", socialSlideId: $"{Id}_6"),
                    SlideButton.GetSetChoiceButton(AsteroidConstants.Small, "Третий — малый", administratorSlideId: $"{Id}_7")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Крупный астероид",
                imageName: ImageSet.Cassius,
                text: new string[]
                {
                    "Кассиус не колебался ни секунды.",
                    "«Двенадцать модулей добычи — это максимальная прибыль. Не меньше, чем у Авалона. " +
                    "Мы построим промышленную базу с расчётом на десятилетия и будем качать руду, пока соседи " +
                    "считают копейки. Да, он дальше от Цереры — четыре с лишним суток. Но расходы на логистику " +
                    "легко покроются объёмом добычи.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Вернуться к списку", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(AsteroidConstants.Large)]);
        }

        private static Slide GetSlide6()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Средний астероид",
                imageName: ImageSet.Darius,
                text: new string[]
                {
                    "Док пожал плечами.",
                    "«Девять модулей — достаточно, чтобы не беспокоиться о бюджете. А расположение — центр " +
                    "кантона, рядом с Авалоном. Это шанс наладить торговлю, заключить союз или " +
                    "использовать его инфраструктуру. Камилла права — логистика важна. Кассиус прав — " +
                    "добыча важна. Так зачем выбирать? Возьмите и того, и другого. Хороший компромисс.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Вернуться к списку", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(AsteroidConstants.Medium)]);
        }

        private static Slide GetSlide7()
        {
            return new Slide(
                id: $"{Id}_7",
                title: "Малый астероид",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла смотрела на карту чуть дольше остальных.",
                    "«Семь модулей — это мало. Но посмотрите, где он. Ближе всех к Церере, прямо на маршруте " +
                    "к Авалону. Это идеальный перевалочный пункт. Через нас пойдут грузы Феникса, потом — " +
                    "других колоний кантона. Мы можем стать вратами в кантон. Не просто добывающей станцией — " +
                    "хабом.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Вернуться к списку", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(AsteroidConstants.Small)]);
        }

        private static DisplayInfo GetEpilog(string asteroidName) => new(
            name: "Выбор астероида",
            imageName: ImageSet.Station_1,
            description:
            [
                $"Вы выбрали {asteroidName} астероид. Вскоре модули станции отправились с верфи Психеи к выбранному астероиду. " +
                $"Строительные работы продолжались в пути, а окончательная сборка прошла уже на месте."
            ]);
    }
}