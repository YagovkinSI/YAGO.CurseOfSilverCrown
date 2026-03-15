using YAGO.World.Domain.Common;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Decrees
{
    public static class DecreeDataset
    {
        public static Decree[] Get()
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
            return new Decree(
                id: 1,
                name: "Локальный концерт",
                image: ImageSet.Show_StendUp,
                text: ["Провести небольшой местный концерт, чтобы поднять настроение жителеям."],
                parameters:
                [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -200),
                    new KeyValueParameter(ColonyStatNames.Mood_Total, 3),
                ],
                description: [
                        "Местные самодеятельные коллективы дадут бесплатный концерт в центральном атриуме. Бюджет уйдет только на усиление трансляции и синтезированные закуски. Жители ненадолго отвлекутся от серых будней."
                    ]);
        }

        private static Decree GetShowMedium()
        {
            return new Decree(
                id: 2,
                name: "Общестанционный фестиваль",
                image: ImageSet.Show_RockConcert,
                text: ["Провести концерт с приглашением групп из соседних колоний."],
                parameters:
                [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -600),
                    new KeyValueParameter(ColonyStatNames.Mood_Total, 10),
                ],
                description: [
                        "Пригласите популярных исполнителей из соседних колоний и устройте голографическое шоу в куполе обзора. Люди будут обсуждать это событие неделями, но организаторы и артисты требуют оплаты."
                    ]);
        }

        private static Decree GetShowHigh()
        {
            return new Decree(
                id: 3,
                name: "Прибытие легенды",
                image: ImageSet.Show_PopStar,
                text: ["Провести концерт с приглашением популярного исполнителя."],
                parameters:
                [
                    new KeyValueParameter(ColonyStatNames.Economic_Reserves, -1500),
                    new KeyValueParameter(ColonyStatNames.Mood_Total, 30),
                ],
                description: [
                        "Орбитальная звезда, чьи песни слушали ещё на Старой Земле, согласилась дать живой концерт на вашей станции. Трансляция пойдет на все сектора. Такой праздник не забудет никто, но гонорар артиста и её охрана съедят значительную часть казны."
                    ]);
        }
    }
}
