namespace YAGO.World.Application.Common.Pagination
{
    public record PaginatedResponse<T>(
        T[] Data,
        int Total,
        int Page,
        int Limit);
}
