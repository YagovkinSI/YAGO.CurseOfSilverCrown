namespace YAGO.World.Domain.Common
{
    public class ListData
    {
        public ListItem[] Items { get; }
        public int Count { get; }

        public ListData(
            ListItem[] items,
            int count)
        {
            Items = items;
            Count = count;
        }

    }
}
