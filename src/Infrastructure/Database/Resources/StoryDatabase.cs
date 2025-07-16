using System.Collections.Generic;
using YAGO.World.Domain.Story;

namespace YAGO.World.Infrastructure.Database.Resources
{
    public static class StoryDatabase
    {
        private static Dictionary<int, StoryNode> _nodes = new()
        {
            [0] = new StoryNode
            (
                id: 0,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new StoryCard(
                        0,
                        StoryResources.StoryNode_SimpleAssignment_Assignment,
                        "home"
                    ),
                    new StoryCard(
                        1,
                        StoryResources.StoryNode_SimpleAssignment_Istila,
                        "home"
                    ),
                    new StoryCard(
                        2,
                        StoryResources.StoryNode_SimpleAssignment_UpperTown,
                        "home"
                    ),
                    new StoryCard(
                        3,
                        StoryResources.StoryNode_SimpleAssignment_Market,
                        "market"
                    ),
                    new StoryCard(
                        4,
                        StoryResources.StoryNode_SimpleAssignment_Prophet,
                        "prophet"
                    ),
                    new StoryCard(
                        5,
                        StoryResources.StoryNode_SimpleAssignment_GodsWrath,
                        "prophet"
                    ),
                },
                choices: new StoryChoice[]
                {
                    new StoryChoice(1, "Послушать пророка"),
                    new StoryChoice(2, "Отправится дальше")
                }
            ),
        };

        public static Dictionary<int, StoryNode> Nodes => _nodes;
    }
}
