using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class JourneyToElevatorEvent
    {
        private const string Id = GameEventConstants.JourneyToElevator;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 1,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { "#default", new GameAction(
                    effects: [],
                    newEventCodes: [GameEventConstants.SkipPrologue]) } };
            return new(
                code: Id,
                eventType: EventType.Urgent,
                eventOccurrenceOptions,
                slides: GetPrologSlides(),
                actions: changeList);
        }

        private static Slide[] GetPrologSlides() => [
                GetSlide0(),
                GetSlide4(),
                GetSlide5(),
                GetSlide6()];

        private static Slide GetSlide0()
        {
            return new Slide(
                id: $"{Id}_0",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Вы простились с близкими и отправились в путь. Гиперзвуковой частный самолёт за несколько " +
                    "часов доставил вас на платформу космического лифта на экваторе в Тихом океане. Лифт доставит " +
                    "вас на геостационарную орбиту Земли откуда можно будет отправиться в Пояс."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_4", "Далее")]);
        }

        private static Slide GetSlide4()
        {
            return new Slide(
                id: $"{Id}_4",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Платформа лифта — это стальной остров в Тихом океане. Терминал не похож на шумные аэропорты " +
                    "Земли: функционально, тихо, без толп.",
                    "Капсула мягко трогается, и океан начинает уходить вниз. В течении первых пары часов после старта " +
                    "вы чувствуете, как гравитация постепенно исчезает, и начнётся настоящая невесомость. Остаток пути " +
                    "до геостационарной орбиты займёт почти десять дней."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_5", "Куда летят люди?"),
                    SlideButton.GetButtonToSlide($"{Id}_6", "Комфорт лифта"),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }

        private static Slide GetSlide5()
        {
            return new Slide(
                id: $"{Id}_5",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Почти 60% всех, кто покидает Землю, остаются на орбите — отели, верфи, научные станции. " +
                    "Четверть направляется в Пояс, к десяткам тысяч колонистов. Остальное — Луна (где добывают " +
                    "гелий-3 для термоядерных реакторов), Марс и дальние станции.",
                    "Вы летите к тем, кто строит новую цивилизацию."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_6", "Комфорт лифта"),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }

        private static Slide GetSlide6()
        {
            return new Slide(
                id: $"{Id}_6",
                title: "Путь в Пояс",
                imageName: ImageSet.EarthLeaving,
                text: new string[]
                {
                    "Капсула лифта — это герметичный вагон-отель на 20 человек. В бизнес-каютах — отдельные отсеки " +
                    "с панорамными окнами, столами и креслами-кроватями. Обычные каюты скромнее, но всё равно " +
                    "комфортнее любого земного перелёта.",
                    "Здесь можно работать, читать, смотреть фильмы или просто наблюдать за тем, как Земля становится " +
                    "всё меньше. У вас будет достаточно времени, чтобы привыкнуть к мысли, что вы покидаете родной мир."
                },
                parameterChanges: [],
                buttons: [
                    SlideButton.GetButtonToSlide($"{Id}_5", "Куда летят люди?"),
                    SlideButton.GetCloseNewsButton(Id, "В путь!")]);
        }
    }
}
