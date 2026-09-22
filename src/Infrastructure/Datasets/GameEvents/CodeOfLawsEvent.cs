using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class CodeOfLawsEvent
    {
        private const string Id = GameEventConstants.CodeOfLaws;

        private const string NoBonuses = "no_bonuses";
        private const string TaxCut = "tax_cut";
        private const string ExtendedMedicine = "extended_medicine";
        private const string AutomationSubsidies = "automation_subsidies";

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { NoBonuses, new GameAction(
                    effects: [
                        ..GetDefaultEffects()],
                    newEventCodes: [GameEventConstants.OpenColony],
                    displayInfoResult: GetEpilog("Без дополнительных бонусов", ImageSet.Cassius)) },
                { TaxCut, new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SetCorporateTaxRate, 20),
                        ..GetDefaultEffects()],
                    newEventCodes: [GameEventConstants.OpenColony],
                    displayInfoResult: GetEpilog("Снижение налогов на бизнес", ImageSet.Camilla)) },
                { ExtendedMedicine, new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SetMedicalInsuranceLevel, 1),
                        ..GetDefaultEffects()],
                    newEventCodes: [GameEventConstants.OpenColony],
                    displayInfoResult: GetEpilog("Расширенная медстраховка", ImageSet.Darius)) },
                { AutomationSubsidies, new GameAction(
                    effects: [
                        new GameEffect(GameEffectType.SetAutomationIncentive, 1),
                        ..GetDefaultEffects()],
                    newEventCodes: [GameEventConstants.OpenColony],
                    displayInfoResult: GetEpilog("Субсидии на автоматизацию", ImageSet.Lien)) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetSlides(),
                actions: changeList);
        }

        private static GameEffect[] GetDefaultEffects()
        {
            return [];
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
                title: "Свод законов",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Чтобы принимать людей и компании, колонии нужен свод законов. Консорциум даёт правителям свободу — " +
                    "каждая станция устанавливает свои правила, если они не противоречат уставу.",
                    "Законы можно менять. Но чем хаотичнее реформы, тем хуже реагируют люди. Поэтому важно задать основу " +
                    "с самого начала."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_1", "Далее")]);
        }

        private static Slide GetSlide1()
        {
            return new Slide(
                id: $"{Id}_1",
                title: "Свод законов",
                imageName: ImageSet.Cassius,
                text: new string[]
                {
                    "Кассиус изучил законодательство десятков станций — от Цереры до дальних кантонов. На основе анализа " +
                    "он представил свод законов, который, по его мнению, отлично сработает и принесёт в бюджет хорошую " +
                    "прибыль.",
                    "В ходе обсуждения появились предложения: для новой станции важно дать дополнительные бонусы, чтобы " +
                    "увеличить привлекательность. Кассиус раздражённо отстаивал ещё не существующий бюджет от лишних растрат."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Далее")]);
        }

        private static Slide GetSlide2()
        {
            return new Slide(
                id: $"{Id}_2",
                title: "Свод законов",
                imageName: ImageSet.CaptainHall,
                text: new string[]
                {
                    "Решающее слово вновь было за правителем станции. Совет ждал."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetSetChoiceButton(NoBonuses, "Без дополнительных бонусов", financierSlideId: $"{Id}_3"),
                    SlideButton.GetSetChoiceButton(TaxCut, "Снижение налогов на бизнес", administratorSlideId: $"{Id}_4"),
                    SlideButton.GetSetChoiceButton(ExtendedMedicine, "Расширенная медстраховка", socialSlideId: $"{Id}_5"),
                    SlideButton.GetSetChoiceButton(AutomationSubsidies, "Субсидии на автоматизацию", engineerSlideId: $"{Id}_6")]);
        }

        private static Slide GetSlide3()
        {
            return new Slide(
                id: $"{Id}_3",
                title: "Без дополнительных бонусов",
                imageName: ImageSet.Cassius,
                text: new string[]
                {
                    "Кассиус сидит прямо, как на аудиторской проверке, и говорит ровно, чеканя каждое слово.",
                    "«Я подробно изучил, что работает и не работает у других. Эти законы достаточно выгодны инвесторам, " +
                    "чтобы вложить средства в нашу колонию. Вложим немного денег в рекламу и индивидуальные предложения, " +
                    "если потребуется подтолкнуть инвесторов. Но делать им более выгодные предложения на годы вперёд за счёт " +
                    "нашего бюджета не вижу никакого смысла.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Назад", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(NoBonuses)]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Снижение налогов на бизнес",
                imageName: ImageSet.Camilla,
                text: new string[]
                {
                    "Камилла чуть улыбается и говорит мягко, но уверенно.",
                    "«Мы новенькие на этом поле. При равных условиях инвесторы скорее выберут колонию, которая уже доказала " +
                    "свою эффективность и стабильность. Новые колонии для них — дополнительный риск, который может покрыть " +
                    "только дополнительная прибыль. Думаю, мы можем позволить себе снизить налог на прибыль на несколько " +
                    "процентов.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Назад", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(TaxCut)]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Расширенная медстраховка",
                imageName: ImageSet.Darius,
                text: new string[]
                {
                    "Док развалился в кресле, но глаза серьёзные.",
                    "«Мне кажется, нам лучше предложить расширенную медстраховку людям. Ежегодные осмотры, профилактика " +
                    "радиации. У многих колоний есть такие привилегии — разве мы хуже? Это привлечёт колонистов, а компании " +
                    "смогут немного снизить свои траты. К тому же это вложение в будущее: оно положительно отразится " +
                    "в статистике через год-другой.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Назад", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(ExtendedMedicine)]);
        }

        private static Slide GetSlide6()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Субсидии на автоматизацию",
                imageName: ImageSet.Lien,
                text: new string[]
                {
                    "Лиен говорит быстро, чуть жестикулируя, как будто объясняет схему на доске.",
                    "«Давайте установим субсидии на автоматизацию для компаний. Меньше людей на опасных участках — меньше " +
                    "человеческого фактора. Больше автоматического контроля — меньше аварий. И компаниям выгода, " +
                    "и нам безопаснее.»"
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_2", "Назад", kind: SlideButtonKind.Return),
                    SlideButton.GetSetChoiceButton(AutomationSubsidies)]);
        }

        private static DisplayInfo GetEpilog(string choiceName, string imageName) => new(
            name: choiceName,
            imageName,
            description:
            [
                $"Вы выбрали «{choiceName}». Свод законов утверждён. Кассиус отправил документы в Консорциум — теперь " +
                $"колония может принимать людей и компании."
            ]);
    }
}