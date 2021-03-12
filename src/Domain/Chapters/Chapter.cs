namespace YAGO.World.Domain.Chapters
{
    public class Chapter
    {
        public long Id { get; }
        public int Number { get; }
        public string Title { get; }

        public Chapter(
            long id,
            int number,
            string title)
        {
            Id = id;
            Number = number;
            Title = title;
        }

        public static Chapter Demo => new Chapter(1, 1, "Обычное поручение");
    }
}
