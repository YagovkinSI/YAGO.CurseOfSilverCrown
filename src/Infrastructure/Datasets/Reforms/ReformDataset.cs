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
                displayInfo: new DisplayInfo(
                    "Локальный концерт",
                    ImageSet.Show_StendUp,
                    [
                        "Местные самодеятельные коллективы дадут бесплатный концерт в центральном атриуме. " +
                        "Бюджет уйдет только на усиление трансляции и синтезированные закуски. Жители ненадолго отвлекутся от серых будней."
                    ]),
                action: new GameAction(
                    changes:
                    [
                        new GameEffect(GameEffectType.SpendActionPoints, actionPoints),
                        new GameEffect(GameEffectType.SpendSolars, solars),
                        new GameEffect(GameEffectType.AddMood, 3),
                    ],
                    newEventCodes: [],
                    requirements: [
                        GameRequirement.ActionPointsMoreThan(actionPoints),
                        GameRequirement.SolarsMoreThan(solars)]));
        }

        private static Reform GetShowMedium()
        {
            const int actionPoints = 1;
            const int solars = 60;
            return new Reform(
                code: "Show_2",
                displayInfo: new DisplayInfo(
                    "Общестанционный фестиваль",
                    ImageSet.Show_RockConcert,
                    [
                        "Пригласите популярных исполнителей из соседних колоний и устройте голографическое шоу в куполе обзора. " +
                        "Люди будут обсуждать это событие неделями, но организаторы и артисты требуют оплаты."
                    ]),
                action: new GameAction(
                    changes:
                    [
                        new GameEffect(GameEffectType.SpendActionPoints, actionPoints),
                        new GameEffect(GameEffectType.SpendSolars, solars),
                        new GameEffect(GameEffectType.AddMood, 10),
                    ],
                    newEventCodes: [],
                    requirements: [
                        GameRequirement.ActionPointsMoreThan(actionPoints),
                        GameRequirement.SolarsMoreThan(solars)]));
        }

        private static Reform GetShowHigh()
        {
            const int actionPoints = 1;
            const int solars = 150;
            return new Reform(
                code: "Show_3",
                displayInfo: new DisplayInfo(
                    "Прибытие легенды",
                    ImageSet.Show_PopStar,
                    [
                        "Орбитальная звезда, чьи песни слушали ещё на Старой Земле, согласилась дать живой концерт на вашей станции. " +
                        "Трансляция пойдет на все сектора. Такой праздник не забудет никто, но гонорар артиста и " +
                        "её охрана съедят значительную часть казны."
                    ]),
                action: new GameAction(
                    changes:
                    [
                        new GameEffect(GameEffectType.SpendActionPoints, actionPoints),
                        new GameEffect(GameEffectType.SpendSolars, solars),
                        new GameEffect(GameEffectType.AddMood, 30),
                    ],
                    newEventCodes: [],
                    requirements: [
                        GameRequirement.ActionPointsMoreThan(actionPoints),
                        GameRequirement.SolarsMoreThan(solars)]));
        }

        private static Reform GetCredit()
        {
            const int actionPoints = 1;
            const int solars = 10_000;
            return new Reform(
                code: "Debt",
                displayInfo: new DisplayInfo(
                    "Получить кредит",
                    ImageSet.ConcEarchOffice,
                    [
                        "Кредит позволит получить денежные средства, но увеличит плату по госдолгу."
                    ]),
                action: new GameAction(
                    changes:
                    [
                        new GameEffect(GameEffectType.SpendActionPoints, actionPoints),
                        new GameEffect(GameEffectType.AddSolars, solars),
                        new GameEffect(GameEffectType.AddPublicDebt, solars)
                    ],
                    newEventCodes: [],
                    requirements: [
                        GameRequirement.ActionPointsMoreThan(actionPoints),
                        new GameRequirement(GameRequirementType.CreditCanTake, solars)]));
        }
    }
}
