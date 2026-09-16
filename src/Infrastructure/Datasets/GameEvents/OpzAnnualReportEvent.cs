using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class OpzAnnualReportEvent
    {
        private const string Id = GameEventConstants.OpzAnnualReport;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeOpz),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeColonization)],
                        newEventCodes: [],
                        requirements: [],
                        displayInfoResult: null) } };
            return new(
                code: Id,
                eventType: EventType.Default,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides() => [
                GetSlide0(),
                GetSlide1(),
                GetSlide2()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Отчет ОПЗ о колонизация Пояса",
                imageName: ImageSet.SecurityCouncil,
                text: new string[]
                {
                    "ОПЗ опубликовала ежегодный отчёт о колониях Пояса.",
                    "Орбитальное Правительство Земли (ОПЗ) — структура при ООН, созданная для координации космической " +
                    "деятельности. ОПЗ не управляет колониями напрямую, но контролирует космический лифт и потоки людей с Земли.",
                    "По данным на 2073 год, в Поясе работают 76 станций. Общее население — около 46 000 человек. " +
                    "Большинство колоний сосредоточены в трёх доменах: Цереры (32 станции), Весты (18) и Психеи (14). " +
                    "Остальные — в соседних доменах."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Отчет ОПЗ о колонизация Пояса",
                imageName: ImageSet.RegularTurn,
                text: new string[]
                {
                    "Добыча металлов, платиноидов и льда остаётся главным занятием колоний. За год добыча и обработка " +
                    "сырья выросли на 6,4%.",
                    "Но быстрее растут производство (+9,2%) и услуги (+8,9%). Пояс постепенно перестаёт быть только " +
                    "сырьевым придатком. Лидером в производстве остаётся RAS (Rinehart's Automatic Systems): его верфь " +
                    "у Психеи выпускает до четырёх станций типа «Рассвет» в год.",
                    "Главные статьи импорта — электроника, сложное оборудование и медикаменты."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Отчет ОПЗ о колонизация Пояса",
                imageName: ImageSet.MiningBrigade,
                text: new string[]
                {
                    "Товары в Поясе остаются в разы дороже, чем на Земле. Большинство колонистов работают по контракту, " +
                    "но сроки снижаются: если раньше стандартом были 5 лет, то теперь всё чаще 2–3 года.",
                    "За год население Пояса выросло на 2 700 человек — в основном за счёт прибывших с Земли. " +
                    "Умерших в Поясе за год — 257 (выросло за год на 5,7%). Родилось 188 детей (рост на 6,2%)."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetCloseNewsButton(Id, "Закрыть")]);
        }
    }
}