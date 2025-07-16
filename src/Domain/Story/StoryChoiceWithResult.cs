using System;

namespace YAGO.World.Domain.Story
{
    public class StoryChoiceWithResult : StoryChoice
    {
        public long NextStoreNodeId { get; }
        public Action<StoryData> Action { get; }

        public StoryChoiceWithResult(
            int number,
            string text,
            long nextStoreNodeId,
            Action<StoryData> action)
            : base(number, text)
        {
            NextStoreNodeId = nextStoreNodeId;
            Action = action;
        }
    }
}
