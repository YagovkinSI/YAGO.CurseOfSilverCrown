namespace YAGO.World.Domain.Story
{
    public class StoryNode
    {
        public long Id { get; }
        public string Title { get; }
        public Slide[] Slides { get; }
        public StoryChoice[] Choices { get; }

        public StoryNode(
            long id,
            string title,
            Slide[] slides,
            StoryChoice[] choices)
        {
            Id = id;
            Title = title;
            Slides = slides;
            Choices = choices;
        }
    }
}
