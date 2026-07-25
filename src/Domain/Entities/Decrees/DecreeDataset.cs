using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

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
            ];
        }

        private static Decree GetShowLow()
        {
            const int actionPoints = 4;
            const int solars = 200;
            return new Decree(
                id: 1,
                name: "Локальный концерт",
                image: ImageSet.Show_StendUp,
                text: ["Провести небольшой местный концерт, чтобы поднять настроение жителеям."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ReformPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, -solars),
                    new KeyValueParameter(StateKey.MoodReserve, 3),
                ],
                description: [
                        "Местные самодеятельные коллективы дадут бесплатный концерт в центральном атриуме. " +
                        "Бюджет уйдет только на усиление трансляции и синтезированные закуски. Жители ненадолго отвлекутся от серых будней."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints),
                    RequirementsParameter.Cost(solars)]);
        }

        private static Decree GetShowMedium()
        {
            const int actionPoints = 6;
            const int solars = 600;
            return new Decree(
                id: 2,
                name: "Общестанционный фестиваль",
                image: ImageSet.Show_RockConcert,
                text: ["Провести концерт с приглашением групп из соседних колоний."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ReformPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, -solars),
                    new KeyValueParameter(StateKey.MoodReserve, 10),
                ],
                description: [
                        "Пригласите популярных исполнителей из соседних колоний и устройте голографическое шоу в куполе обзора. " +
                        "Люди будут обсуждать это событие неделями, но организаторы и артисты требуют оплаты."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints),
                    RequirementsParameter.Cost(solars)]);
        }

        private static Decree GetShowHigh()
        {
            const int actionPoints = 8;
            const int solars = 1500;
            return new Decree(
                id: 3,
                name: "Прибытие легенды",
                image: ImageSet.Show_PopStar,
                text: ["Провести концерт с приглашением популярного исполнителя."],
                parameters:
                [
                    new KeyValueParameter(StateKey.ReformPointsCurrent, -actionPoints),
                    new KeyValueParameter(StateKey.SolarsCurrent, -solars),
                    new KeyValueParameter(StateKey.MoodReserve, 30),
                ],
                description: [
                        "Орбитальная звезда, чьи песни слушали ещё на Старой Земле, согласилась дать живой концерт на вашей станции. " +
                        "Трансляция пойдет на все сектора. Такой праздник не забудет никто, но гонорар артиста и " +
                        "её охрана съедят значительную часть казны."
                    ],
                requirements: [
                    RequirementsParameter.ActionPoints(actionPoints),
                    RequirementsParameter.Cost(solars)]);
        }
    }
}
