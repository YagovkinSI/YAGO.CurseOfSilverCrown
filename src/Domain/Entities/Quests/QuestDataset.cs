using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Quests
{
    public static class QuestDataset
    {
        public static IReadOnlyList<Quest> All => [
            MvpQuest(),
            ];

        public static Quest Get(string id) => All.Single(x => x.Id == id);

        private static Quest MvpQuest()
        {
            var id = nameof(MvpQuest);
            var name = "Резолют-206";
            return new(
                id,
                name,
                QuestType.Default,
                new Slide(
                    name,
                    ImageSet.Station_1,
                    [
                        "Станция Рассвет может иметь не более 140 жилых модулей и не более 1000 жителей. " +
                        "Когда её лимит будет подходить к концу нам нужно будет перейти на станцию следующего уровня.",
                        "Станция Резолют-206 имеет более широкое колько диаметром 2 километра и расчитано на 3000 жителей. " +
                        "Это дорогостоящий переход, но если мы планируем увеличивать колонию и далее, то об этом переходе не стоит забывать."],
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.AreaCapacity_Occupied, 120),
                        new KeyValueParameter(ColonyStatNames.Economic_Reserves, 15000)],
                    continueButtonName: "Переход на следующий уровень",
                    buttons: []),
                new Episode(
                    slides: [
                        new Slide(
                            title: name,
                            imageName: ImageSet.Station_1,
                            text: [
                                "Вы прошли сложный путь от пустой конструкции в открытом космосе к колонии в несколько сотен человек. " +
                                "Вы доказали, что можетет эффективно наладить добычу ресурсов на астероиде и управлять бюджетом. Доказали," +
                                "что можете быть лидером сообщества и следить на потребностями жителей.",
                                "Многие правители Пояса справляются с этой задачей и успешных колоний на станциях типа Рассвет в Поясе " +
                                "большое количество. Но не многие решаются сделать следующий шаг. Расширить колонию до пары тысяч человек, " +
                                "превратив её из шахтёрского посёлка в настоящий городок."],
                            parameters: [],
                            continueButtonName: "Далее",
                            buttons: []),
                        new Slide(
                            title: name,
                            imageName: ImageSet.Yago,
                            text: [
                                "Разработчик:",
                                "Поздравляю! Вы прошли демонстрационную часть игры.",
                                "В будущем я продлю геймплей до станции Резолют, но на текущий момент я хочу довести текущий геймплей " +
                                "Рассвета до дейвительно интересного. Поэтому расскажите в нашей групппе ВК о том, с какими проблемами " +
                                "вы столкнулись при игре, что показалось скучным и непонятным. Это позволит мне сделать игру лушче.",
                                "Дальнейший геймплей ещё в разработке. Спасибо."],
                            parameters: [],
                            continueButtonName: "Вернуться на Рассвет",
                            buttons: [])],
                    dilemma: null));
        }
    }
}
