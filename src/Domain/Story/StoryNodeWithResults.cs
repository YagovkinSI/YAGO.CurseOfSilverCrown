namespace YAGO.World.Domain.Story
{
    public class StoryNodeWithResults
    {
        public long Id { get; }
        public string Title { get; }
        public Slide[] Slides { get; }
        public StoryChoiceWithResult[] Choices { get; }

        public StoryNodeWithResults(
            long id,
            string title,
            Slide[] slides,
            StoryChoiceWithResult[] choices)
        {
            Id = id;
            Title = title;
            Slides = slides;
            Choices = choices;
        }
    }
}
