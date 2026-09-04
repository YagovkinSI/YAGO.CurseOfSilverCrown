using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Wiki.Queries.GetWikiSummaries;
using YAGO.World.Domain.Wiki;

namespace YAGO.World.Host.Controllers.Wiki
{
    public static class WikiResponseMapping
    {
        public static WikiArticleResponse ToResponse(this WikiArticle article) =>
            new(
                article.Code,
                article.DisplayInfo.Name,
                article.DisplayInfo.ImageName,
                article.DisplayInfo.Description);

        public static IReadOnlyList<WikiSummaryResponse> ToSummaryResponse(
            this IEnumerable<WikiSummaryDto> summaries) =>
            summaries.Select(summary => new WikiSummaryResponse(
                summary.Article.Code,
                summary.Article.DisplayInfo.Name,
                summary.IsRead)).ToList();
    }
}
