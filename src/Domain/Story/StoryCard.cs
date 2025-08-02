using System;

namespace YAGO.World.Domain.Story
{
    public class StoryCard
    {
        public int Number { get; }
        public string[] Text { get; }
        public string ImageName { get; }

        public StoryCard(
            int number,
            string[] text,
            string imageName)
        {
            Number = number;
            Text = text;
            ImageName = imageName;
        }

        public StoryCard(
            int number,
            string text,
            string imageName)
            : this(number, SplitMultiString(text), imageName)
        { }

        private static string[] SplitMultiString(string value)
        {
            var text = string.Format(value, Environment.NewLine);
            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
