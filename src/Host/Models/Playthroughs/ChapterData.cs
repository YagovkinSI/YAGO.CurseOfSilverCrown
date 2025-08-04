namespace YAGO.World.Host.Models.Playthroughs
{
    public class ChapterData
    {
        public int Number { get; }
        public string Title { get; }

        public ChapterData(
            int number, 
            string title)
        {
            Number = number;
            Title = title;
        }
    }
}
