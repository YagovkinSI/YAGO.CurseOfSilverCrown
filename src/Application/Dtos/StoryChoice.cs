namespace YAGO.World.Application.Dtos
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
