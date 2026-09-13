using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

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
                { "Large", new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SpendSolars, 1300)],
                    newEventCodes: [GameEventConstants.SkipPrologue],
                    displayInfoResult: GetEpilog()) },
                { "Medium", new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SpendSolars, 1300)],
                    newEventCodes: [GameEventConstants.SkipPrologue],
                    displayInfoResult: GetEpilog()) },
                { "Small", new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SpendSolars, 1300)],
                    newEventCodes: [GameEventConstants.SkipPrologue],
                    displayInfoResult: GetEpilog()) } };
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
                GetSlide6()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Выбор астероида",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Сообщение от Камиллы.",
                    "«Завершила анализ. Думаю, нам стоит сосредоточиться на добыче платиноидов. Рядом с Вестой " +
                    "выбора больше, но лучшие астероиды уже заняты. Войти туда — значит быть одним из многих.",
                    "Предлагаю кантон Авалона. Это соседний с Церерой кантон: несколько дней пути. Он не так богат, " +
                    "но и не так занят. Всего две колонии. Данные о них приложила.",
                    "Если мы закрепимся здесь сейчас, у нас есть шанс со временем занять в нём центральное место»."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Об Авалоне"),
                    SlideButton.GetButtonToSlide($"{Id}_2", "О Фениксе"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Авалон",
                imageName: ImageSet.Station_2,
                text: new string[]
                {
                    "Управляет Лоренцо Хейл — крупный акционер Консорциума. Открыл «Авалон» семь лет назад, взяв один " +
                    "из крупнейших платиноидов, близких к Церере. Предпочитает использовать дешёвый труд.",
                    "Но сейчас астероид истощается: верхние слои выработаны, себестоимость растёт. Пару лет назад " +
                    "Хейл сменил станцию на «Резолют» и увеличил население до 1200 человек, открыв дешёвые производства. " +
                    "Какие у него дальнейшие планы — неизвестно."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "О Фениксе"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Феникс",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Молодая колония у ледяного астероида. «Феникс» возит водяной лёд на Цереру сам, через 0,45 а.е. Дорого. " +
                    "Видимо, не смогли скооперироваться с Авалоном.",
                    "Если построить станцию рядом, они могут стать поставщиком воды и топлива. Или мы — их транзитным узлом."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Об Авалоне"),
                    SlideButton.GetButtonToSlide($"{Id}_3", "Далее")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Выбор астероида",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла продолжает.",
                    "«Подобрала три астероида на выбор.",
                    "Первый — крупный, как у Авалона, но ещё на сутки дальше от Цереры. Если хотите специализироваться " +
                    "на добыче.",
                    "Второй — меньше, но рядом с Авалоном. Сбалансированный вариант.",
                    "Третий — малый, но ближе к Церере. Если желаете стать хабом и вратами в кантон.",
                    "Подробнее в отчёте. Жду вашего решения»."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("Large", "Первый — крупный", infoSlideId: $"{Id}_4"),
                    SlideButton.GetSetChoiceButton("Medium", "Второй — средний", infoSlideId: $"{Id}_5"),
                    SlideButton.GetSetChoiceButton("Small", "Третий — малый", infoSlideId: $"{Id}_6")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Крупный астероид",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "До двенадцати модулей добычи — как у Авалона.",
                    "Здесь можно построить настоящую промышленную базу с расчётом на десятилетия. Запасов хватит надолго. " +
                    "Но он дальше всех от Цереры — 0,6 а.е. Это больше четырёх суток пути. Зато в стороне от чужих глаз.",
                    "До Авалона — 0,1 а.е., до Феникса — 0,2 а.е.",
                    "Если готовы вкладываться в масштаб и не боитесь удалённости — это ваш выбор."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("Large")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Средний астероид",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Сбалансированный вариант. Девять модулей добычи — достаточно для устойчивого роста.",
                    "Главное преимущество — близость к Авалону. Всего 0,05 а.е. Вы будете рядом с крупнейшей станцией " +
                    "кантона. Это шанс наладить торговлю, заключить союз или использовать его инфраструктуру для " +
                    "логистики и доставки.",
                    "До Цереры — 0,45 а.е. До Феникса — 0,2 а.е.",
                    "Идеален, если не желаете рисковать."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("Medium")]);
        }

        private static Slide GetSlide6()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Малый астероид",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "Всего семь модулей добычи, но его козырь — расположение. Ближе остальных к Церере: 0,3 а.е. " +
                    "Находится прямо на маршруте к Авалону. Это делает его идеальным перевалочным пунктом. Придётся " +
                    "целиться в то, чтобы стать хабом, а не просто добывающей станцией.",
                    "До Авалона — 0,2 а.е., и до Феникса — 0,2 а.е.",
                    "Много металла не добудете, зато логистика будет дешёвой. Если хотите стать вратами в кантон — это ваш выбор."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton("Small")]);
        }

        private static DisplayInfo GetEpilog() => new(
            name: "Выбор астероида",
            imageName: ImageSet.Station_1,
            description:
            [
                "Вы отправили выбор Камилле. Транспортировка станции от Психеи к выбранному астероиду будет " +
                "оплачена из бюджета колонии."
            ]);
    }
}