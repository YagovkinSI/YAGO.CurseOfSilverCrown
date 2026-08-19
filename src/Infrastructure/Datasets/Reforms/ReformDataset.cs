using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.Reforms;

namespace YAGO.World.Infrastructure.Datasets.Reforms
{
    internal static class ReformDataset
    {
        public static IReadOnlyList<Reform> All =>
        [
            GetShowLow(),
            GetShowMedium(),
            GetShowHigh(),
            GetCredit(),
        ];

        public static Reform Get(string code)
        {
            var reform = All.SingleOrDefault(x => x.Code == code)
                ?? throw new YagoNotFoundException(nameof(Reform), code.ToString());
            return reform;
        }

        private static Reform GetShowLow()
        {
            const int actionPoints = 1;
            const int solars = 20;
            return new Reform(
                code: "Show_1",
                name: "Локальный концерт",
                image: ImageSet.Show_StendUp,
                text: ["Провести небольшой местный концерт, чтобы поднять настроение жителеям."],
                changes:
                [
                    GameParameterChanging.CreateNumberChanging(GameParameterType.ActionPointsCurrent, -actionPoints),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.SolarsCurrent, -solars),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.MoodCurrent, 3),
                ],
                description: [
                        "Местные самодеятельные коллективы дадут бесплатный концерт в центральном атриуме. " +
                        "Бюджет уйдет только на усиление трансляции и синтезированные закуски. Жители ненадолго отвлекутся от серых будней."
                    ],
                requirements: [
                    GameParameterRequirement.ActionPoints(actionPoints),
                    GameParameterRequirement.Cost(solars)],
                additionalCheck: null);
        }

        private static Reform GetShowMedium()
        {
            const int actionPoints = 1;
            const int solars = 60;
            return new Reform(
                code: "Show_2",
                name: "Общестанционный фестиваль",
                image: ImageSet.Show_RockConcert,
                text: ["Провести концерт с приглашением групп из соседних колоний."],
                changes:
                [
                    GameParameterChanging.CreateNumberChanging(GameParameterType.ActionPointsCurrent, -actionPoints),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.SolarsCurrent, -solars),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.MoodCurrent, 10),
                ],
                description: [
                        "Пригласите популярных исполнителей из соседних колоний и устройте голографическое шоу в куполе обзора. " +
                        "Люди будут обсуждать это событие неделями, но организаторы и артисты требуют оплаты."
                    ],
                requirements: [
                    GameParameterRequirement.ActionPoints(actionPoints),
                    GameParameterRequirement.Cost(solars)],
                additionalCheck: null);
        }

        private static Reform GetShowHigh()
        {
            const int actionPoints = 1;
            const int solars = 150;
            return new Reform(
                code: "Show_3",
                name: "Прибытие легенды",
                image: ImageSet.Show_PopStar,
                text: ["Провести концерт с приглашением популярного исполнителя."],
                changes:
                [
                    GameParameterChanging.CreateNumberChanging(GameParameterType.ActionPointsCurrent, -actionPoints),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.SolarsCurrent, -solars),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.MoodCurrent, 30),
                ],
                description: [
                        "Орбитальная звезда, чьи песни слушали ещё на Старой Земле, согласилась дать живой концерт на вашей станции. " +
                        "Трансляция пойдет на все сектора. Такой праздник не забудет никто, но гонорар артиста и " +
                        "её охрана съедят значительную часть казны."
                    ],
                requirements: [
                    GameParameterRequirement.ActionPoints(actionPoints),
                    GameParameterRequirement.Cost(solars)],
                additionalCheck: null);
        }

        private static Reform GetCredit()
        {
            const int actionPoints = 1;
            const int solars = 10_000;
            return new Reform(
                code: "Debt",
                name: "Получить кредит",
                image: ImageSet.ConcEarchOffice,
                text: ["Получить дополнительные средства за счет долга станции."],
                changes:
                [
                    GameParameterChanging.CreateNumberChanging(GameParameterType.ActionPointsCurrent, -actionPoints),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.SolarsCurrent, solars),
                    GameParameterChanging.CreateNumberChanging(GameParameterType.PublicDebt, solars)
                ],
                description: [
                        "Кредит позволит получить денежные средства, но увеличит плату по госдолгу."
                    ],
                requirements: [
                    GameParameterRequirement.ActionPoints(actionPoints)],
                additionalCheck: (colonyState) =>
                {
                    var publicDebt = colonyState.GetPublicDebt();
                    if (!publicDebt.Check(solars))
                        throw new YagoException("Получен отказ из-за недостаточного рейинга.");
                });
        }
    }
}
