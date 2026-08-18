namespace YAGO.World.Host.Controllers.Common.Models
{
    public record PaginatedResponse<T>(
        T[] Data,
        int Total,
        int Page,
        int Limit);
}
