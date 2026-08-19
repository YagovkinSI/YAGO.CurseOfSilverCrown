using System.Collections.Generic;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Episodes;

namespace YAGO.World.Infrastructure.Datasets.GameEvents
{
    public static class MvpQuest
    {
        private const string Id = nameof(MvpQuest);
        private const string Name = "Резолют-120";
        private const int Cost = 3000;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new GameActionChance(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameAction>() {
                { "#end", new GameAction(
                    changes: [],
                    newEventCodes: [],
                    requirements: [
                        GameRequirement.SolarsMoreThan(Cost),
                        new GameRequirement(GameRequirementType.ModulesUsedMoreThan, 120)])}
            };
            return new(
                code: Id,
                eventType: EventType.Quest,
                eventOccurrenceOptions,
                slides: GetPrologSlides(changeList),
                results: GetResults());
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameAction> changeList)
        {

            return [
                new Slide(
                    id: $"{Id}_0",
                    Name,
                    ImageSet.Station_2,
                    [
                        "Станция Рассвет может иметь не более 140 жилых модулей и не более 1000 жителей. " +
                        "Когда её лимит будет подходить к концу нам нужно будет перейти на станцию следующего уровня.",
                        "Станция Резолют-120 имеет более широкое колько диаметром более 200 метров и расчитано на 3000 жителей. " +
                        "Это дорогостоящий переход, но если мы планируем увеличивать колонию и далее, то об этом переходе не стоит забывать."],
                    parameterChanges: [],
                    buttons: [
                        SlideButton.GetSetChoiceButton(
                            dilemmaResolving: "Complete",
                            name: "Перейти на следующий уровень",
                            requirements: changeList["#end"].Requirements)])];
        }

        private static Dictionary<string, GameActionResult> GetResults()
        {
            var result = GameActionResult.CreateNew(
                title: Name,
                imageName: ImageSet.Station_2,
                text: [
                    "Вы прошли сложный путь от пустой конструкции в открытом космосе к колонии в несколько сотен человек. " +
                    "Вы доказали, что можете эффективно наладить добычу ресурсов на астероиде и управлять бюджетом. Доказали, " +
                    "что можете быть лидером сообщества и следить на потребностями жителей.",
                    "Многие правители Пояса справляются с этой задачей и успешных колоний на станциях типа Рассвет в Поясе " +
                    "большое количество. Но не многие решаются сделать следующий шаг. Расширить колонию до пары тысяч человек, " +
                    "превратив её из шахтёрского посёлка в настоящий городок.",
                    "Разработчик:",
                    "Поздравляю! Вы прошли демонстрационную часть игры.",
                    "В будущем я продлю геймплей до станции Резолют, но на текущий момент я хочу довести текущий геймплей " +
                    "Рассвета до дейвительно интересного. Поэтому расскажите в нашей групппе ВК о том, с какими проблемами " +
                    "вы столкнулись при игре, что показалось скучным и непонятным. Это позволит мне сделать игру лушче.",
                    "Дальнейший геймплей ещё в разработке. Спасибо."],
                showForce: true);
            return new Dictionary<string, GameActionResult>() { { "#end", result } };
        }
    }
}
