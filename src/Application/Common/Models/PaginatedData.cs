namespace YAGO.World.Application.Common.Models
{
    public class PaginatedData<T>
    {
        public T[] Data { get; }
        public int Total { get; }
        public int Page { get; }
        public int Limit { get; }

        public PaginatedData(
            T[] data,
            int total,
            int page,
            int limit)
        {
            Data = data;
            Total = total;
            Page = page;
            Limit = limit;
        }
    }
}
