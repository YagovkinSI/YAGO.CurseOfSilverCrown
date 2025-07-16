namespace YAGO.World.Domain.Story
{
    public class StoryChoice
    {
        public int Number { get; }
        public string Text { get; }

        public StoryChoice(
            int number,
            string text)
        {
            Number = number;
            Text = text;
        }
    }
}
