using System.Collections.Generic;

namespace YAGO.World.Application.Common.Pagination
{
    public record PaginatedData<T>(
        IReadOnlyList<T> Data,
        int Total,
        int Page,
        int Limit);
}
