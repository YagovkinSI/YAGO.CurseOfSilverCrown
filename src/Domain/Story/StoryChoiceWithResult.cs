namespace YAGO.World.Domain.Story
{
    public class StoryChoiceWithResult : StoryChoice
    {
        public long NextStoreNodeId { get; }

        public StoryChoiceWithResult(
            int number,
            string text,
            long nextStoreNodeId)
            : base(number, text)
        {
            NextStoreNodeId = nextStoreNodeId;
        }
    }
}
