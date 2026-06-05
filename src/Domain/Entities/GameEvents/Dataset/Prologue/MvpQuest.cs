using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue
{
    public static class MvpQuest
    {
        private const string Id = nameof(MvpQuest);
        private const string Name = "Резолют-206";
        private const int ActionPoints = 7;
        private const int Cost = 10000;

        public static GameEvent Get()
        {
            var eventOccurrenceOptions = new EventOccurrenceOptions(
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: []);
            var changeList = new Dictionary<string, GameEventChangeList>() {
                { "end", new GameEventChangeList(
                    colonyStats: [
                        new KeyValueParameter(ColonyStatNames.ActionPoints_Resourses, ActionPoints),
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 120),
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, Cost)],
                    newQuests: [ ],
                    availableRequirements: [
                        ActionAvailableRequirement.ActionPoints(ActionPoints),
                        ActionAvailableRequirement.Cost(Cost),
                        new ActionAvailableRequirement(
                            new RequirementsParameter(ColonyStatNames.AreaCapacity_Occupied, 120),
                            "Занято мало пространства")])}
            };
            return new(
                id: Id,
                eventOccurrenceOptions,
                episode: GetEpisode(changeList),
                epilog: GetEpilog());
        }

        private static Episode GetEpisode(Dictionary<string, GameEventChangeList> changeList)
        {
            return new Episode(
                slides: GetPrologSlides(changeList));
        }

        private static Slide[] GetPrologSlides(Dictionary<string, GameEventChangeList> changeList)
        {

            return [
                new Slide(
                    id: $"{Id}_0",
                    Name,
                    ImageSet.Station_1,
                    [
                        "Станция Рассвет может иметь не более 140 жилых модулей и не более 1000 жителей. " +
                        "Когда её лимит будет подходить к концу нам нужно будет перейти на станцию следующего уровня.",
                        "Станция Резолют-206 имеет более широкое колько диаметром 2 километра и расчитано на 3000 жителей. " +
                        "Это дорогостоящий переход, но если мы планируем увеличивать колонию и далее, то об этом переходе не стоит забывать."],
                    parameters: changeList["end"].ColonyStats,
                    buttons: [
                        SlideButton.GetSetChoiceButton(
                            Id,
                            dilemmaResolving: "Complete",
                            name: "Перейти на следующий уровень",
                            availableRequirements: changeList["end"].AvailableRequirements)])];
        }

        private static Episode GetEpilog()
        {
            return new Episode(
                slides: [
                    new Slide(
                        id: $"{Id}_0",
                        title: Name,
                        imageName: ImageSet.Station_1,
                        text: [
                            "Вы прошли сложный путь от пустой конструкции в открытом космосе к колонии в несколько сотен человек. " +
                            "Вы доказали, что можете эффективно наладить добычу ресурсов на астероиде и управлять бюджетом. Доказали, " +
                            "что можете быть лидером сообщества и следить на потребностями жителей.",
                            "Многие правители Пояса справляются с этой задачей и успешных колоний на станциях типа Рассвет в Поясе " +
                            "большое количество. Но не многие решаются сделать следующий шаг. Расширить колонию до пары тысяч человек, " +
                            "превратив её из шахтёрского посёлка в настоящий городок."],
                        parameters: [],
                        buttons: [
                            SlideButton.GetButtonToSlide($"{Id}_1")]),
                    new Slide(
                        id: $"{Id}_1",
                        title: Name,
                        imageName: ImageSet.Yago,
                        text: [
                            "Разработчик:",
                            "Поздравляю! Вы прошли демонстрационную часть игры.",
                            "В будущем я продлю геймплей до станции Резолют, но на текущий момент я хочу довести текущий геймплей " +
                            "Рассвета до дейвительно интересного. Поэтому расскажите в нашей групппе ВК о том, с какими проблемами " +
                            "вы столкнулись при игре, что показалось скучным и непонятным. Это позволит мне сделать игру лушче.",
                            "Дальнейший геймплей ещё в разработке. Спасибо."],
                        parameters: [],
                        buttons: [])]);
        }
    }
}
