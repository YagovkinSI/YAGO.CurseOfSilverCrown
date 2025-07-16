namespace YAGO.World.Domain.Story
{
    public class StoryNodeWithResults
    {
        public long Id { get; }
        public string Title { get; }
        public StoryCard[] Cards { get; }
        public StoryChoiceWithResult[] Choices { get; }

        public StoryNodeWithResults(
            long id,
            string title,
            StoryCard[] cards,
            StoryChoiceWithResult[] choices)
        {
            Id = id;
            Title = title;
            Cards = cards;
            Choices = choices;
        }
    }
}
