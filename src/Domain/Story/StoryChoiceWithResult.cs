using System;

namespace YAGO.World.Domain.Story
{
    public class StoryChoiceWithResult : StoryChoice
    {
        public long NextStoreNodeId { get; }
        public Action<StoryDataImmutable> Action { get; }

        public StoryChoiceWithResult(
            int number,
            string text,
            long nextStoreNodeId,
            Action<StoryDataImmutable> action)
            : base(number, text)
        {
            NextStoreNodeId = nextStoreNodeId;
            Action = action;
        }
    }
}
