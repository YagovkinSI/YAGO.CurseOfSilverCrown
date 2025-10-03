namespace YAGO.World.Application.Common.Pagination
{
    public record PaginatedData<T>(
        T[] Data,
        int Total,
        int Page,
        int Limit);
}
