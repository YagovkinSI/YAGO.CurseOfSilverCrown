using System.Collections.Generic;

namespace YAGO.World.Domain.Story
{
    public class StoryDataImmutable
    {
        public Dictionary<long, int> NodesResults { get; set; }

        public StoryDataImmutable(
            Dictionary<long, int> nodesResults)
        {
            NodesResults = nodesResults ?? new Dictionary<long, int>();
        }

        public static StoryDataImmutable Empty =>
            new StoryDataImmutable(new Dictionary<long, int>());

        public void SetNodeResult(long nodeId, int number)
        {
            if (NodesResults == null)
                NodesResults = new Dictionary<long, int>();

            if (NodesResults.ContainsKey(nodeId))
                NodesResults[nodeId] = number;
            else
                NodesResults.Add(nodeId, number);
        }
    }
}
