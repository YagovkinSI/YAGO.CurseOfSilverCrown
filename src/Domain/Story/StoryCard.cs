namespace YAGO.World.Domain.Story
{
    public class StoryCard
    {
        public int Number { get; }
        public string Text { get; }
        public string ImageName { get; }

        public StoryCard(
            int number,
            string text,
            string imageName)
        {
            Number = number;
            Text = text;
            ImageName = imageName;
        }
    }
}
