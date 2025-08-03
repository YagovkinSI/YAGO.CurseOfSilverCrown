namespace YAGO.World.Domain.Story
{
    public class StoryChoice
    {
        public long FragmentId { get; }
        public string Text { get; }

        public StoryChoice(
            long number,
            string text)
        {
            FragmentId = number;
            Text = text;
        }
    }
}
