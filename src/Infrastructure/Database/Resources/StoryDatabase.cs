using System.Collections.Generic;
using YAGO.World.Domain.Story;
using YAGO.World.Domain.Story.StoryEvents;

namespace YAGO.World.Infrastructure.Database.Resources
{
    public static class StoryDatabase
    {
        private static StoryNodeWithResults NodeInProgress(long id)
        {
            return new StoryNodeWithResults
            (
                id,
                "В разработке",
                new StoryCard[]
                {
                    new(
                        0,
                        "Продолжение следует... Ожидайте обновлений.",
                        "home"
                    )
                },
                new StoryChoiceWithResult[0]
            );
        }

        public static Dictionary<long, StoryNodeWithResults> Nodes { get; } = new()
        {
            [0] = new StoryNodeWithResults
            (
                id: 0,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new(0, StoryResources.StoryNode_0_0, "UpperTown"),
                    new(1, StoryResources.StoryNode_0_1, "UpperTown"),
                    new(2, StoryResources.StoryNode_0_2, "EirusTemple"),
                    new(3, StoryResources.StoryNode_0_3, "market"),
                    new(4, StoryResources.StoryNode_0_4, "prophet"),
                    new(5, StoryResources.StoryNode_0_5, "prophet"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "Прислушаться к словам проповедника", 1,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.ListenedToSermonAtMarket, true); }),
                    new(
                        2, "Протиснуться сквозь толпу к торговцам специями", 2,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.ListenedToSermonAtMarket, false); })
                }
            ),

            [1] = new StoryNodeWithResults
            (
                id: 0,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new(0, StoryResources.StoryNode_1_0, "prophet"),
                    new(1, StoryResources.StoryNode_1_1, "prophet"),
                    new(2, StoryResources.StoryNode_1_2, "prophet"),
                    new(3, StoryResources.StoryNode_1_3, "prophet"),
                    new(4, StoryResources.StoryNode_1_4, "prophet"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "[Мысли] Это бред! Эльниры принесли нам знания и порядок", 2,
                        (data) => {
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Elnirs, 100);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Magic, 60);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Tieflings, 10);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Dragons, 10);
                        }),
                    new(
                        2, "[Мысли] Боги... Я действительно служу темным силам?", 2,
                        (data) => {
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Elnirs, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Magic, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Tieflings, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Dragons, -50);
                        }),
                    new(
                        2, "[Мысли] Нужно узнать больше, прежде чем судить", 2,
                        (data) => { })
                }
            ),

            [2] = NodeInProgress(2),
        };
    }
}
