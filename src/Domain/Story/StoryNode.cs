namespace YAGO.World.Domain.Story
{
    public class StoryNode
    {
        public long Id { get; }
        public string Title { get; }
        public StoryCard[] Cards { get; }
        public StoryChoice[] Choices { get; }

        public StoryNode(
            long id,
            string title,
            StoryCard[] cards,
            StoryChoice[] choices)
        {
            Id = id;
            Title = title;
            Cards = cards;
            Choices = choices;
        }
    }
}
