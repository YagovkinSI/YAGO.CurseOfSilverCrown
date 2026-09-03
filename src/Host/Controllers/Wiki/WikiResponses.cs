using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Wiki
{
    public record WikiArticleResponse(
        string Code,
        string Name,
        string? Image,
        string[] Text);

    public record WikiSummaryResponse(
        string Code,
        string Name,
        bool IsRead);
}
