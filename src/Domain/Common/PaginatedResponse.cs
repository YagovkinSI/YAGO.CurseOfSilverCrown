namespace YAGO.World.Domain.Common
{
    public class PaginatedResponse<T>
    {
        public T[] Data { get; }
        public int Total { get; }
        public int Page { get; }
        public int Limit { get; }

        public PaginatedResponse(
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
