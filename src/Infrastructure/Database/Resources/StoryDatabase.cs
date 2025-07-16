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
                    new(0, StoryResources.StoryNode_SimpleAssignment_Assignment, "home"),
                    new(1, StoryResources.StoryNode_SimpleAssignment_Istila, "home"),
                    new(2, StoryResources.StoryNode_SimpleAssignment_UpperTown, "home"),
                    new(3, StoryResources.StoryNode_SimpleAssignment_Market, "market"),
                    new(4, StoryResources.StoryNode_SimpleAssignment_Prophet, "prophet"),
                    new(5, StoryResources.StoryNode_SimpleAssignment_GodsWrath, "prophet"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "Прислушаться к словам проповедника", 1,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.ListenedToSermonAtMarket, true); }),
                    new(
                        2, "Протиснуться сквозь толпу к торговцам специями", 3,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.ListenedToSermonAtMarket, false); })
                }
            ),
            [1] = NodeInProgress(1),
            [2] = NodeInProgress(2),
        };
    }
}
