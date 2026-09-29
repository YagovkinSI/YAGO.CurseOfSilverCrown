using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;
using YAGO.World.Domain.Stations;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class StartColonyEvent
    {
        private const string Id = GameEventConstants.StartColony;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance();
            var changeList = new Dictionary<string, GameAction>() {
                    { "#default", new GameAction(
                        effects: [
                            new GameEffect(GameEffectType.SetStation, code: StationModelConstants.Dawn_342),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeColonization),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.LifeShareholders),
                            new GameEffect(GameEffectType.UnlockWikiArticle, code: WikiArticleConstants.StationDawn),
                            new GameEffect(GameEffectType.OpenMenu, code: ColonyMenuType.Wiki.ToString())],
                        newEventCodes: [
                            GameEventConstants.MeetTheTeam,
                            GameEventConstants.NewYorkStrike,
                        ],
                        requirements: [],
                        displayInfoResult: GetEpilog()) } };
            return new(
                code: Id,
                eventType: EventType.Autostart,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides()
        {
            return [
                GetSlide1(),
                GetSlideBelt(),
                GetSlide2(),
                GetSlideRisks(),
                GetSlideStation(),
                GetSlide3(),
                GetSlideWhyMe(),
                GetSlideGameObjective()];
        }

        // Блок 1. 2073 год

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Рассвет",
                imageName: ImageSet.SpaceElevator,
                text: new string[]
                {
                    "2073 год.",
                    "Десятки тысяч людей покинули Землю, чтобы добывать ресурсы в Поясе астероидов. Здесь уже больше семидесяти станций, и каждая — как маленькое государство: свои законы, налоги, порядки.",
                    "Вы — один из акционеров Консорциума Пояса, крупнейшей компании XXI века, владеющей большей частью станций в Поясе. И сегодня вам предстоит подписать контракт, который сделает вас правителем новой станции."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1A", "О Поясе астероидов", kind: SlideButtonKind.Reference),
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее")]);
        }

        private static Slide GetSlideBelt()
        {
            return new Slide(
                id: $"{Id}_1A",
                title: "Пояс астероидов",
                imageName: ImageSet.RegularTurn,
                text: new string[]
                {
                    "Пояс астероидов — регион между Марсом и Юпитером. Изначально сюда летели за платиноидами и редкоземельными металлами, которых на Земле почти не осталось. Сегодня Пояс — это не только добыча: здесь перерабатывают ресурсы, строят станции и обеспечивают колонии почти всем необходимым.",
                    "Благодаря термоядерным двигателям путь от Земли до Пояса занимает всего один-два месяца."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее")]);
        }

        // Блок 2. Офис

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Рассвет",
                imageName: ImageSet.ConsortiumDialog,
                text: new string[]
                {
                    "В офисе Консорциума координатор поясняет вам детали контракта.",
                    "«Станция будет запущена через четыре месяца. До этого вам предстоит выбрать астероид, нанять базовый персонал и составить план развития. Помогать будут советники, отобранные Консорциумом. Каждый — специалист в своём деле. Но решающее слово всегда за вами.",
                    "Консорциум ожидает, что за год доходы колонии превысят издержки. Это ваша основная цель»."
                },
                parameterChanges: [],
                buttons: GetSlide2Buttons());
        }

        private static Slide GetSlideRisks()
        {
            return new Slide(
                id: $"{Id}_2A",
                title: "Если не выполню цель?",
                imageName: ImageSet.ConsortiumDialog2,
                text: new string[]
                {
                    "«Консорциум пришлёт аудит и временно отстранит вас от правления, чтобы разобраться в причинах и помочь исправить ситуацию.",
                    "Если это не мошенничество с вашей стороны или со стороны команды, а неудачное стечение обстоятельств — полномочия вернут.»"
                },
                parameterChanges: [],
                buttons: GetSlide2Buttons($"{Id}_2A"));
        }

        private static Slide GetSlideStation()
        {
            return new Slide(
                id: $"{Id}_2B",
                title: "Станция «Рассвет»",
                imageName: ImageSet.Station_1,
                text: new string[]
                {
                    "«Рассвет» — самая популярная модель в Поясе. Вращающееся кольцо диаметром 150 метров создаёт гравитацию 0,85 g. Внутри — центральная улица, модульные трёхэтажные секции, компактный ядерный реактор и замкнутая система рециркуляции воды и воздуха — станция способна существовать автономно месяцами.",
                    "Полная застройка вмещает до тысячи человек. Стартовая — около ста пятидесяти."
                },
                parameterChanges: [],
                buttons: GetSlide2Buttons($"{Id}_2B"));
        }

        private static SlideButton[] GetSlide2Buttons(string? excludeSlideId = null)
        {
            List<SlideButton> allButtons = [
                SlideButton.GetButtonToSlide($"{Id}_2A", "Если не выполню цель?"),
                SlideButton.GetButtonToSlide($"{Id}_2B", "О станции", kind: SlideButtonKind.Reference),
                SlideButton.GetButtonToSlide($"{Id}_3", "Далее")];

            return excludeSlideId != null
                ? allButtons.Where(x => x.ToSlide?.SlideId != excludeSlideId).ToArray()
                : allButtons.ToArray();
        }

        // Блок 3. Подпись

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Рассвет",
                imageName: ImageSet.ConsortiumDialog2,
                text: new string[]
                {
                    "Координатор разворачивает дисплей. На нём — контракт и поле для отпечатка.",
                    "«Осталась только ваша подпись. После неё вы — правитель колонии в Поясе Астероидов. Если остались вопросы, я готов ответить.»",
                    "Он откидывается в кресле. Ждёт."
                },
                parameterChanges: [],
                buttons: GetSlide3Buttons());
        }

        private static Slide GetSlideWhyMe()
        {
            return new Slide(
                id: $"{Id}_3A",
                title: "Почему я?",
                imageName: ImageSet.ConsortiumDialog,
                text: new string[]
                {
                    "Консорциум предлагает посты правителей акционерам по очереди, начиная с самых крупных. Но верхушка топа — люди, у которых уже есть всё, что нужно. Они предпочитают оставаться на Земле, получая дивиденды без лишних хлопот. Очередь дошла до вас — и это отличный шанс.",
                    "К этому моменту уже больше пятидесяти акционеров согласились стать правителями — и их станции работают. Теперь ваш черёд."
                },
                parameterChanges: [],
                buttons: GetSlide3Buttons($"{Id}_3A"));
        }

        private static Slide GetSlideGameObjective()
        {
            return new Slide(
                id: $"{Id}_3B",
                title: "Зачем мне это?",
                imageName: ImageSet.ConsortiumDialog2,
                text: new string[]
                {
                    "Правитель получает фиксированную зарплату из бюджета колонии — около полумиллиона долларов в год до налогов. Если колония процветает, растёт и зарплата.",
                    "Но дело не только в деньгах. Консорциум даёт правителям свободу: вы сами решаете, где ставить станцию, кого нанимать, по каким законам жить колонии.",
                    "Немногие в Поясе обладают такой властью. И немногие оставляют след в его истории."
                },
                parameterChanges: [],
                links: [new SlideLink(SlideLinkType.Rating)],
                buttons: GetSlide3Buttons($"{Id}_3B"));
        }

        private static SlideButton[] GetSlide3Buttons(string? excludeSlideId = null)
        {
            List<SlideButton> allButtons = [
                SlideButton.GetButtonToSlide($"{Id}_3A", "Почему я?"),
                SlideButton.GetButtonToSlide($"{Id}_3B", "Зачем мне это?"),
                SlideButton.GetCloseNewsButton(Id, "Подписать контракт")];

            return excludeSlideId != null
                ? allButtons.Where(x => x.ToSlide?.SlideId != excludeSlideId).ToArray()
                : allButtons.ToArray();
        }

        private static DisplayInfo? GetEpilog()
        {
            return new DisplayInfo(
                "Контракт подписан",
                ImageSet.ConsortiumDialog,
                [
                    "Сканер вспыхивает зелёным. Контракт зарегистрирован.",
                    "«Поздравляю. Вскоре мы добавим вас в систему и познакомим с советниками. Желаю вам успехов»."
                ]);
        }
    }
}