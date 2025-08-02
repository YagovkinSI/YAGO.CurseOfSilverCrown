namespace YAGO.World.Domain.Story
{
    public class Fragment
    {
        public long Id { get; }
        public string Title { get; }
        public Slide[] Slides { get; }
        public StoryChoiceWithResult[] Choices { get; }

        public Fragment(
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
