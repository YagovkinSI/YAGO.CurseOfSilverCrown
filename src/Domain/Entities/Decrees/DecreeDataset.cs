using System.Collections.Generic;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Decrees
{
    public static class DecreeDataset
    {
        public static IReadOnlyList<Decree> Get()
        {
            return
            [
                GetShowLow(),
                GetShowMedium(),
                GetShowHigh(),
                GetCredit(),
            ];
        }

        private static Decree GetShowLow()
        {
            const int actionPoints = 1;
            const int solars = 20;
            return new Decree(
                id: 1,
                name: "Локальный концерт",
                image: ImageSet.Show_StendUp,
                text: ["Провести небольшой местный концерт, чтобы поднять настроение жителеям."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ActionPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, -solars),
                    new KeyValueParameter(StateKey.MoodCurrent, 3),
                ],
                description: [
                        "Местные самодеятельные коллективы дадут бесплатный концерт в центральном атриуме. " +
                        "Бюджет уйдет только на усиление трансляции и синтезированные закуски. Жители ненадолго отвлекутся от серых будней."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints),
                    RequirementsParameter.Cost(solars)],
                additionalCheck: null);
        }

        private static Decree GetShowMedium()
        {
            const int actionPoints = 1;
            const int solars = 60;
            return new Decree(
                id: 2,
                name: "Общестанционный фестиваль",
                image: ImageSet.Show_RockConcert,
                text: ["Провести концерт с приглашением групп из соседних колоний."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ActionPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, -solars),
                    new KeyValueParameter(StateKey.MoodCurrent, 10),
                ],
                description: [
                        "Пригласите популярных исполнителей из соседних колоний и устройте голографическое шоу в куполе обзора. " +
                        "Люди будут обсуждать это событие неделями, но организаторы и артисты требуют оплаты."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints),
                    RequirementsParameter.Cost(solars)],
                additionalCheck: null);
        }

        private static Decree GetShowHigh()
        {
            const int actionPoints = 1;
            const int solars = 150;
            return new Decree(
                id: 3,
                name: "Прибытие легенды",
                image: ImageSet.Show_PopStar,
                text: ["Провести концерт с приглашением популярного исполнителя."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ActionPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, -solars),
                    new KeyValueParameter(StateKey.MoodCurrent, 30),
                ],
                description: [
                        "Орбитальная звезда, чьи песни слушали ещё на Старой Земле, согласилась дать живой концерт на вашей станции. " +
                        "Трансляция пойдет на все сектора. Такой праздник не забудет никто, но гонорар артиста и " +
                        "её охрана съедят значительную часть казны."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints),
                    RequirementsParameter.Cost(solars)],
                additionalCheck: null);
        }

        private static Decree GetCredit()
        {
            const int actionPoints = 1;
            const int solars = 10_000;
            return new Decree(
                id: 4,
                name: "Получить кредит",
                image: ImageSet.ConcEarchOffice,
                text: ["Получить дополнительные средства за счет долга станции."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ActionPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, solars),
                    new KeyValueParameter(StateKey.PublicDebt, solars)
                ],
                description: [
                        "Кредит позволит получить денежные средства, но увеличит плату по госдолгу."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints)],
                additionalCheck: (colonyState) =>
                {
                    var publicDebt = colonyState.GetPublicDebt();
                    if (!publicDebt.Check(solars))
                        throw new YagoException("Получен отказ из-за недостаточного рейинга.");
                });
        }
    }
}
