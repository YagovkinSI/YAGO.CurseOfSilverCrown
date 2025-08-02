using System;

namespace YAGO.World.Domain.Story
{
    public class Slide
    {
        public long Id { get; }
        public string[] Text { get; }
        public string ImageName { get; }

        public Slide(
            long id,
            string[] text,
            string imageName)
        {
            Id = id;
            Text = text;
            ImageName = imageName;
        }

        public Slide(
            long number,
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
