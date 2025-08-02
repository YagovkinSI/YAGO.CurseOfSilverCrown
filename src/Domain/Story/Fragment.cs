namespace YAGO.World.Domain.Story
{
    public class Fragment
    {
        public long Id { get; }
        public Slide[] Slides { get; }
        public StoryChoiceWithResult[] Choices { get; }

        public Fragment(
            long id,
            Slide[] slides,
            StoryChoiceWithResult[] choices)
        {
            Id = id;
            Slides = slides;
            Choices = choices;
        }
    }
}
